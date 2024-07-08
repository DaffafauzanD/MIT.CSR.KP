$(document).ready(function () {
    SetDatePicker('#Penawaran-StartTglPelaksanaan', "DD MMMM, YYYY", null, null, false, null);
    SetDatePicker('#Penawaran-EndTglPelaksanaan', "DD MMMM, YYYY", null, null, false, null, function () {
        setTimeout(ListPenawaran,
            1000);
    });

    ListJenisProgram("#Penawaran-JenisProgram-Container", "#Penawaran-JenisProgram", null, true);
    ("#Penawaran-Kegiatan-Container", null, true);
    $('#Penawaran-JenisProgram').on('change', function () {
        var param = [{

            "field": "idjenisprogram",
            "search": `${$(this).val()}`
        }];
        var boolean = false;
        if ($(this).val() == "") {
            param = null;
            boolean = true;
        }

        ListKegiatanReferensi('#Penawaran-Kegiatan-Container', '#Penawaran-Kegiatan', null, boolean, param);
    });
    ListPenawaran();
});


function ListPenawaran(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#Penawaran-page_select').val();
    var element = {
        table: '#Penawaran-table',
        tbody: '#Penawaran-tbody',
        from: '#Penawaran-from_page',
        to: '#Penawaran-to_page',
        total: '#Penawaran-total',
        pagination: '#Penawaran-pagination',
        item_pagination: 'Penawaran-item'
    }

    var params = {
        search: $('#Penawaran-Search').val(),
        id_jenis_program: $('#Penawaran-JenisProgram').val(),
        start: page,
        length: pageSize,
        id_kegiatan: $('#Penawaran-Kegiatan').val(),
        tgl_pelaksanaan: $('#Penawaran-StartTglPelaksanaan').val() ? StringToDateFormat($('#Penawaran-StartTglPelaksanaan').val(), "DD MMMM, YYYY") + "|" + StringToDateFormat($('#Penawaran-EndTglPelaksanaan').val(), "DD MMMM, YYYY") : "",
    };
    RequestData('GET', '/v1/Penawaran/list_program', element.table, element.tbody, params, function (data) {
        if (data.succeeded) {
            if (data.count > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListPenawaran'
                }, function (count) {
                    data.list.forEach(function (item) {
                        let kegiatan = '-';
                        let available = '-';
                        if (item.items.length > 0) {
                            var _available = 0;
                            var _available_unit = 0;
                            var _kegiatan = item.items.length;
                            var _kegiatan_unit = 0;
                            item.items.forEach(function (i){
                                _kegiatan_unit += i.jumlah;
                                if (i.sisa > 0) {
                                    _available++;
                                    _available_unit += i.sisa;
                                }
                            });
                            kegiatan = `${_kegiatan} Kegiatan (${_kegiatan_unit})`;
                            available = `${_available} Kegiatan (${_available_unit})`;
                        }
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.jenisProgram.nama}</td>
                                        <td>${item.namaProgram.nama}</td>
                                        <td>${item.lokasiDati}</td>
                                        <td>
                                            <ul style="list-style-type: none;padding-left: 0;">
                                                <li>${DateToStringFormat(item.startTglPelaksanaan)}</li>
                                                <li>${DateToStringFormat(item.endTglPelaksanaan)}</li>
                                            </ul>
                                        </td>
                                        <td class="text-center">${kegiatan}</td>
                                        <td class="text-center">${available}</td>
                                         <td class="text-center">${item.penawaran}</td>
                                        <td class="text-center">
                                            <a href='${window.location.origin}/Penawaran/Detail/${item.id}' type="button" class="btn btn-sm btn-info box-shadow-2">Detail</a>
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
