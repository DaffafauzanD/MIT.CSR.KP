
$(document).ready(function () {
    getListJenisProgram();
});

function getListJenisProgram(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#JenisProgram-page_select').val();
    var element = {
        table: '#JenisProgram-table',
        tbody: '#JenisProgram-tbody',
        from: '#JenisProgram-from_page',
        to: '#JenisProgram-to_page',
        total: '#JenisProgram-total',
        pagination: '#JenisProgram-pagination',
        item_pagination: 'JenisProgram-item'
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
    RequestData('POST', "/v1/JenisProgram/list", element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log('data', data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.List.length > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.Count,
                    function_name: 'getListJenisProgram'
                }, function (count) {
                    data.List.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-nowrap text-center">${count}</td>
								<td class="text-nowrap">${item.Name}</td>							

                                <td class="">
                                    <div class=dropdown-basic>
                                       <div class="dropdown">
                                          <div class="btn-group">
                                              <button class="dropbtn btn-success btn-sm" type="button">Action</button>
                                              <div class="dropdown-content">
                                                  <a href="#" onclick="detailJenisProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Detail</a>                                              
                                                  <a href="#" onclick="editJenisProgramDialog(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'>Edit</a>
                                                  <a href="#" onclick="deleteJenisProgramDialog(this);" data-detail='${JSON.stringify(item)}'>Delete</a>
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