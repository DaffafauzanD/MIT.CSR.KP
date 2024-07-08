$(document).ready(function () {
    getListUser();
    getListRoleUser();
    //RequestComboBox("Role", "Role", "#id-role", "#id-role", null, null, false, true); //Role

    //$("#id-role").on("change", function () {
    //    getListUser();
    //});


});

function getListUser(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#User-page_select').val();
    var element = {
        table: '#User-table',
        tbody: '#User-tbody',
        from: '#User-from_page',
        to: '#User-to_page',
        total: '#User-total',
        pagination: '#User-pagination',
        item_pagination: 'User-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "role",
                search: $('#User-RoleFilter').val()
            },
            {
                field: "status",
                search: "Active"
            }
        ],
        sort: {
            field: "id",
            type: 0
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', "/v1/User/list", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListUser'
                }, function (count) {
                    data.List.forEach(function (item) {
                        if (item.Status == 'Active') {
                            $(element.tbody).append(`
                                <tr>
                                    <td class="text-nowrap text-center">${count}</td>
								    <td class="text-nowrap">${item.Fullname}</td>
								    <td class="text-nowrap">${item.Username}</td>
								    <td class="text-nowrap">${item.Mail}</td>
								    <td class="text-nowrap">${item.Role.Nama}</td>
								    <td class="text-nowrap"><span class="badge bg-success">${item.Status}</span></td>

                                    <td class="">
                                        <div class=dropdown-basic>
                                           <div class="dropdown">
                                              <div class="btn-group">
                                                  <button class="dropbtn btn-success btn-sm" type="button">Action</button>
                                                  <div class="dropdown-content">
                                                      <a href="#" onclick="detailUserDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                                      <a href="#!" onclick="openEditUser(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                                      <a href="#!" onclick="deactivateUserDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Deactivate</a>
                                                  </div>
                                              </div>
                                           </div>
                                        </div>
                                    </td>
                                </tr>
                            `);
                            count++;
                        } else if (item.Status == 'Not Active') {
                            $(element.tbody).append(`
                                <tr>
                                    <td class="text-nowrap text-center">${count}</td>
								    <td class="text-nowrap">${item.Fullname}</td>
								    <td class="text-nowrap">${item.Username}</td>
								    <td class="text-nowrap">${item.Mail}</td>
								    <td class="text-nowrap">${item.Role.Nama}</td>								
								    <td class="text-nowrap"><span class="badge bg-danger">${item.Status}</span></td>

                                    <td class="">
                                        <div class=dropdown-basic>
                                           <div class="dropdown">
                                              <div class="btn-group">
                                                  <button class="dropbtn btn-success btn-sm" type="button">Action</button>
                                                  <div class="dropdown-content">
                                                      <a href="#" onclick="detailUserDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>
                                                      <a href="#!" onclick="openEditUser(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                                      <a href="#!" onclick="activateUserDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Activate</a>
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
                });
            } else {
                SetTableData(false, 9, element);
            }
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}

function getListRoleUser() {
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
    RequestData('POST', "/v1/Role/list", '', null, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $('#User-RoleFilter, #AddInternal-User-Role, #AddExternal-User-Role, #EditInternal-User-Role, #EditExternal-User-Role').html("");
            $('#User-RoleFilter, #AddInternal-User-Role, #EditInternal-User-Role').append(`<option value=""> -- Pilih Role --</option>`);
            data.List.forEach(function (item) {
                if (item.Id == 1 || item.Id == 5) {
                    $('#AddInternal-User-Role').append(`<option value="${item.Id}">${item.Name} </option>`);
                    $('#EditInternal-User-Role').append(`<option value="${item.Id}">${item.Name} </option>`);
                    $('#User-RoleFilter').append(`<option value="${item.Name}">${item.Name} </option>`);
                } else if (item.Id == 2) {
                    $('#AddExternal-User-Role').append(`<option value="${item.Id}">${item.Name} </option>`);
                    $('#EditExternal-User-Role').append(`<option value="${item.Id}">${item.Name} </option>`);
                    $('#User-RoleFilter').append(`<option value="${item.Name}">${item.Name} </option>`);
                }
                //$('#Edit-User-Role').append(`<option value="${item.Id}">${item.Name} </option>`);
                
            });
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}

function openEditUser(el) {
    var dataDetail = $(el).data('detail');
    console.log('dataedit', dataDetail.Id);
    openMenu(`/User/edit?id=${dataDetail.Id}`);
    //editUserDialog(dataDetail.Id);
}

//function AddUserDialog(el) {
//    console.log('detailhtml', dataDetail);
//    var dataDetail = $(el).data('detail');
//    openMenu(`/Views/User/Add`);
//}
