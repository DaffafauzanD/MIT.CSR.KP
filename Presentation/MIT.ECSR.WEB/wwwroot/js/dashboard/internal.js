$(document).ready(function () {
    getListMonitoringInternal();
    getListFilterPerusahaan();
});

function getListMonitoringInternal(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#MonitoringInt-page_select').val();
    var element = {
        table: '#MonitoringInt-table',
        tbody: '#MonitoringInt-tbody',
        from: '#MonitoringInt-from_page',
        to: '#MonitoringInt-to_page',
        total: '#MonitoringInt-total',
        pagination: '#MonitoringInt-pagination',
        item_pagination: 'MonitoringInt-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            }
            //{
            //    field: "namaperusahaan",
            //    search: $('#MonitoringInt-filterPerusahaan').val()
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
            console.log('data', data);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'getListMonitoringInternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                                <tr>
                                    <td class="text-center">${count}</td>
                                    <td class="text-center">${item.Penawaran.Program.NamaProgram}</td>
                                    <td class="text-center">${item.Penawaran.Program.JenisProgram.Nama}</td>
                                    <td class="text-center">${item.Penawaran.Perusahaan.NamaPerusahaan}</td>
                                    <td class="text-center">${item.Penawaran.Program.TglPelaksanaan}</td>
                                    <td class="text-center">${item.Penawaran.Program.StatusDescription}</td>
                                    <td class="text-center">${item.TotalProgress}</td>
                                    <td class="text-center">
                                        <div class=dropdown-basic>
                                            <div class="dropdown">
                                                <div class="btn-group">
                                                    <button class="dropbtn btn-success btn-sm btn-round" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                    <div class="dropdown-content">
                                                        <a href="#" id="btnShow_modal-Detail" onClick="detailMonitoringDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>History Progres</a>
                                                        <a href="#" id="btnShow_modal-UpdateProgress" onClick="updateProgressDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, "")}'>Update Progres</a>
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
            $('#MonitoringInt-filterPerusahaan').html("");
            $('#MonitoringInt-filterPerusahaan').append(`<option value=""> -- Pilih Perusahaan --</option>`)
            data.List.forEach(function (item) {
                $('#MonitoringInt-filterPerusahaan').append(`<option value="${item.Id}">${item.NamaPerusahaan} </option>`);
            })
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}

//$('#btnShow_modal-detailMonitoring').on('click', function () {
//    $('#modalDetail-monitoring').modal('show');
//});

//function openComment() {
//    $('#tableComment').removeClass("col-sm-12");
//    $('#tableComment').addClass("col-sm-5");
//    $('#showCommentProgram').removeAttr('hidden', true);
//}

//function closeComment() {
//    $('#tableComment').removeClass("col-sm-5");
//    $('#tableComment').addClass("col-sm-12");
//    $('#showCommentProgram').removeAttr('hidden', false);
//}