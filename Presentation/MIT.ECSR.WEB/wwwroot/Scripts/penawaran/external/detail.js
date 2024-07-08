$(document).ready(function () {
    ListPenawaran();
});


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
            console.log(data);
            if (data.count > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListPenawaran'
                }, function (count) {
                    data.list.forEach(function (item) {
                        let status = '';
                        var can_submit = false;
                        switch (item.status) {
                            case 1:
                                can_submit = true;
                                status = `<div class="badge badge-info"><span>Draft</span></div>`;
                                break;
                            case 2:
                                status = `<div class="badge badge-success"><span>Submit</span></div>`;
                                break;
                            case 3:
                                status = `<div class="badge badge-danger"><span>Closed</span></div>`;
                                break;
                        }
                        var btnAction = can_submit ? `<button type="button" class="btn btn-sm btn-success box-shadow-2" onclick="submit(this, true);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'">Submit</button>
                                        <button type="button" class="btn btn-sm btn-danger box-shadow-2" onclick="submit(this, false);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'">Cancel</button>` : '-';
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.programItemName}</td>
                                        <td>${item.penawaran.jumlah} ${item.satuanUnit}</td>
                                        <td>${DateToStringFormat(item.createDate)}</td>
                                        <td>${status}</td>
                                        <td>
                                            ${btnAction}
                                        </td>
                                  </tr>
                            `);
                        count++;

                    });
                }
                )
            } else {
                SetTableData(false, 9, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }

    })
}

function PengajuanProgram() {
    if (!FormValidate('Penawaran')) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (!isConfirm) {
            return;
        }
        var items = [];
        var index = 0;
        $("input[name='pengajuan_item[]']").each(function () {
            var anggaran = $("input[name='anggaran_item[]']")[index];
            items.push({
                idProgramItem: $(this).attr('id'),
                value: $(this).val(),
                anggaran: $(anggaran).val()
            });
            index++;
        });
        var param = {
            items: items
        }

        RequestData('POST', `/v1/Penawaran/submit`, '#Penawaran', null, JSON.stringify(param), function (data_obj) {
            if (data_obj.succeeded) {
                AlertMessage("Data Berhasil Disimpan",null, "success");
                location.reload();
            }
            else
                AlertMessage(data_obj.message);
        });
    });
}

function submit(el, value) {
    var data = $(el).data('detail');
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var param = {
                id: data.id,
                isApprove: value,
                notes: ""
            }
            RequestData('POST', `/v1/penawaran/approve`, '#md-Penawaran_approval', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    //AlertMessage("Data Berhasil Disimpan", null, "success");
                    //ListPenawaran();
                    location.reload();
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}