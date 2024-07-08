
$(document).ready(function () {
    getListSatuanUnit();
});

function getListSatuanUnit(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#SatuanUnit-page_select').val();
    var element = {
        table: '#SatuanUnit-table',
        tbody: '#SatuanUnit-tbody',
        from: '#SatuanUnit-from_page',
        to: '#SatuanUnit-to_page',
        total: '#SatuanUnit-total',
        pagination: '#SatuanUnit-pagination',
        item_pagination: 'SatuanUnit-item'
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
    RequestData('POST', "/v1/SatuanJenis/list", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListSatuanUnit'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-nowrap">${count}</td>
								<td class="text-nowrap">${item.Kode}</td>									
                                <td class="text-nowrap">${item.Name}</td>

                                <td class="">
                                    <div class=dropdown-basic>
                                       <div class="dropdown">
                                          <div class="btn-group">
                                              <button class="dropbtn btn-success btn-sm" type="button">Action</button>
                                              <div class="dropdown-content">
                                              <a href="#" onclick="detailSatuanUnitDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>                                              
                                              <a href="#" onclick="editSatuanUnitDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                              <a href="#" onclick="deleteSatuanUnitDialog(this);" data-detail='${JSON.stringify(item)}'>Delete</a>
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
                SetTableData(false, 9, element);
            }
        } else {
            ShowNotif(`${data.Message} : ${data.Description}`, "error");
        }
    });
}