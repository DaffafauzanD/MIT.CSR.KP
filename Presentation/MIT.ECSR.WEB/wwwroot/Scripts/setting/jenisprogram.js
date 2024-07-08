$(document).ready(function () {
    $('#JenisProgram-AddBtn').on('click', function () {
        $('.clear').val('');
        $('#md-JenisProgram-Add').modal('show');
    });
    listJenisProgram();
});

function listJenisProgram(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#JenisProgram-page_select').val();
    var element = {
        table: '#JenisProgram-table',
        tbody: '#JenisProgram-tbody',
        from: '#JenisProgram-from_page',
        to: '#JenisProgram-to_page',
        total: '#JenisProgram-total',
        pagination: '#JenisProgram-pagination',
        item_pagination: 'JenisProgram-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "name",
                search: $('#JenisProgram-Search').val()
            }

        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', "/v1/JenisProgram/list", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.succeeded) {
            console.log('data', data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.list.length > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'listJenisProgram'
                }, function (count) {
                    data.list.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-nowrap text-center">${count}</td>
								<td class="text-nowrap">${item.name}</td>
                                <td class="text-center">
                                    <div>
                                         <button type="button" data-toggle="dropdown" class="btn btn-sm btn-info box-shadow-2" aria-hashpopup="true" aria-expanded="false"><i class="ft-list"></i></button>
                                        <div class="dropdown-menu">
                                            <a href="#" class="dropdown-item" onclick="editJenisProgram(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                            <a href="#" class="dropdown-item" onclick="deleteJenisProgram('${item.id}');">Delete</a>
                                        </div>
                                   </div>
                                </td>
                            </tr>
                        `);
                        count++;
                    });
                });
            } else {
                SetTableData(false, 9, element);
            }
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}


function addJenisProgramSave() {
    ConfirmMessage('Apakah Anda Yakin?', function (isConfirm) {
        if (isConfirm) {
            var param = {
                Name: $('#Add-JenisProgram-Nama').val(),
                Active: $('#Add-JenisProgram-Status').is(":checked")
            }
            RequestData('POST', `/v1/JenisProgram/add`, '#md-JenisProgram-Add', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Disimpan",null, "success");
                    $('#md-JenisProgram-Add').modal('hide');
                    listJenisProgram();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.message);
                } else
                    ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
            });
        }
    });
}

function editJenisProgram(el) {
    var data = $(el).data('detail');
    console.log(data)
    $('.clear').val('');
    $('#md-JenisProgram-Edit').modal('show');
    $('#Edit-JenisProgram-Nama').val(data.name);

    $('#md-JenisProgram-Edit').data('id', data.id);
}
function editJenisProgramSave() {
    ConfirmMessage('Apakah Anda Yakin Akan Mengubah Data Ini?', isConfirm => {
        if (isConfirm) {
            var param = {
                Active: true,
                Name: $('#Edit-JenisProgram-Nama').val()
            }
            RequestData('PUT', `/v1/JenisProgram/edit/${$('#md-JenisProgram-Edit').data('id')}`, '#md-JenisProgram-Edit .modal-content', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Dirubah", null, "success");
                    $('#md-JenisProgram-Edit').modal('hide');
                    listJenisProgram();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.message);
                } else
                    ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
            });
        }
    });
}

function deleteJenisProgram(id) {
    ConfirmMessage('Apakah Anda Yakin Akan Menghapus Data Ini?', isConfirm => {
        if (isConfirm) {
            RequestData('DELETE', `/v1/JenisProgram/delete/${id}`, null, null, null, function (data) {
                if (data.succeeded) {
                    AlertMessage("Data Deleted Successfully ...",null, "success");
                    listJenisProgram();
                } else
                    ShowNotif(`${data.message} : ${data.description}`, "error");
            });
        }
    });
}
