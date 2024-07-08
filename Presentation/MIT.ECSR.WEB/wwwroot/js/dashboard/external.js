$(document).ready(function () {
    $('#MonitoringExternal-filterPerusahaan').change(function () {
        getListFilterStatus();
    })
    getListMonitoringExternal();
    getListFilterPerusahaan();
    getListFilterStatus();
});

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
            }
            //{
            //    field: "namaperusahaan",
            //    search: $('#MonitoringExternal-filterPerusahaan').val()
            //}
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
                                                    <button class="dropbtn btn-success btn-sm btn-round" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                    <div class="dropdown-content">
                                                        <a href="#" id="btnShow_modal-Detail" onClick="detailMonitoringExternalDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Detail Progress</a>
                                                        <a href="#" id="btnShow_modal-UpdateProgress" onClick="updateProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Update Progress</a>
                                                        <a href="#" id="btnShow_modal-UpdateProgress" onClick="updateProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Close Progress</a>
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

//Filter
function getListFilterStatus(page) {
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
        status: $('#MonitoringExternal-filterPerusahaan').val(),
        search: $('#monitoringProgram-Search').val()
    }

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
                                    <td class="text-center">${item.Penawaran.Program.TglPelaksanaan}</td>
                                    <td class="text-center">${item.Penawaran.Program.StatusDescription}</td>
                                    <td class="text-center">${item.TotalProgress + "%"}</td>
                                    <td class="text-center">
                                        <div class=dropdown-basic>
                                            <div class="dropdown">
                                                <div class="btn-group">
                                                    <button class="dropbtn btn-success btn-sm btn-round" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                    <div class="dropdown-content">
                                                        <a href="#" id="btnShow_modal-Detail" onClick="detailMonitoringExternalDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Detail Progress</a>
                                                        <a href="#" id="btnShow_modal-UpdateProgress" onClick="updateProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Update Progress</a>
                                                        <a href="#" id="btnShow_modal-UpdateProgress" onClick="updateProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Close Progress</a>
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

function getListFilterPerusahaan() {
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
    RequestData('POST', "/v1/Perusahaan/list", '', null, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
