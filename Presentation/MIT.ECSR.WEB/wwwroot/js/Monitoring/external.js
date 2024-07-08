﻿$(document).ready(function () {
    $('#MonitoringExternal-filterStatus').change(function () {
        filterTabelMonitoring();
    })
    $('#MonitoringExternal-filterIdJenisProgram').change(function () {
        filterTabelMonitoring();
    })
    getListMonitoringExternal();
    filterTabelMonitoring();
    ListJenisProgram();

});

//TAbel Monitoring
function getListMonitoringExternal(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#MonitoringExternal-page_select').val();
    var element = {
        table: '#MonitoringExternal-table',
        tbody: '#MonitoringExternal-tbody',
        from: '#MonitoringExternal-from_page',
        to: '#MonitoringExternal-to_page',
        total: '#MonitoringExternal-total',
        pagination: '#MonitoringExternal-pagination',
        item_pagination: 'MonitoringExternal-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }

    RequestData('GET', '/v1/Monitoring/list', element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log('data', data.Count);
            console.log(location.href);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListMonitoringExternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                                <tr>
                                    <td class="text-center">${count}</td>
                                    <td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
                                    <td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
                                    <td class="text-center">${DateToStringFormat(item.Penawaran.Program.TglPelaksanaan)}</td>
                                    <td class="text-center">${item.Penawaran.Program.StatusDescription}</td>
                                    <td class="text-center">${item.TotalProgress + "%"}</td>
                                    <td class="text-center">
                                        <div class=dropdown-basic>
                                            <div class="dropdown">
                                                <div class="btn-group">
                                                    <button class="dropbtn btn-success btn-sm btn-round text-center" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                    <div class="dropdown-content">
                                                        <button class="m-2" type="button" style="background:transparent; border:none; color:#2b2b2b; opacity:0.6;" id="btnShow_modal-Detail" onClick="detailMonitoringExternalDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Detail Program</button>
                                                        <button class="m-2" type="button" style="background:transparent; border:none; color:#2b2b2b; opacity:0.6;" onClick="HistoryProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>History Progress</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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

    });
}

//Filter & Search
function filterTabelMonitoring(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#MonitoringExternal-page_select').val();
    var element = {
        table: '#MonitoringExternal-table',
        tbody: '#MonitoringExternal-tbody',
        from: '#MonitoringExternal-from_page',
        to: '#MonitoringExternal-to_page',
        total: '#MonitoringExternal-total',
        pagination: '#MonitoringExternal-pagination',
        item_pagination: 'MonitoringExternal-item'
    }
    var param = {
        id_jenis_program: $('#MonitoringExternal-filterIdJenisProgram').val(),
        search: $('#monitoringProgram-Search').val(),
        status: $('#MonitoringExternal-filterStatus').val()
    }
    console.log("Param", param);
    RequestData('GET', '/v1/Monitoring/list', element.table, element.tbody, param, function (data) {
        if (data.Succeeded) {
            console.log('data', data.Count);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListFilterStatus'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                                <tr>
                                    <td class="text-center">${count}</td>
                                    <td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
                                    <td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
                                    <td class="text-center">${DateToStringFormat(item.Penawaran.Program.TglPelaksanaan)}</td>
                                    <td class="text-center">${item.Penawaran.Program.StatusDescription}</td>
                                    <td class="text-center">${item.TotalProgress + "%"}</td>
                                    <td class="text-center">
                                        <div class=dropdown-basic>
                                            <div class="dropdown">
                                                <div class="btn-group">
                                                    <button class="dropbtn btn-success btn-sm btn" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                    <div class="dropdown-content">
                                                        <button class="m-2" type="button" style="background:transparent; border:none; color:#2b2b2b; opacity:0.6;" id="btnShow_modal-Detail" onClick="detailMonitoringExternalDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Detail Program</button>
                                                        <button class="m-2" type="button" style="background:transparent; border:none; color:#2b2b2b; opacity:0.6;" onClick="HistoryProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>History Progress</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
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

    });
}

//Melihat History Progress
function HistoryProgressDialog(el) {
    var data = $(el).data('detail');
    console.log("DATA DETAIL :", data);
    window.open(`/HistoryProgress/External?id_program=${data.Penawaran.Program.Id}&nama_program=${data.Penawaran.Program.NamaProgram}`, "_self");
    detailHistoryProgressDialog(el);
}


//List JEnis PRogram
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
            $('#MonitoringExternal-filterIdJenisProgram').html("");
            $('#MonitoringExternal-filterIdJenisProgram').append(`<option value=""> -- Filter Jenis Program --</option>`);
            data.List.forEach(function (item) {
                $('#MonitoringExternal-filterIdJenisProgram').append(`<option value="${item.Id}">${item.Name} </option>`);
            })
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}