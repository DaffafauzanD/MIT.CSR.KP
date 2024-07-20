$(document).ready(function () {
    ListKegiatan();
    ListPenawaran();
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

    var params = {
        start: page,
        length: pageSize
    };
    RequestData('GET', `/v1/Penawaran/list_internal/${id_program}`, element.table, element.tbody, params, function (data) {
        if (data.succeeded) {
            console.log("data kegiatan",data);
            if (data.count > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListKegiatan'
                }, function (count) {
                    data.list.forEach(function (item) {
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.item.nama}</td>
                                      <td class="text-right">Rp. ${formatNumber(item.item.rupiah)}</td>    
                                        <td class="text-right">${item.item.jumlah}</td>
                                        <td class="text-right">${item.item.satuanUnit}</td>
                                        <td class="text-right">${item.item.sisaJumlah}</td>
                                  </tr>
                            `);
                        item.penawaran.forEach(function (i) {
                            var img = '';
                            if (i.photos != null)
                                img = `<img src="${i.photos.resize}" class="avatar avatar-online">`;

                            var approve_action = `<a href="#!" class="dropdown-item text-info" onclick="approveKegiatan('${i.id}')"><i class="ft-book"></i> Approve</a>`
                            $(element.tbody).append(`<tr>
                                        <td class="text-center">${img}</td>
                                        <td><span class="ml-2">${i.perusahaan}</span></td>
                                        <td class="text-right">Rp. ${formatNumber(i.rupiah)}</td>
                                        <td class="text-right">${i.jumlah} ${item.item.satuanUnit}</td>
                                        <td class="text-right">${item.item.sisaJumlah} ${item.item.satuanUnit}</td>
                                  </tr>`);
                        });
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

function ListPenawaran(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#RiwayatPenawaran-page_select').val();
    var element = {
        table: '#RiwayatPenawaran-table',
        tbody: '#RiwayatPenawaran-tbody',
        from: '#RiwayatPenawaran-from_page',
        to: '#RiwayatPenawaran-to_page',
        total: '#RiwayatPenawaran-total',
        pagination: '#RiwayatPenawaran-pagination',
        item_pagination: 'RiwayatPenawaran-item'
    }
    var params = {
        filter: [
            {
                field: "status",
                search: $('#RiwayatPenawaran-Status').val()
            },
            {
                field: "programitemname",
                search: $('#RiwayatPenawaran-Search').val()
            },
            {
                field: "idprogram",
                search: id_program
            }
        ],
        sort: {
            field: "createdate",
            type: 1
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', '/v1/Penawaran/list_penawaran', element.table, element.tbody, JSON.stringify(params), function (data) {
        if (data.succeeded) {
            console.log("data riwayat pengajuan", data)
            if (data.count > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListPenawaran'
                }, function (count) {
                    data.list.forEach(function (item) {
                        let status = '';
                        switch (item.status) {
                            case 1:
                                status = `<div class="badge badge-info"><span>DRAFT</span></div>`;
                                break;
                            case 2:
                                status = `<div class="badge badge-success"><span>SUBMIT</span></div>`;
                                break;
                            case 3:
                                status = `<div class="badge badge-danger"><span>CLOSED</span></div>`;
                                break;
                        }
                        var approve_by = '-';
                        if (item.approvedBy != null)
                            approve_by = item.approvedBy;
                        var approve_at = '-';
                        if (item.approvedAt != null)
                            approve_at = DateToStringFormat(item.approvedAt);
                        var notes = '-';
                        if (item.notes != null)
                            notes = item.notes;
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.perusahaan}</td>
                                        <td>${item.programItemName}</td>
                                        <td class="text-right">${item.penawaran.jumlah}</td>
                                        <td class="text-right">${item.satuanUnit}</td>
                                        <td class="text-right">${DateToStringFormat(item.createDate)}</td>
                                        <td>${status}</td>
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


function showApproval(el) {
    var data = $(el).data('detail');
    console.log(data)
    $('#md-Penawaran_approval').modal('show');
    $('#Penawaran_approval-Notes').val('');

    $('#Penawaran_approval-perusahaan').val(data.perusahaan);
    $('#Penawaran_approval-jumlah').val(data.jumlah);
    $('#Penawaran_approval-deskripsi').val(data.deskripsi);
    $('#Penawaran_approval-lampiran').html(`<label class="font-weight-bold">Lampiran</label>`);

    if (data.lampiran != null) {
        data.lampiran.forEach(function (item) {
            $('#Penawaran_approval-lampiran').append(`<a href="${item.original}" target="_blank" class="display-block">${item.filename}</a>`);
        });
    }
    $('#Penawaran_approval-save').unbind();
    $('#Penawaran_approval-save').on('click', function () {
        approvePenawaran(data.id, $('#Progress_Approval-Status').val(), $('#Penawaran_approval-Notes').val());
    });
}
function approvePenawaran(id, status, notes) {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var param = {
                id: id,
                isApprove: status == "true" ? true : false,
                notes: notes
            }
            RequestData('POST', `/v1/penawaran/approve`, '#md-Penawaran_approval', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Disimpan", null, "success");
                    $('#md-Penawaran_approval').modal('hide');
                    ListKegiatan();
                    ListPenawaran();
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}

function StartProgram() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            RequestData('POST', `/v1/program/start/${id_program}`, '#Section-DetailPenawaran', null, null, function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Disimpan", null, "success");
                    window.location.reload;
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}