$(document).ready(function () {
    listUser();
    ListRole('#Search-User-Role-Container', '#Search-User-Role', null, true, null);
    $('#Search-User-Role, #Search-User-Status').on('change', function () {
        listUser();
    });
    $('#search-all').on('keyup', function () {
        listUser();
    });
});



function listUser(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#user-page_select').val();
    var element = {
        table: '#user-table',
        tbody: '#user-tbody',
        from: '#user-from_page',
        to: '#user-to_page',
        total: '#user-total',
        pagination: '#user-pagination',
        item_pagination: 'User-item'
    }
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
            {
                field: "name",
                search: $('#search-all').val()
            },
            {
                field: "role",
                search: $('#Search-User-Role').val()
            },
            {
                field: "status",
                search: $('#Search-User-Status').val()
            }
        ],
        sort: {
            field: "createdate",
            type: 1
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', "/v1/User/list", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.succeeded) {
            console.log('data', data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.list.length > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'listUser'
                }, function (count) {
                    data.list.forEach(function (item) {
                        var btnDelete = item.status == 'Active' ? `<a href="#!" class="dropdown-item text-danger" onclick="deleteUser('${item.id}', false);"><i class="ft-trash"></i> Deactivate</a>` : `<a href="#!" class="dropdown-item text-success" onclick="deleteUser('${item.id}', true);"><i class="ft-edit"></i> Activate</a>`;
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-nowrap text-center">${count}</td>
                                <td class="text-nowrap">${item.fullname}</td>
								<td class="text-nowrap">${item.username}</td>
                                <td class="text-nowrap">${item.mail}</td>
                                <td class="text-nowrap">${item.role.nama}</td>
                                <td class="text-nowrap">${item.status}</td>
                                <td class="text-center">
                                    <div>
                                        <button type="button" data-toggle="dropdown" class="btn btn-sm btn-info box-shadow-2" aria-hashpopup="true" aria-expanded="false"><i class="ft-list"></i></button>
                                          <div class="dropdown-menu">
                                                <a href="${window.location.origin}/Setting/UserEdit?id=${item.id}" class="dropdown-item"><i class="ft-edit"></i> Edit</a>
                                                ${btnDelete}
                                          </div>
                                   </div>
                                </td>
                            </tr>
                        `);
                        count++;
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

function deleteUser(id, value) {
    var theValue = value ? "Activated" : "Deactivated";
    ConfirmMessage('Apakah Anda Yakin?', isConfirm => {
        if (isConfirm) {
            var element = {
                tbody: '#user-tbody',
                tcontainer: '#user-table',
            };
            RequestData('PUT', `/v1/User/active/${id}/${value}`, element.tcontainer, element.tbody, null, function (data) {
                if (data.succeeded) {
                    AlertMessage("Data "+theValue+" Successfully", null, "success");
                    listUser();
                } else
                    ShowNotif(`${data.message} : ${data.description}`, "error");
            });
        }
    });
}
