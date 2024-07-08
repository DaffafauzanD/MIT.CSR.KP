
function detailMonitoringDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#modalDetail-monitoring').modal('show');
    console.log("data",data.Penawaran.Program.Items);
    $('#Detail-Monitoring-NamaProgram').val(data.Penawaran.Program.NamaProgram);
    $('#Detail-Monitoring-JenisProgram').val(data.Penawaran.Program.JenisProgram.Nama);
    $('#Detail-Monitoring-WaktuPelaksanaan').val(DateToStringFormat(data.Penawaran.Program.TglPelaksanaan));
    $('#Detail-Monitoring-Tempat').val(data.Penawaran.Program.Kecamatan + "," + data.Penawaran.Program.Kelurahan);
    $('#Detail-Monitoring-Deskripsi').val(data.Penawaran.Program.Deskripsi);

    $('#Detail-Monitoring-NamaItem').val(data.Penawaran.Program.Items.Nama);
    $('#Detail-Monitoring-Qty').val(data.Penawaran.Program.Items.SatuanJenis.Nama);
    $('#Detail-Monitoring-Satuan').val(data.Penawaran.Program.Items.Jumlah);

    getListHistoryProgress();
    

   
}
function getListHistoryProgress(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#HistoryProgress-page_select').val();
    var element = {
        table: '#HistoryProgress-table',
        tbody: '#HistoryProgress-tbody',
        from: '#HistoryProgress-from_page',
        to: '#HistoryProgress-to_page',
        total: '#HistoryProgress-total',
        pagination: '#HistoryProgress-pagination',
        item_pagination: 'HistoryProgress-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            }
        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }

    RequestData('GET', `/v1/Monitoring/list_progress/${$('#modalDetail-monitoring').data('Penawaran.Program.Id')}`, '#modalDetail-monitoring .modal-content', JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log('data', data);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'getListHistoryProgress'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                                <tr>
                                    <td class="text-center">${count}</td>
                                    <td class="text-center">${item.Item.Nama}</td>

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

    });
}
