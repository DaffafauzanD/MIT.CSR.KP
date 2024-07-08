$(document).ready(function () {
    ListBanner();
});



function ListBanner(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#banner-page_select').val();
    var element = {
        table: '#banner-table',
        tbody: '#banner-tbody',
        from: '#banner-from_page',
        to: '#banner-to_page',
        total: '#banner-total',
        pagination: '#banner-pagination',
        item_pagination: 'banner-item'
    }

    RequestData('GET', '/v1/Banner/list', element.table, element.tbody, null, function (data) {
        if (data.succeeded) {
            console.log('data', data);
            $(element.tbody).html('');
            $(element.pagination).html('');
            if (data.list.length > 0) {
                SetTableData(true, 9, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'listBanner'
                }, function (count) {
                    data.list.forEach(function (item) {
                        $(element.tbody).append(`
                            <tr>
                                <td class="text-nowrap text-center">${count}</td>
                                <td class="text-nowrap">
                                   <figure class="col-lg-6 col-md-12 col-12" itemprop="associatedMedia" itemscope="" itemtype="http://schema.org/ImageObject">
                                     <a href="${item.url.original}" itemprop="contentUrl" data-size="900x300">
                                        <img class="img-thumbnail img-fluid" src="${item.url.resize}" itemprop="thumbnail" alt="Image description">
                                     </a>
                                   </figure>
                               </td>
								<td class="text-nowrap">${item.fileName}</td>
                                <td class="text-nowrap">${item.createBy}</td>
                                <td class="text-nowrap">${item.createDate}</td>
                                <td class="text-nowrap">
                                    <button class="btn-sm btn-danger box-shadow-2 px-2" type="button" onClick="RemoveGallery('${item.id}')"><i class="ft-trash icon-left"></i> Hapus</button>
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


function showNameFile() {
    var fileName = $('#AddBanner-File').val().replace('C:\\fakepath\\', " ");
    $('#AddBanner-File').next('.custom-file-label').html(fileName);
}

function UploadGallery() {

    if ($('#AddBanner-File')[0].files[0] == undefined) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var file_attach = $('#AddBanner-File')[0].files[0];
            var base64 = "";
            if (file_attach != undefined && file_attach != null) {
                await FileToBase64(file_attach)
                    .then(dataBase64 => base64 = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }
            var param = {
                filename: file_attach.name,
                base64: base64
            };

            var id = $('#EditProgram-Id').val();
            RequestData('POST', `/v1/Banner/upload`, '#md-AddBanner', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Diupload", null, "success");
                    $('#md-AddBanner').modal('hide');
                    ListBanner();
                } else {
                    AlertMessage(data_obj.message);
                }
            });

        }
    });
}
function RemoveGallery(id) {
    ConfirmMessage('Apakah Anda Yakin Akan Menghapus Data Ini?', isConfirm => {
        if (isConfirm) {
            var element = {
                tbody: '#banner-tbody',
                tcontainer: '#banner-tbody',
            };
            RequestData('DELETE', `/v1/Banner/delete/${id}`, element.tcontainer, element.tbody, null, function (data) {
                if (data.succeeded) {
                    AlertMessage("Data Deleted Successfully ...",null, "success");
                    ListBanner();
                } else
                    ShowNotif(`${data.message} : ${data.description}`, "error");
            });
        }
    });
}


