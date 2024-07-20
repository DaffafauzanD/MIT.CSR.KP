var rolename = $('#rolename').val();
$(document).ready(function () {
    SetDatePicker('#Program-StartTglPelaksanaan', "DD MMMM, YYYY", null, null, false, null);
    SetDatePicker('#Program-EndTglPelaksanaan', "DD MMMM, YYYY", null, null, false, null, function () {
        setTimeout(ListProgram,
            1000);
    });

    ListJenisProgram("#Program-JenisProgram-Container", "#Program-JenisProgram", null, true);
    ("#Program-Kegiatan-Container", null, true);
    $('#Program-JenisProgram').on('change', function () {
        var param = [{

            "field": "idjenisprogram",
            "search": `${$(this).val()}`
        }];
        var boolean = false;
        if ($(this).val() == "") {
            param = null;
            boolean = true;
        }

        ListKegiatanReferensi('#Program-Kegiatan-Container', '#Program-Kegiatan', null, boolean, param);
    });
    ListProgram();
    $('#btn-export').on('click', function () {
        ExportProgram();
    });
});


function ListProgram(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#Program-page_select').val();
    var element = {
        table: '#Program-table',
        tbody: '#Program-tbody',
        from: '#Program-from_page',
        to: '#Program-to_page',
        total: '#Program-total',
        pagination: '#Program-pagination',
        item_pagination: 'Program-item'
    }
    var param = {
        filter: [
            {
                field: "idjenisprogram",
                search: $('#Program-JenisProgram').val()
            },
            {
                field: "kegiatan",
                search: $('#Program-Kegiatan').val()
            },
            {
                field: "status",
                search: $('#Program-Status').val()
            },
            {
                field: "namaprogram",
                search: $('#Program-Search').val()
            },
            {
                field: "tglpelaksanaan",
                search: $('#Program-StartTglPelaksanaan').val() ? StringToDateFormat($('#Program-StartTglPelaksanaan').val(), "DD MMMM, YYYY") + "|" + StringToDateFormat($('#Program-EndTglPelaksanaan').val(), "DD MMMM, YYYY") : ""
            }
        ], 
        sort: {
            field: "updatedate",
            type: 1
        },
        //id_jenis_program: $('#Program-JenisProgram').val(),
        //search: $('#Program-Search').val(),
        start: page,
        length: pageSize
    }
    RequestData('POST', '/v1/Program/list', element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.succeeded) {
            console.log("data",data);
            if (data.list.length > 0) {
                SetTableData(true, 8, element, {
                    page: page,
                    pageSize: pageSize,
                    count: data.count,
                    function_name: 'ListProgram'
                }, function (count) {
                    data.list.forEach(function (item) {
                        let color = 'bg-primary';
                        let can_delete = false;
                        let can_edit = false;
                        let can_approve = false;
                        switch (item.status.toUpperCase()) {
                            case 'OPEN':
                                color = 'badge-info';
                                can_edit = true;
                                break;
                            case 'DRAFT':
                                color = 'badge-warning'
                                can_edit = true;
                                can_delete = true;
                                break;
                            case 'ON PROGRESS':
                                color = 'badge-success';
                                can_edit = true;
                                break;
                            case 'CLOSED':
                                color = 'badge-success';
                                break;
                            case 'EXPIRED':
                                color = 'badge-danger';
                                break;
                            case 'WAITING VERIFIKASI':
                                color = 'badge-warning';
                                can_approve = true;
                                break;
                            case 'WAITING APPROVAL':
                                color = 'badge-warning';
                                can_approve = true;
                                break;
                            case 'REJECT VERIFIKASI':
                                can_edit = true;
                                color = 'badge-danger';
                                break;
                            case 'REJECT APPROVAL':
                                can_edit = true;
                                color = 'badge-danger';
                                break;
                        }

                        if (!rolename.includes("BAPPEDA")) {
                            if (rolename.includes("OPD") && item.status.toUpperCase() == "WAITING VERIFIKASI") {
                                can_approve = true;
                            } else {
                                can_approve = false;
                                can_delete = false;
                            }
                        }

                        let delete_action = `<a href="#!" class="dropdown-item text-danger" onclick="deleteProgram('${item.id}');"><i class="ft-trash"></i> Delete</a>`;
                        let edit_action = `<a href="${window.location.origin}/Program/Edit/${item.id}" class="dropdown-item"><i class="ft-edit"></i> Edit</a>`;
                        let approval_action = `<a href="#" class="dropdown-item" onclick="showApproval(this, ListProgram);" data-detail="${item.id}"><i class="ft-edit"></i> ${item.status == 'WAITING VERIFIKASI' ? 'Verifikasi' : 'Approve'}</a>`;
                        let status = `<div class="badge ${color}"><span>${item.status}</span></div>`;
                        $(element.tbody).append(`
                                  <tr>
                                        <td class="text-center">${count}</td>
                                        <td>${item.jenisProgram.nama}</td>
                                        <td>${item.namaProgram.nama}</td>
                                        <td class="text-center"><span class="badge badge-pill badge-info">${item.kegiatan}</span></td>
                                        <td>
                                            <ul style="list-style-type: none;padding-left: 0;">
                                                <li>${DateToStringFormat(item.startTglPelaksanaan)}</li>
                                                <li>${DateToStringFormat(item.endTglPelaksanaan)}</li>
                                            </ul>
                                        </td>
                                      
                                        <td>
                                            <ul style="list-style-type: none;padding-left: 0;">
                                                <li class="text-right"><span class="badge badge-info">${item.unit} Unit</span></li>
                                                <li class="text-right">Rp. ${formatNumber(item.rupiah)}</li>
                                            </ul>
                                        </td>
                                        <td class="text-center">${status}</td>
                                        <td class="text-center">${item.notes ? item.notes : "-"}</td>
                                        <td class="text-center">${item.approvedBy ? item.approvedBy : "-"}</td>
                                        <td class="text-center">${item.approvedAt ? DateToStringFormat(item.updateDate) : "-"}</td>
                                        <td class="text-center">${item.createBy}</td>
                                        <td class="text-center">
                                            <div>
                                                <button type="button" data-toggle="dropdown" class="btn btn-sm btn-info box-shadow-2" aria-hashpopup="true" aria-expanded="false"><i class="ft-list"></i></button>
                                                <div class="dropdown-menu">
                                                    <a href="${window.location.origin}/Program/Detail/${item.id}" class="dropdown-item"><i class="ft-book"></i> Detail</a>
                                                    ${can_edit ? edit_action : ``}
                                                    ${can_delete ? delete_action : ``}
                                                    ${can_approve ? approval_action : ``}
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

    })
}

function deleteProgram(id) {
    ConfirmMessage('Apakah Anda Yakin Akan Menghapus Data Ini?', isConfirm => {
        if (isConfirm) {
            var element = {
                tbody: '#Program-tbody',
                tcontainer: '#Program-table',
            };
            RequestData('DELETE', `/v1/Program/delete/${id}`, element.tcontainer, element.tbody, null, function (data) {
                if (data.succeeded) {
                    AlertMessage("Data Deleted Successfully", null, "success");
                    ListProgram();
                } else
                    ShowNotif(`${data.message} : ${data.description}`, "error");
            });
        }
    });
}

function ExportProgram(page) {
    page = page != undefined ? page : 1;
    var pageSize = $('#Program-page_select').val();
    var param = {
        filter: [
            {
                field: "idjenisprogram",
                search: $('#Program-JenisProgram').val()
            },
            {
                field: "kegiatan",
                search: $('#Program-Kegiatan').val()
            },
            {
                field: "status",
                search: $('#Program-Status').val()
            },
            {
                field: "namaprogram",
                search: $('#Program-Search').val()
            },
            {
                field: "tglpelaksanaan",
                search: $('#Program-StartTglPelaksanaan').val() ? StringToDateFormat($('#Program-StartTglPelaksanaan').val(), "DD MMMM, YYYY") + "|" + StringToDateFormat($('#Program-EndTglPelaksanaan').val(), "DD MMMM, YYYY") : ""
            }
        ],
        sort: {
            field: "updatedate",
            type: 1
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', '/v1/Program/export', '', '', JSON.stringify(param), function (data) {
        if (data.succeeded) {
            var sampleArr = base64ToArrayBuffer(data.data);
            saveByteArray(`Program-Report-${DateToString(new Date(), 'DD-MM-YYYY')}`, sampleArr);
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}

function saveByteArray(reportName, byte) {
    var blob = new Blob([byte], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = reportName;
    link.download = fileName;
    link.click();
}

function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}