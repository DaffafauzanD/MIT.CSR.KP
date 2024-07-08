$(document).ready(function () {
    ListJenisProgram("#Progress-JenisProgram-Container", "#Progress-JenisProgram", null, true);
    $('#Progress-JenisProgram').on('change', function () {
        var param = [{

            "field": "idjenisprogram",
            "search": `${$(this).val()}`
        }];
        var boolean = false;
        if ($(this).val()=="") {
            param = null;
            boolean = true;
        }
        ListKegiatanReferensi('#Progress-Kegiatan-Container', '#Progress-Kegiatan', null, boolean, param);
    });
    ListProgress();
});


function ListProgress(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#Progress-page_select').val();
    var element = {
        table: '#Progress-table',
        tbody: '#Progress-tbody',
        from: '#Progress-from_page',
        to: '#Progress-to_page',
        total: '#Progress-total',
        pagination: '#Progress-pagination',
        item_pagination: 'Progress-item'
    }
    var param = {
        filter: [
            {
                field: "companyname",
                search: $('#Progress-Perusahaan').val()
            },
            {
                field: "jenisprogram",
                search: $('#Progress-JenisProgram').val()
            },
         
            {
                field: "status",
                search: $('#Progress-Status').val()
            },
           
            {
                field: "itemName",
                search: $('#Progress-Search').val()
            }
        ],
        sort: {
            field: "createdate",
            type: 1
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', '/v1/ProgressProgram/list', element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.succeeded) {
            console.log(data);
            if (data.list.length > 0) {
                SetTableData(true, 13, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListProgress'
                }, function (count) {
                    data.list.forEach(function (item) {
                        var status = '';
                        var approve = '';
                        switch (item.status) {
                            case 1:
                                status = '<div class="badge badge-info"><span>Waiting</span></div>';
                                approve = `<a href="#" class="dropdown-item" onclick="showApproval(this);" data-detail='${JSON.stringify(item).replace(/' /g, " ")}'"><i class="ft-edit"></i> Approval</a>`;
                                break;
                            case 2:
                                status = '<div class="badge badge-success"><span>Approve</span></div>';
                                break;
                            case 3:
                                status = '<div class="badge badge-danger"><span>Reject</span></div>';
                                break;
                        }
                        $(element.tbody).append(`<tr>
                                                    <td class="text-center">${count}</td>
                                                    <td>${item.companyName}</td>
                                                    <td>${item.jenisProgram}</td>
                                                    <td>${item.namaProgram}</td>
                                                     <td>${item.itemName}</td>
                                                    <td>${item.item} ${item.satuanUnit}</td>
                                                    <td>${item.deskripsi}</td>
                                                    <td class="text-right">${DateToStringFormat(item.createDate)}</td>
                                                    <td>${status}</td>
                                                    <td>${item.notes != null ? item.notes : `-`}</td>
                                                    <td>${item.approvedBy}</td>
                                         <td class="text-center">
                                            <a href="#" data-detail='${JSON.stringify(item)}' onclick="Detail(this)" type="button" class="btn btn-sm btn-info box-shadow-2">Detail</a>
                                        </td>
                                                    
                                                </tr>
                            `);
                        count++;

                    });
                }
                )
            } else {
                SetTableData(false, 13, element);
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }

    })
}

function showApproval(el) {
    var data = $(el).data('detail');
    console.log(data)
    $('.clear').val('');
    $('#md-Progress_Approval').modal('show');
    $('#Progress_Approval-Notes').val('');
    $('#Progress_Approval-Id').val(data.id);
}

function ApprovalProgress(approvalType) {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var param = {
                id: $('#md-approvalProgress-Id').val(),
                isApprove: approvalType,
                notes: $('#md-approvalProgress-notes').val()
            }
            RequestData('POST', `/v1/ProgressProgram/approve`, '#md-Progress_Approval', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Disimpan", null, "success");
                    $('#md-Progress_Approval').modal('hide');

                    $('#md-approveProgress').modal('hide')
                    ListProgress();
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}

function Detail(el) {
    var data = $(el).data('detail');
    $('#md-approveProgress-company').text(data.companyName);
    $('#md-approveProgress-program').text(data.jenisProgram);
    $('#md-approveProgress-activity').text(data.namaProgram);
    $('#md-approveProgress-subactivity').text(data.itemName);
    $('#md-approveProgress-presentage').text(data.progress + ' %');
    $('#md-approveProgress-description').text(data.deskripsi);
    $('#md-approveProgress-attachment').html("");
    $('#md-approvalProgress-Id').val(data.id);
    data.lampiran.forEach(function (item) {
        $('#md-approveProgress-attachment').append(`
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <span class="col-10 justify-content-start"><i class="ft-file"></i> ${item.filename}</span> <span class="col-2 d-flex justify-content-end"><a href="${item.original}"><i class="ft-external-link"></i></a></span>
                </div>
            </div>
        </div>
    `)
    })
    $('#md-approveProgress').modal('show')
}