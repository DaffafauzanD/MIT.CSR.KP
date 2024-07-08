
$(document).ready(function () {
    $("#Pengajuan-jenisProgramFilter").change(function () {
        getListPenawaranProgramExternalSearch();
    })
    $("#Penawaran-jenisProgramFilter, #Penawaran-StatusFilter").change(function () {
        getListPenawaranExternalFilter();
    })
    $("#History-jenisProgramFilter,#History-StatusFilter").change(function () {
        getListHistoryExternalFilter();
    })
    getListPenawaranProgramExternal();
    getListPenawaranExternal();
    getListHistoryExternal();
    getListPenawaranExternalFilter();
    getListHistoryExternalFilter();
    ListJenisProgram();
});
function SearchPengajuan() {
    console.log('search');
    getListPenawaranProgramExternal();
}
/*Table Pengajuan*/
function getListPenawaranProgramExternal(page) {
    console.log("test");
    page = page != undefined ? page : 1;
    var pageSize = $('#PenawaranProgramExternal-page_select').val();
    var element = {
        table: '#PenawaranProgramExternal-table',
        tbody: '#PenawaranProgramExternal-tbody',
        from: '#PenawaranProgramExternal-from_page',
        to: '#PenawaranProgramExternal-to_page',
        total: '#PenawaranProgramExternal-total',
        pagination: '#PenawaranProgramExternal-pagination',
        item_pagination: 'PenawaranProgramExternal-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "",
                search: $('#PengajuanProgram-Search').val()
            }

        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }
    RequestData('GET', "/v1/Program/list_external", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log("DATA eXTERNAL",data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 7, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListPenawaranProgramExternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.NamaProgram}</td>
								<td class="text-center">${item.JenisProgram.Nama}</td>
								<td class="text-center">${DateToStringFormat(item.TglPelaksanaan)}</td>
							    <td class="text-center">${item.Kecamatan + " - " + item.Kelurahan}</td>
							    <td class="text-center">${item.Deskripsi}</td>
                                <td class="text-center">
                                    <button class="dropbtn btn-success btn-sm" type="button" onclick="PengajuanExternalDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Ajukan</button>
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

/*Table Filter Pengajuan*/
function getListPenawaranProgramExternalSearch(page) {
    console.log("test search");
    page = page != undefined ? page : 1;
    var pageSize = $('#PenawaranProgramExternal-page_select').val();
    var element = {
        table: '#PenawaranProgramExternal-table',
        tbody: '#PenawaranProgramExternal-tbody',
        from: '#PenawaranProgramExternal-from_page',
        to: '#PenawaranProgramExternal-to_page',
        total: '#PenawaranProgramExternal-total',
        pagination: '#PenawaranProgramExternal-pagination',
        item_pagination: 'PenawaranProgramExternal-item'
    }
    var param = {
        id_jenis_program: $('#Pengajuan-jenisProgramFilter').val(),
        search: $('#PengajuanProgram-Search').val()
    }
    RequestData('GET', "/v1/Program/list_external", element.table, element.tbody, param, function (data) {
        console.log("data url",data);
        if (data.Succeeded) {
            console.log("searcheXTERNAL", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 6, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListPenawaranProgramExternalSearh'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.NamaProgram}</td>
								<td class="text-center">${item.JenisProgram.Nama}</td>
								<td class="text-center">${DateToStringFormat(item.TglPelaksanaan)}</td>
							    <td class="text-center">${item.Kecamatan + " - " + item.Kelurahan}</td>
							    <td class="text-center">${item.Deskripsi}</td>
                                <td class="text-center">
                                    <button class="dropbtn btn-success btn-sm" type="button" onclick="PengajuanExternalDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Ajukan</button>
                                </td>
                            </tr>
                        `);
                        count++;
                    });
                });
            } else {
                SetTableData(false, 6, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
/*Table Riwayat Penawaran*/
function getListPenawaranExternal(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#PenawaranExternal-page_select').val();
    var element = {
        table: '#PenawaranExternal-table',
        tbody: '#PenawaranExternal-tbody',
        from: '#PenawaranExternal-from_page',
        to: '#PenawaranExternal-to_page',
        total: '#PenawaranExternal-total',
        pagination: '#PenawaranExternal-pagination',
        item_pagination: 'PenawaranExternal-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "Status",
                search: $('#Penawaran-StatusFilter').val()
            }

        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }
    RequestData('GET', "/v1/Penawaran/list_external", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log("Riwaya Penawaran", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListPenawaranExternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                            $(element.tbody).append(`
                             <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Program.NamaProgram}</td>
								<td class="text-center">${item.Program.JenisProgram.Nama}</td>
								<td class="text-center">${DateToStringFormat(item.Program.TglPelaksanaan)}</td>
							    <td class="text-center">${item.Program.Kecamatan + " - " + item.Program.Kelurahan}</td>
							    <td class="text-center">${item.Program.Deskripsi}</td>
								<td class="text-center">${item.StatusDescription}</td>
                                <td class="text-center">
                                    <button class="dropbtn btn-success btn-sm" type="button" onclick="detailPengajuanDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</button>
                                </td>
                            </tr>
                            `);
                            count++;
                       
                       
                    });
                });
            } else {
                SetTableData(false, 8, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}

/*Table History*/
function getListHistoryExternal(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#HistoryExternal-page_select').val();
    var element = {
        table: '#HistoryExternal-table',
        tbody: '#HistoryExternal-tbody',
        from: '#HistoryExternal-from_page',
        to: '#HistoryExternal-to_page',
        total: '#HistoryExternal-total',
        pagination: '#HistoryExternal-pagination',
        item_pagination: 'HistoryExternal-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "Status",
                search: $('#History-StatusFilter').val()
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
            console.log("data history", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 10, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListHistoryExternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
								<td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
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
                SetTableData(false, 10, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
/*Table Filter Riwayat*/
function getListPenawaranExternalFilter(page) {

    page = page != undefined ? page : 1;
    var pageSize = $('#PenawaranExternal-page_select').val();
    var element = {
        table: '#PenawaranExternal-table',
        tbody: '#PenawaranExternal-tbody',
        from: '#PenawaranExternal-from_page',
        to: '#PenawaranExternal-to_page',
        total: '#PenawaranExternal-total',
        pagination: '#PenawaranExternal-pagination',
        item_pagination: 'PenawaranExternal-item'
    }
    var param = {
        id_jenis_program: $('#Penawaran-jenisProgramFilter').val(),
        search: $('#PenawaranProgram-Search').val(),
        status: $('#Penawaran-StatusFilter').val()
    }
    console.log("Param",param);
    RequestData('GET', "/v1/Penawaran/list_external", element.table, element.tbody, param, function (data) {
        if (data.Succeeded) {
            console.log("search berhasil", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListPenawaranExternalSearch'
                }, function (count) {
                    data.List.forEach(function (item) {
                        
                            $(element.tbody).append(`
                             <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Program.NamaProgram}</td>
								<td class="text-center">${item.Program.JenisProgram.Nama}</td>
								<td class="text-center">${DateToStringFormat(item.Program.TglPelaksanaan)}</td>
							    <td class="text-center">${item.Program.Kecamatan + " - " + item.Program.Kelurahan}</td>
							    <td class="text-center">${item.Program.Deskripsi}</td>
								<td class="text-center">${item.StatusDescription}</td>

                                <td class="text-center">
                                    <button class="dropbtn btn-success btn-sm" type="button" onclick="detailPengajuanDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</button>
                                </td>
                            </tr>
                            `);
                            count++;

                    });
                });
            } else {
                SetTableData(false, 8, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
/*Table Filter History*/
function getListHistoryExternalFilter(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#HistoryExternal-page_select').val();
    var element = {
        table: '#HistoryExternal-table',
        tbody: '#HistoryExternal-tbody',
        from: '#HistoryExternal-from_page',
        to: '#HistoryExternal-to_page',
        total: '#HistoryExternal-total',
        pagination: '#HistoryExternal-pagination',
        item_pagination: 'HistoryExternal-item'
    }
    var param = {
        id_jenis_program: $('#History-jenisProgramFilter').val(),
        search: $('#HistoryPenawaran-Search').val(),
        status: $('#History-StatusFilter').val()
    }
    RequestData('GET', "/v1/Penawaran/list_log", element.table, element.tbody, param, function (data) {
        if (data.Succeeded) {
            console.log("data history", data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 10, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListHistoryExternalFilter'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-center">${count}</td>
								<td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
								<td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
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
                SetTableData(false, 10, element);
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
            $('#Pengajuan-jenisProgramFilter,#History-jenisProgramFilter,#Penawaran-jenisProgramFilter').html("");
            $('#Pengajuan-jenisProgramFilter,#History-jenisProgramFilter,#Penawaran-jenisProgramFilter').append(`<option value=""> -- Pilih Jenis Program --</option>`);
            data.List.forEach(function (item) {
                $('#Pengajuan-jenisProgramFilter,#History-jenisProgramFilter,#Penawaran-jenisProgramFilter').append(`<option value="${item.Id}">${item.Name} </option>`);
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
