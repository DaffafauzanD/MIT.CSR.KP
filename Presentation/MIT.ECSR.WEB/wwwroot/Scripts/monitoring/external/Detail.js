$(document).ready(function () {
    DetailExternal();
    GetListLampiran();
    ListHistory();
});

function DetailExternal(page) {
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
                search: $('#DetailExternal-Id').val()
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
                SetTableData(true, 7, element, {
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
                                  </tr>
                            `);
                        count++;

                    });
                }
                )
            } else {
                SetTableData(false, 7, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }

    })
}

function GetListLampiran() {
    var id = $('#DetailExternal-Id').val();
    RequestData('GET', `/v1/ProgramMedia/list/${id}`, '#DetailExternal-Lampiran', null, null, function (data) {
        if (data.succeeded) {
            $('#DetailExternal-ListLampiran').html('');
            if (data.list.length > 0) {
                data.list.forEach(function (item) {
                    $('#DetailExternal-ListLampiran').append(`
                           <div class="col-md-12 my-1">
                               <div class="bs-callout-blue-grey callout-border-left callout-bordered callout-transparent">
                                   <div class="card-header">
                                       <div class="row">
                                           <div class="d-flex align-self-center col-md-6 justify-content-start">
                                               <a href="${item.url.original}" target="_blank">${item.fileName}</a>
                                           </div>
                                       
                                       </div>
                                   </div>
                               </div>
                           </div>
                       `);
                });
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }

    })
}

function ListHistory(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#RiwayatProgress-page_select').val();

    var params = {
        start: page,
        length: pageSize,
        idProgram: $('#DetailExternal-Id').val()
    };
    var element = {
        table: '#RiwayatProgress-table',
        tbody: '#RiwayatProgress-tbody',
        from: '#RiwayatProgress-from_page',
        to: '#RiwayatProgress-to_page',
        total: '#RiwayatProgress-total',
        pagination: '#RiwayatProgress-pagination',
        item_pagination: 'RiwayatProgress-item'
    }

    RequestData('GET', '/v1/ProgressProgram/list_history_external', '#RiwayatProgress-table', null, params, function (data) {
        if (data.succeeded) {
            $(element.tbody).empty();
            if (data.list.length > 0) {
                SetTableData(true, 10, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListHistory'
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
                        var status = '';
                        var can_submit = false;
                        switch (item.status) {
                            case 0:
                                status = 'DRAFT';
                                can_submit = true;
                                break;
                            case 1:
                                if (item.progress == 100) {
                                    status = 'WAITING';
                                }
                                else {
                                    status = 'SUBMIT';
                                }
                                break;
                            case 2:
                                status = 'APPROVE';
                                break;
                            case 3:
                                status = 'REJECT';
                                break;
                            default:
                        }
                        var elBtn = can_submit ? `<button type="button" class="btn btn-sm btn-success box-shadow-2" onclick="submit(this, true);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'">Submit</button>
                                                 <button type="button" class="btn btn-sm btn-danger box-shadow-2" onclick="submit(this, false);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'">Cancel</button>` : '-';
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.programItemName}</td>
                                        <td>${item.unit} (${item.booking}) ${item.satuan}</td>
                                        <td>${DateToStringFormat(item.tglProgress)}</td>
                                        <td>${item.progress}%</td>
                                        <td>${item.deskripsi ?? "-"}</td>
                                        <td>
                                            ${elLampiran}
                                        </td>
                                        <td>
                                            ${status}
                                        </td>
                                        <td>
                                            ${item.notes ?? "-"}
                                        </td>
                                        <td class="text-center">
                                            ${elBtn}
                                        </td>
                                  </tr>
                            `);
                        count++;

                    });
                }
                );
            } else {
                SetTableData(false, 10, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    })
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
            RequestData('POST', `/v1/progressprogram/submit`, '#RiwayatProgress-table', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Disimpan", null, "success");
                    ListHistory();
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}