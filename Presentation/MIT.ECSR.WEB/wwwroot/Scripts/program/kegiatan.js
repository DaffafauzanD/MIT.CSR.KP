Dropzone.autoDiscover = false;
var lampiran = [];
var lampiranEdit = [];
$(document).ready(function () {
    ListKegiatan();
    SetDatePicker('#AddKegiatan-StartTglPelaksanaan', "DD MMMM, YYYY", null, null, false);
    SetDatePicker('#AddKegiatan-EndTglPelaksanaan', "DD MMMM, YYYY", null, null, false);
    InitAddDropzone();
    InitEditDropzone();
});
function ListKegiatan(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#Kegiatan-page_select').val();
    var element = {
        table: '#Kegiatan-table',
        tbody: '#Kegiatan-tbody',
        from: '#Kegiatan-from_page',
        to: '#Kegiatan-to_page',
        total: '#Kegiatan-total',
        pagination: '#Kegiatan-pagination',
        item_pagination: 'Kegiatan-item'
    }
    var param = {
        filter: [
            {
                field: "idprogram",
                search: $('#EditProgram-Id').val()
            }
        ],
        sort: {
            field: "createdate",
            type: 1
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', '/v1/ProgramItem/list', element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.succeeded) {
            console.log(data);
            if (data.list.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListKegiatan'
                }, function (count) {
                    data.list.forEach(function (item) {
                        let elLampiran = `Tidak ada lampiran`;
                        if (item.lampiran) {
                            elLampiran = `<ul style="list-style-type: none;padding-left: 0;">`;
                            $.each(item.lampiran, (i, item) => {
                                elLampiran += `<li><a href="${item.original}" target="_blank">${item.filename}</a></li>`;
                            });
                            elLampiran += `</ul>`;
                        }
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.nama}</td>
                                        <td>
                                            <ul style="list-style-type: none;padding-left: 0;">
                                                <li>${DateToStringFormat(item.startTglPelaksanaan)}</li>
                                                <li>${DateToStringFormat(item.endTglPelaksanaan)}</li>
                                            </ul>
                                        </td>
                                        <td>${item.lokasi ?? "-"}</td>
                                        <td>
                                            ${elLampiran}
                                        </td>
                                        <td>${item.jumlah} ${item.satuanUnit}</td>
                                        <td class="text-right">Rp. ${formatNumber(item.rupiah)}</td>
                                        <td class="text-center">
                                            <div>
                                                <button type="button" data-toggle="dropdown" class="btn btn-sm btn-info box-shadow-2" aria-hashpopup="true" aria-expanded="false"><i class="ft-list"></i></button>
                                                <div class="dropdown-menu">
                                                    <a href="#!" class="dropdown-item" onclick="EditKegiatanDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                                    <a href="#!" class="dropdown-item text-danger" onclick="deleteKegiatan('${item.id}');">Delete</a>
                                                </div>
                                            </div>
                                        </td>
                                  </tr>
                            `);
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

    })
}
function EditKegiatanDialog(el) {
    InitEditDropzone();
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#md-EditKegiatan').modal('show');
    $('#EditKegiatan-Id').val(data.id);
    $('#EditKegiatan-Nama').val(data.nama);
    $('#EditKegiatan-Qty').val(data.jumlah);
    $('#EditKegiatan-Harga').val(data.rupiah);
    $('#EditKegiatan-Satuan').val(data.satuanUnit);
    $('#EditKegiatan-StartTglPelaksanaan').val(DateToStringFormat(data.startTglPelaksanaan));
    $('#EditKegiatan-EndTglPelaksanaan').val(DateToStringFormat(data.endTglPelaksanaan));
    $('#EditKegiatan-Lokasi').val(data.lokasi);

    $.each(data.lampiran, function (i, item) {
        var file = {
            name: item.filename,
            size: 100,
            status: Dropzone.ADDED,
            accepted: true,
            id: item.id,
            isEdit: true
        };
        Dropzone.forElement("#EditKegiatan-Lampiran").emit("addedfile", file);
        Dropzone.forElement("#EditKegiatan-Lampiran").emit("thumbnail", file, item.original);
        Dropzone.forElement("#EditKegiatan-Lampiran").emit("complete", file);
    })
}
function AddKegiatan() {
    if (!FormValidate('md-AddKegiatan')) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (!isConfirm) {
            return;
        }
        var param = {
            idProgram: $('#EditProgram-Id').val(),
            jumlah: $('#AddKegiatan-Qty').val(),
            nama: $('#AddKegiatan-Nama').val(),
            rupiah: $('#AddKegiatan-Harga').val(),
            satuanUnit: $('#AddKegiatan-Satuan').val(),
            startTglPelaksanaan: StringToDateFormat($('#AddKegiatan-StartTglPelaksanaan').val(), "DD MMMM, YYYY"),
            endTglPelaksanaan: StringToDateFormat($('#AddKegiatan-EndTglPelaksanaan').val(), "DD MMMM, YYYY"),
            lokasi: $('#AddKegiatan-Lokasi').val(),
            lampiran: lampiran
        }
        RequestData('POST', `/v1/ProgramItem/add`, '#md-AddKegiatan', null, JSON.stringify(param), function (data_obj) {
            if (data_obj.succeeded) {
                $('.clear').val('')
                $('#md-AddKegiatan').modal('hide');
                AlertMessage("Data Berhasil Disimpan", null, "success");
                ListKegiatan();
                lampiran = [];
                InitAddDropzone();
            }
            else
                AlertMessage(data_obj.message);
        });
    });
}
function EditKegiatan() {
    if (!FormValidate('md-EditKegiatan')) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (!isConfirm) {
            return;
        }
        var id = $('#EditKegiatan-Id').val();
        var param = {
            idProgram: $('#EditProgram-Id').val(),
            jumlah: $('#EditKegiatan-Qty').val(),
            nama: $('#EditKegiatan-Nama').val(),
            rupiah: $('#EditKegiatan-Harga').val(),
            satuanUnit: $('#EditKegiatan-Satuan').val(),
            startTglPelaksanaan: StringToDateFormat($('#EditKegiatan-StartTglPelaksanaan').val(), "DD MMMM, YYYY"),
            endTglPelaksanaan: StringToDateFormat($('#EditKegiatan-EndTglPelaksanaan').val(), "DD MMMM, YYYY"),
            lokasi: $('#EditKegiatan-Lokasi').val(),
            lampiran: lampiranEdit
        }
        RequestData('PUT', `/v1/ProgramItem/edit/${id}`, '#md-EditKegiatan', null, JSON.stringify(param), function (data_obj) {
            if (data_obj.succeeded) {
                $('.clear').val('')
                $('#md-EditKegiatan').modal('hide');
                AlertMessage("Data Berhasil Dirubah", null, "success");
                ListKegiatan();
                lampiranEdit = [];
                InitEditDropzone();
            }
            else
                AlertMessage(data_obj.message);
        });
    });
}
function deleteKegiatan(id) {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            RequestData('DELETE', `/v1/ProgramItem/delete/${id}`, '#Kegiatan-table', null, null, function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Dihapus", null, "success");
                    ListKegiatan();
                    lampiran = [];
                    lampiranEdit = [];
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}
function InitAddDropzone() {
    let element = `<div id="AddKegiatan-Lampiran" class="dropzone" style="border-style: dashed;
                        border-color: #16181e;">
                    <div class="dz-default dz-message">
                    <img src="/Content/images/upload.png" class="text-center" width="10%">
                    <p class="text-center">Upload Here</p>
                    </div>
                </div>`;
    $('#AddKegiatan-Lampiran-Container').empty();
    $('#AddKegiatan-Lampiran-Container').append(element);
    InitDropzone('#AddKegiatan-Lampiran');

}
function InitEditDropzone() {
    let element = `<div id="EditKegiatan-Lampiran" class="dropzone" style="border-style: dashed;
                        border-color: #16181e;">
                    <div class="dz-default dz-message">
                    <img src="/Content/images/upload.png" class="text-center" width="10%">
                    <p class="text-center">Upload Here</p>
                    </div>
                </div>`;
    $('#EditKegiatan-Lampiran-Container').empty();
    $('#EditKegiatan-Lampiran-Container').append(element);
    InitDropzone('#EditKegiatan-Lampiran', true);

}
function InitDropzone(element, isEdit = false) {
    $(element).dropzone({
        addRemoveLinks: true,
        url: "/",
        success: async function (file, response) {
            let _base64;
            await FileToBase64(file)
                .then(dataBase64 => _base64 = dataBase64)
                .catch(error => {
                    AlertMessage(error);
                    return;
                });
            if (!isEdit) {
                lampiran.push({
                    filename: file.upload.filename,
                    base64: _base64
                });
            } else {
                lampiranEdit.push({
                    filename: file.upload.filename,
                    base64: _base64
                });
            }

            file.previewElement.classList.add("dz-success");
        },
        error: function (file, response) {
            file.previewElement.classList.add("dz-error");
        },
        removedfile: function (file) {
            file.previewElement.remove();
            if (file.isEdit){
                RemoveLampiranItemProgram(file.id);
            }
        }
    });
}
function RemoveLampiranItemProgram(id) {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            RequestData('DELETE', `/v1/ProgramMedia/delete/${id}`, '#md-Addlampiran', null, null, function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Dihapus", null,"success");
                    ListKegiatan();
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}
