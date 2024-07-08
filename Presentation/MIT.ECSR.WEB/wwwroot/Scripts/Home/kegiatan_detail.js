Dropzone.autoDiscover = false;
var lampiran = [];
var lampiranEdit = [];
$(document).ready(function () {
    ListKegiatan();
    ListHistory();
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
    RequestData('GET', `/v1/Penawaran/list_internal/${$('#DetailProgram-Id').val()}`, element.table, element.tbody, params, function (data) {
        if (data.succeeded) {
            console.log(data);
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
                                        <td class="text-right">${item.item.jumlah} ${item.item.satuanUnit}</td>
                                        <td class="text-right">${item.item.sisaJumlah} ${item.item.satuanUnit}</td>
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

function ListHistory(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#RiwayatProgress-page_select').val();

    var params = {
        start: page,
        length: pageSize,
        idProgram: $('#DetailProgram-Id').val()
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

    RequestData('GET', '/v1/ProgressProgram/list_history_public_external', '#RiwayatProgress-table', null, params, function (data) {
        if (data.succeeded) {
            $(element.tbody).empty();
            if (data.list.length > 0) {
                SetTableData(true, 7, element, {
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
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${DateToStringFormat(item.tglProgress)}</td>
                                        <td>${item.perusahaan}</td>
                                        <td>${item.programItemName}</td>
                                        <td>${item.unit} ${item.satuan}</td>
                                        <td>${item.progress}%</td>
                                        <td>
                                            ${elLampiran}
                                        </td>
                                  </tr>
                                 
                            `);
                        count++;

                    });
                }
                );
            } else {
                SetTableData(false, 7, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    })
}