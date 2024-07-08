var IDPROGRAM;
var NAMAPROGRAM;

$(document).ready(function () {

    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (results == null) {
            return null;
        }
        return decodeURI(results[1]) || 0;
    }
    IDPROGRAM = $.urlParam('id_program');
    NAMAPROGRAM = $.urlParam('nama_program');
    $('#Nama_Program').append(`Riwayat Update Progress Program ${NAMAPROGRAM}`)
    detailHistoryProgressDialog();
});

function detailHistoryProgressDialog() {
    getListDetailInternalProgress();
}

function updateProgressDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#modalUpdateProgress-monitoring').modal('show');
    $('#Id_Program').val(IDPROGRAM);
    getListItemKebutuhan();
    $('#UpdateProgress-edit_btn').on('click', function () {
        tambahUpdateProgress();
    });
}

//List Item
function getListItemKebutuhan() {
    var param = {
        filter: [
            {
                field: "",
                search: ""
            }
        ],
        sort: {
            field: "id",
            type: 0
        },
        start: 1,
        length: 0,
        id_program: $('#Id_Program').val()
    }
    RequestData('GET', `/v1/Monitoring/list_item/${IDPROGRAM}`, '', null, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $('#Update-Progress-Item').html("");
            $('#Update-Progress-Item').append(`<option value=""> -- Pilih Item --</option>`)
            data.List.forEach(function (item) {
                console.log("data id", item.Item.Id);
                $('#Update-Progress-Item').append(`<option value="${item.Item.Id}">${item.Item.Nama} </option>`);
            })
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });

}

//tambah update progress
function tambahUpdateProgress() {
    if ($('#Update-Progress-Persentase').val() > 100) {
        ConfirmMessage('Progress lebih dari 100 atau lebih kecil dari progress sebelumnya');
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var submitAttachment = "";
            var file_attach = $('#Update-Progress-Lampiran')[0].files[0];
            if (file_attach != undefined && file_attach != null) {
                await FileToBase64(file_attach)
                    .then(dataBase64 => submitAttachment = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }

            var param = {
                IdProgramItem: $('#Update-Progress-Item').val(),
                Deskripsi: $('#Update-Progress-Deskripsi').val(),
                Progress: $('#Update-Progress-Persentase').val(),
                Media: [
                    {
                        Filename: file_attach.name,
                        Base64: submitAttachment
                    }
                ]
            }
            var table = $('#DetailInternalProgress-table');

            RequestData('PUT', `/v1/Monitoring/update_progress`, '#modalUpdateProgress-monitoring .modal-content', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#modalUpdateProgress-monitoring').modal('hide');
                    getListDetailInternalProgress();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}

//Tabel History
function getListDetailInternalProgress(page) {

    page = page != undefined ? page : 1;
    var pageSize = $('#DetailInternalProgress-page_select').val();
    var element = {
        table: '#DetailInternalProgress-table',
        tbody: '#DetailInternalProgress-tbody',
        from: '#DetailInternalProgress-from_page',
        to: '#DetailInternalProgress-to_page',
        total: '#DetailInternalProgress-total',
        pagination: '#DetailInternalProgress-pagination',
        item_pagination: 'DetailInternalProgress-item'
    }
    var param = {
        search: "",
        id_program: IDPROGRAM,
        start_date: "2022-10-21",
        end_date: "2022-10-15",
        start: page,
        length: pageSize,
        sort: {
            field: "id",
            type: 0
        }
    }

    RequestData('GET', `/v1/Monitoring/list_progress/${IDPROGRAM}`, element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log('data', data.Count);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListDetailInternalProgress'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                                <tr>
                                    <td class="text-center">${count}</td>
                                    <td class="text-center">${DateToStringFormat(item.CreateDate)}</td>
                                    <td class="text-center">${item.Item.Nama}</td>
                                    <td class="text-center">${item.Progress} %</td>
                                    <td class="text-center">${item.Deskripsi}</td>
                                    <td class="text-center"><a href="#" type="${item.Media[0].Id}"  onclick="LampiranProgress(this)">${item.Media[0].Filename}</a</td>
                                </tr>
                            `);
                        if (item.Progress >= 100) {
                            $("button").remove(".btnUpdate");
                            $('#btnClose').append(`
                                <div class="col mx-3 border border-dark border-2 rounded-2">
                                    <div class="row mt-2">
                                        <div class="col">
                                            <h6>Progress telah mencapai 100% segera upload dokumen BAST</p>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-12">
                                            <label>Deskripsi :</label>
                                            <textarea class="form-control form-control-sm clear" id="Close-Progress-Deskripsi"></textarea>
                                        </div>
                                    </div>
                                    <div class="row my-2">
                                        <div class="form-group col-12">
                                            <label>Upload Dokumen BAST</label>
                                            <input type="file" class="form-control form-control-sm clear" id="Close-Progress-Lampiran" />
                                        </div>
                                    </div>
                                    <div class="row my-2">
                                        <label>Status : -</label>
                                    </div>
                                    <div class="row my-2">
                                        <label>Catatan : -</label>
                                    </div>
                                    <div class="row my-2">
                                        <div class="form-group col-md-12 text-end">
                                            <button type="submit" class="btn btn-sm btn-success" id="CloseProgress-edit_btn">Submit</button>
                                        </div>
                                    </div>
                                </div>
                            `);
                        }
                        count++;

                    });
                }
                )
            } else {
                SetTableData(false, 8, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }

    });
}

function tambahCloseProgress() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var submitAttachment = "";
            var file_attach = $('#Close-Progress-Lampiran')[0].files[0];
            if (file_attach != undefined && file_attach != null) {
                await FileToBase64(file_attach)
                    .then(dataBase64 => submitAttachment = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }

            var param = {
                IdProgramItem: $('#Close-Progress-Item').val(),
                Deskripsi: $('#Close-Progress-Deskripsi').val(),
                Progress: $('#Close-Progress-Persentase').val(),
                Media: [
                    {
                        Filename: file_attach.name,
                        Base64: submitAttachment
                    }
                ]
            }

            RequestData('PUT', `/v1/Monitoring/Close_progress`, '#modalCloseProgress-monitoring .modal-content', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Pengajuan Closing Berhasil", "success");
                    $('#modalCloseProgress-monitoring').modal('hide');
                    getListMonitoringInternal();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}

//download lampiran
function LampiranProgress(Id) {
    console.log("id : ", Id.type);
    if (Id.type == "") {
        AlertMessage("Lampiran Not Found");
    } else {
        RequestData('GET', `/v1/Media/download/${Id.type}`, '', null, null, function (data_obj) {
            if (data_obj.Succeeded) {
                Base64ToFile(data_obj.Data.Filename, "application/octet-stream", data_obj.Data.Base64);
                /*            $('#Detail-LampiranProgram').text(data_obj.Data.Filename, 'data:application/pdf;base64,' + data_obj.Data.Base64);*/
            }
            else if (data_obj.code == 401) { //unathorized
                AlertMessage(data_obj.message);
            } else
                ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
        });
    }
}