$(document).ready(function () {
    $("#HistoryInternal-jenisProgramFilter,#HistoryInternal-StatusFilter").change(function () {
        getListHistoryInternalFilter();
    })
    $("#PenawaranInternal-jenisProgramFilter").change(function () {
        getListPenawaranInternalFilter();
    })
    getListPenawaranInternal();
    getListHistoryInternal();
    ListJenisProgram();
});

/*Table Penawaran*/
function getListPenawaranInternal(page) {
    console.log("test");
    page = page != undefined ? page : 1;
    var pageSize = $('#PenawaranInternal-page_select').val();
    var element = {
        table: '#PenawaranInternal-table',
        tbody: '#PenawaranInternal-tbody',
        from: '#PenawaranInternal-from_page',
        to: '#PenawaranInternal-to_page',
        total: '#PenawaranInternal-total',
        pagination: '#PenawaranInternal-pagination',
        item_pagination: 'PenawaranInternal-item'
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
    RequestData('GET', "/v1/Penawaran/list", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log("data penawaran1",data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 7, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListPenawaranInternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Program.NamaProgram}</td>
								<td class="text-center">${item.Program.JenisProgram.Nama}</td>
								<td class="text-center">${DateToStringFormat(item.Program.TglPelaksanaan)}</td>								
                                <td class="text-center">${item.Perusahaan.NamaPerusahaan}</td>
                                <td class="text-center">
                                <button class="dropbtn btn-success btn-sm" type="button" onclick="detailPengajuanDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</button>
                                 </td>
                            </tr>
                        `);
                        count++;
                    });
                });
            } else {
                SetTableData(false, 7, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
/*Table History*/
function getListHistoryInternal(page) {
    console.log("test history");
    page = page != undefined ? page : 1;
    var pageSize = $('#HistoryInternal-page_select').val();
    var element = {
        table: '#HistoryInternal-table',
        tbody: '#HistoryInternal-tbody',
        from: '#HistoryInternal-from_page',
        to: '#HistoryInternal-to_page',
        total: '#HistoryInternal-total',
        pagination: '#HistoryInternal-pagination',
        item_pagination: 'HistoryInternal-item'
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
    RequestData('GET', "/v1/Penawaran/list_log", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log("data", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 7, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListHistoryInternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                             <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
								<td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
								<td class="text-center">${item.Penawaran.Perusahaan.BidangUsaha}</td>
                                <td class="text-center">${item.Penawaran.Perusahaan.NamaPerusahaan}</td>
								<td class="text-center">${DateToStringFormat(item.Penawaran.Program.TglPelaksanaan)}</td>							    
								<td class="text-center">${item.Name}</td>
								<td class="text-center">${DateToStringFormat(item.TglLogging)}</td>								
								<td class="text-center">${item.StatusDescription}</td>
                                <td class="text-center">${item.Comment}</td>
                                <td class="text-center">
                                <button class="dropbtn btn-success btn-sm" type="button" onclick="detailHistoryDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</button>
   
                                </td>
                            </tr>
                        `);
                        count++;
                    });
                });
            } else {
                SetTableData(false, 7, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}

/*Table Filter Penawaran*/
function getListPenawaranInternalFilter(page) {

    page = page != undefined ? page : 1;
    var pageSize = $('#PenawaranInternal-page_select').val();
    var element = {
        table: '#PenawaranInternal-table',
        tbody: '#PenawaranInternal-tbody',
        from: '#PenawaranInternal-from_page',
        to: '#PenawaranInternal-to_page',
        total: '#PenawaranInternal-total',
        pagination: '#PenawaranInternal-pagination',
        item_pagination: 'PenawaranInternal-item'
    }
    var param = {
        id_jenis_program: $('#PenawaranInternal-jenisProgramFilter').val(),
        search: $('#PenawaranInternal-Search').val()
    }

    RequestData('GET', "/v1/Penawaran/list", element.table, element.tbody, param, function (data) {
        if (data.Succeeded) {
            console.log("data p1", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 7, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: ' getListPenawaranInternalFilter('
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Program.NamaProgram}</td>
								<td class="text-center">${item.Program.JenisProgram.Nama}</td>
								<td class="text-center">${DateToStringFormat(item.Program.TglPelaksanaan)}</td>								
                                <td class="text-center">${item.Perusahaan.NamaPerusahaan}</td>
                                <td class="text-center">
                                <button class="dropbtn btn-success btn-sm" type="button" onclick="detailHistoryDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</button>
                                    
                                </td>
                            </tr>
                        `);
                        count++;
                    });
                });
            } else {
                SetTableData(false, 7, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
/*Table History Internal Filter*/
function getListHistoryInternalFilter(page) {
    console.log("test history");
    page = page != undefined ? page : 1;
    var pageSize = $('#HistoryInternal-page_select').val();
    var element = {
        table: '#HistoryInternal-table',
        tbody: '#HistoryInternal-tbody',
        from: '#HistoryInternal-from_page',
        to: '#HistoryInternal-to_page',
        total: '#HistoryInternal-total',
        pagination: '#HistoryInternal-pagination',
        item_pagination: 'HistoryInternal-item'
    }
    var param = {
        id_jenis_program: $('#HistoryInternal-jenisProgramFilter').val(),
        search: $('#HistoryInternal-Search').val(),
        status: $('#HistoryInternal-StatusFilter').val(),
    }
    RequestData('GET', "/v1/Penawaran/list_log", element.table, element.tbody, param, function (data) {
        if (data.Succeeded) {
            console.log("data", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 7, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListHistoryInternalFilter'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                             <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
								<td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
								<td class="text-center">${item.Penawaran.Perusahaan.BidangUsaha}</td>
                                <td class="text-center">${item.Penawaran.Perusahaan.NamaPerusahaan}</td>
								<td class="text-center">${DateToStringFormat(item.Penawaran.Program.TglPelaksanaan)}</td>							    
								<td class="text-center">${item.Name}</td>
								<td class="text-center">${DateToStringFormat(item.TglLogging)}</td>								
								<td class="text-center">${item.StatusDescription}</td>
                                <td class="text-center">${item.Comment}</td>
                                <td class="text-center">
                                    <div class=dropdown-basic>
                                       <div class="dropdown">
                                          <div class="btn-group btn-group-sm">
                                              <button class="dropbtn btn-success btn-sm" type="button">Aksi</button>
                                              <div class="dropdown-content">
                                              <a class="text-start" href="#" onclick="detailHistoryDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                              </div>
                                          </div>
                                       </div>
                                   </div>
                                </td>
                            </tr>
                        `);
                        count++;
                    });
                });
            } else {
                SetTableData(false, 7, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
function ListJenisProgram() {
    var param = {
        filter: [
            {
                field: "",
                search: ""
            }
        ],
        sort: {
            field: "id",
            type: 0
        },
        start: 1,
        length: 0
    }
    RequestData('POST', "/v1/JenisProgram/list", '', null, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $('#PenawaranInternal-jenisProgramFilter,#HistoryInternal-jenisProgramFilter').html("");
            $('#PenawaranInternal-jenisProgramFilter,#HistoryInternal-jenisProgramFilter').append(`<option value=""> -- Pilih Jenis Program --</option>`);
            data.List.forEach(function (item) {
                $('#PenawaranInternal-jenisProgramFilter,#HistoryInternal-jenisProgramFilter').append(`<option value="${item.Id}">${item.Name} </option>`);
            })
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}
function detailPengajuanDialog(el) {
    var dataDetail = $(el).data('detail');
    openMenu(`/Penawaran/Detail?id=${dataDetail.Id}`);
}