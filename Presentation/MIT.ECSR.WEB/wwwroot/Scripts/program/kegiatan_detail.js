Dropzone.autoDiscover = false;
var lampiran = [];
var lampiranEdit = [];
$(document).ready(function () {
    ListKegiatan();
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
                search: $('#DetailProgram-Id').val()
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
