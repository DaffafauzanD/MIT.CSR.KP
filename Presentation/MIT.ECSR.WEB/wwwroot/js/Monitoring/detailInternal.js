
function detailMonitoringInternalDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#modalDetailInternal-monitoring').modal('show');
    console.log("data", data.Penawaran.Program.Id);
    $('#Id_Program').val(data.Penawaran.Program.Id);
    $('#Detail-Monitoring-NamaProgram').val(data.Penawaran.Program.NamaProgram);
    $('#Detail-Monitoring-JenisProgram').val(data.Penawaran.Program.JenisProgram.Nama);
    $('#Detail-Monitoring-WaktuPelaksanaan').val(DateToStringFormat(data.Penawaran.Program.TglPelaksanaan));
    $('#Detail-Monitoring-Tempat').val(data.Penawaran.Program.Kecamatan + "," + data.Penawaran.Program.Kelurahan);
    $('#Detail-Monitoring-Perusahaan').val(data.Penawaran.Perusahaan.NamaPerusahaan);
    $('#Detail-Monitoring-Deskripsi').val(data.Penawaran.Program.Deskripsi);

    getListDetailInternalProgress();
    getListDetailKebutuhanProgram();

}

//Tabel Item
function getListDetailKebutuhanProgram(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#DetailKebutuhanProgram-page_select').val();
    var element = {
        table: '#DetailKebutuhanProgram-table',
        tbody: '#DetailKebutuhanProgram-tbody',
        from: '#DetailKebutuhanProgram-from_page',
        to: '#DetailKebutuhanProgram-to_page',
        total: '#DetailKebutuhanProgram-total',
        pagination: '#DetailKebutuhanProgram-pagination',
        item_pagination: 'DetailKebutuhanProgram-item'
    }
    var param = {
        search: "",
        id_program: $('#Id_Program').val(),
        start_date: "2022-10-19",
        end_date: "2022-10-19",
        start: page,
        length: pageSize
    }

    RequestData('GET', `/v1/Monitoring/list_item/${$('#Id_Program').val()}`, element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log('data', data.Count);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListDetailKebutuhanProgram'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                                <tr>
                                    <td class="text-center">${item.Item.Nama}</td>
                                    <td class="text-center">${item.Item.Jumlah}</td>
                                    <td class="text-center">${item.Item.SatuanJenis.Nama}</td>
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

//Tabel Detail
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
        id_program: $('#Id_Program').val(),
        start_date: "2022-10-19",
        end_date: "2022-10-19",
        start: page,
        length: pageSize,
        sort: {
            field: "id",
            type: 0
        }
    }

    RequestData('GET', `/v1/Monitoring/list_progress/${$('#Id_Program').val()}`, element.table, element.tbody, JSON.stringify(param), function (data) {
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
                                    <td class="text-center">${item.Item.Jumlah}</td>
                                    <td class="text-center">${item.Item.SatuanJenis.Nama}</td>
                                    <td class="text-center">${item.Progress}</td>
                                    <td class="text-center">${item.Deskripsi}</td>
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
