$(document).ready(function () {
    $('#ProgramInternal-jenisProgramFilter, #ProgramInternal-statusProgram').change(function () {
        getListFilterProgram();
    });

    getListFilterProgram();
    getListProgramInternal();
    ListJenisProgram();
    //getListProgramExternal();
});

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
            $('#Add-Program-JenisProgram, #Edit-Program-JenisProgram, #ProgramInternal-jenisProgramFilter').html("");
            $('#Add-Program-JenisProgram, #Edit-Program-JenisProgram, #ProgramInternal-jenisProgramFilter').append(`<option value=""> -- Pilih Jenis Program --</option>`);
            data.List.forEach(function (item) {
                if (item.Active == true) {
                    $('#Add-Program-JenisProgram, #Edit-Program-JenisProgram, #ProgramInternal-jenisProgramFilter').append(`<option value="${item.Id}">${item.Name} </option>`);
            }
            });
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}

function ListDatiProgram() {
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
    RequestData('POST', "/v1/Dati/list", '', null, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $('#Add-Program-Tempat, #Edit-Program-Tempat').html("");
            $('#Add-Program-Tempat, #Edit-Program-Tempat').append(`<option value=""> -- Pilih Tempat --</option>`)
            data.List.forEach(function (item) {
                $('#Add-Program-Tempat, #Edit-Program-Tempat').append(`<option value="${item.Id}">${item.NamaDati4} , ${item.NamaDati3}  </option>`);
            })
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}

function getListSatuanJenis() {
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
    RequestData('POST', "/v1/SatuanJenis/list", '', null, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $('.Add-Program-idSatuanJenis').html("");
            $('.Add-Program-idSatuanJenis').append(`<option value=""> -- Pilih --</option>`)
            data.List.forEach(function (item) {
                if (item.Active == true) {
                    $('.Add-Program-idSatuanJenis').append(`<option value="${item.Id}">${item.Kode} </option>`);
                }
            });
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}

function getListProgramInternal(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#ProgramInternal-page_select').val();
    var element = {
        table: '#ProgramInternal-table',
        tbody: '#ProgramInternal-tbody',
        from: '#ProgramInternal-from_page',
        to: '#ProgramInternal-to_page',
        total: '#ProgramInternal-total',
        pagination: '#ProgramInternal-pagination',
        item_pagination: 'ProgramInternal-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "jenisprogram",
                search: $('#ProgramInternal-jenisProgramFilter').val()
            },
            {
                field: "namaprogram",
                search: $('#ProgramInternal-searchProgram').val()
            }
        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }

    RequestData('GET', '/v1/Program/list', element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            //$('#ProgramInternal-statusProgram').html("");
            //$('#ProgramInternal-statusProgram').append(`<option value=""> -- Pilih Status -- </option>`)
            //data.List.forEach(function (item) {
            //    if (item.Status == 1 || item.Status == 2) {
            //        $('#ProgramInternal-statusProgram').append(`<option value="${item.Status}">${item.StatusDescription} </option>`);
            //    }
            //});
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListProgramInternal'
                }, function (count) {
                    data.List.forEach(function (item) {
                        if (item.Status == 1) {
                            $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center text-nowrap">${count}</td>
                                        <td class="text-nowrap">${item.NamaProgram}</td>
                                        <td class="text-nowrap">${item.JenisProgram.Nama}</td>
                                        <td class="text-nowrap">${DateToStringFormat(item.TglPelaksanaan)}</td>
                                        <td class="text-nowrap">${item.Kelurahan + ', ' + item.Kecamatan}</td>
                                        <td class="text-nowrap">${item.Deskripsi}</td>
                                        <td class="text-nowrap">${item.StatusDescription}</td>
                                        <td class="text-nowrap">
                                            <div class=dropdown-basic>
                                                <div class="dropdown">
                                                    <div class="btn-group">
                                                        <button class="dropbtn btn-success btn-sm" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                        <div class="dropdown-content">
                                                            <a href="#" onclick="detailProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                                            <a href="#!" onclick="editDraftProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                                            <a href="#!" onclick="deleteDraftProgramDialog(this);" data-detail='${JSON.stringify(item)}'>Delete</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                  </tr>
                            `);
                            count++;
                        } else {
                            $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center text-nowrap">${count}</td>
                                        <td class="text-nowrap">${item.NamaProgram}</td>
                                        <td class="text-nowrap">${item.JenisProgram.Nama}</td>
                                        <td class="text-nowrap">${DateToStringFormat(item.TglPelaksanaan)}</td>
                                        <td class="text-nowrap">${item.Kelurahan + ',' + item.Kecamatan}</td>
                                        <td class="text-nowrap">${item.Deskripsi}</td>
                                        <td class="text-nowrap">${item.StatusDescription}</td>
                                        <td class="text-nowrap">
                                            <div class=dropdown-basic>
                                                <div class="dropdown">
                                                    <div class="btn-group">
                                                        <button class="dropbtn btn-success btn-sm" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                        <div class="dropdown-content">
                                                            <a href="#" onclick="detailProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                  </tr>
                            `);
                            count++;
                        }
                        

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

function getListFilterProgram(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#ProgramInternal-page_select').val();
    var element = {
        table: '#ProgramInternal-table',
        tbody: '#ProgramInternal-tbody',
        from: '#ProgramInternal-from_page',
        to: '#ProgramInternal-to_page',
        total: '#ProgramInternal-total',
        pagination: '#ProgramInternal-pagination',
        item_pagination: 'ProgramInternal-item'
    }
    var param = {
        id_jenis_program: $('#ProgramInternal-jenisProgramFilter').val(),
        search: $('#ProgramInternal-searchProgram').val(),
        status: $('#ProgramInternal-statusProgram').val()
    }

    RequestData('GET', '/v1/Program/list', element.table, element.tbody, param, function (data) {
        if (data.Succeeded) {
            //console.log('data', data.Count);
            if (data.List.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListFilterProgram'
                }, function (count) {
                    data.List.forEach(function (item) {
                        if (item.Status == 1) {
                            $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center text-nowrap">${count}</td>
                                        <td class="text-nowrap">${item.NamaProgram}</td>
                                        <td class="text-nowrap">${item.JenisProgram.Nama}</td>
                                        <td class="text-nowrap">${DateToStringFormat(item.TglPelaksanaan)}</td>
                                        <td class="text-nowrap">${item.Kelurahan + ', ' + item.Kecamatan}</td>
                                        <td class="text-nowrap">${item.Deskripsi}</td>
                                        <td class="text-nowrap">${item.StatusDescription}</td>
                                        <td class="text-nowrap">
                                            <div class=dropdown-basic>
                                                <div class="dropdown">
                                                    <div class="btn-group">
                                                        <button class="dropbtn btn-success btn-sm" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                        <div class="dropdown-content">
                                                            <a href="#" onclick="detailProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                                            <a href="#!" onclick="editDraftProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                                            <a href="#!" onclick="deleteDraftProgramDialog(this);" data-detail='${JSON.stringify(item)}'>Delete</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                  </tr>
                            `);
                            count++;
                        } else {
                            $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center text-nowrap">${count}</td>
                                        <td class="text-nowrap">${item.NamaProgram}</td>
                                        <td class="text-nowrap">${item.JenisProgram.Nama}</td>
                                        <td class="text-nowrap">${DateToStringFormat(item.TglPelaksanaan)}</td>
                                        <td class="text-nowrap">${item.Kelurahan + ',' + item.Kecamatan}</td>
                                        <td class="text-nowrap">${item.Deskripsi}</td>
                                        <td class="text-nowrap">${item.StatusDescription}</td>
                                        <td class="text-nowrap">
                                            <div class=dropdown-basic>
                                                <div class="dropdown">
                                                    <div class="btn-group">
                                                        <button class="dropbtn btn-success btn-sm" type="button">Aksi <span><i class="icofont icofont-arrow-down"></i></span></button>
                                                        <div class="dropdown-content">
                                                            <a href="#" onclick="detailProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                  </tr>
                            `);
                            count++;
                        }

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