$(document).ready(function () {
    SetDatePicker('#EditProgram-StartTglPelaksanaan', "DD MMMM, YYYY", null, null, false);
    SetDatePicker('#EditProgram-EndTglPelaksanaan', "DD MMMM, YYYY", null, null, false);
    SetDatePicker('#EditProgram-BatasWaktu', "DD MMMM, YYYY", null, null, false);
    SetDatePicker('#EditProgram-BatasWaktu', "DD MMMM, YYYY", null, null, false);
    SetPicture('#photo_file', '#photo_img', null);
    getListLampiran();
    ListKegiatanReferensi('#EditProgram-Nama-Container', '#EditProgram-Nama', $('#EditProgram-NamaProgramId').val(), false, [{
        "field": "idJenisProgram",
        "search": `${$('#EditProgram-JenisProgram').val()}`
    }]);
    $('#EditProgram-JenisProgram').on('change', function () {
        ListKegiatanReferensi('#EditProgram-Nama-Container', '#EditProgram-Nama', null, false, [{
            "field": "idJenisProgram",
            "search": `${$(this).val()}`
        }]);
    });
});

function SaveProgram() {
    if (!FormValidate('Section-EditProgram')) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (!isConfirm) {
            return;
        }
        var photo = null;
        if ($('#photo_file')[0].files[0] != undefined) {
            let _base64;
            await FileToBase64($('#photo_file')[0].files[0])
                .then(dataBase64 => _base64 = dataBase64)
                .catch(error => {
                    AlertMessage(error);
                    return;
                });
            photo = {
                filename: $('#photo_file')[0].files[0].name,
                base64: _base64
            }
        }
        var id = $('#EditProgram-Id').val();
        console.log($('#EditProgram-LokasiKegiatan').val());
        var param = {
            deskripsi: $('#EditProgram-Deskripsi').val(),
            lokasi: $('#EditProgram-LokasiKegiatan').val(),
            idJenisProgram: $('#EditProgram-JenisProgram').val(),
            namaProgram: $('#EditProgram-Nama').val(),
            startPelaksanaan: StringToDateFormat($('#EditProgram-StartTglPelaksanaan').val(), "DD MMMM, YYYY"),
            endPelaksanaan: StringToDateFormat($('#EditProgram-EndTglPelaksanaan').val(), "DD MMMM, YYYY"),
            batasWaktuProgram: StringToDateFormat($('#EditProgram-BatasWaktu').val(), "DD MMMM, YYYY"),
            photo: photo
        }

        RequestData('PUT', `/v1/Program/edit/${id}`, '#Section-EditProgram', null, JSON.stringify(param), function (data_obj) {
            if (data_obj.succeeded) {
                AlertMessage("Data Berhasil Disimpan",null, "success");
            }
            else
                AlertMessage(data_obj.message);
        });
    });
}

function getListLampiran() {
   var id = $('#EditProgram-Id').val();
   RequestData('GET', `/v1/ProgramMedia/list/${id}`, '#DetailProgram-Lampiran', null, null, function (data) {
       if (data.succeeded) {
           $('#DetailProgram-ListLampiran').html('');
           if (data.list.length > 0) {
               data.list.forEach(function (item) {
                   $('#DetailProgram-ListLampiran').append(`
                           <div class="col-md-12 my-1">
                               <div class="bs-callout-blue-grey callout-border-left callout-bordered callout-transparent">
                                   <div class="card-header">
                                       <div class="row">
                                           <div class="d-flex align-self-center col-md-6 justify-content-start">
                                               <a href="${item.url.original}" target="_blank">${item.fileName}</a>
                                           </div>
                                           <div class="d-flex align-self-center col-md-6 justify-content-end">
                                               <button type="button" onclick="RemoveLampiran('${item.id}')" class="btn btn-sm btn-danger"><i class="ft-x"></i></button>
                                           </div>
                                       </div>
                                   </div>
                               </div>
                           </div>
                       `);
               });
           }
       } else {
           ShowNotif(`${data.message} : ${data.description}`, "error");
       }

   })
}

function showNameFile() {
    var fileName = $('#Addlampiran-File').val().replace('C:\\fakepath\\', " ");
    $('#Addlampiran-File').next('.custom-file-label').html(fileName);
}

function UploadLampiran() {

    if ($('#Addlampiran-File')[0].files[0] == undefined) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var file_attach = $('#Addlampiran-File')[0].files[0];
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
            RequestData('POST', `/v1/ProgramMedia/upload/${id}`, '#md-Addlampiran', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Diupload", null, "success");
                    $('#md-Addlampiran').modal('hide');
                    getListLampiran();
                } else {
                    AlertMessage(data_obj.message);
                }
            });

        }
    });
}

function RemoveLampiran(id) {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            RequestData('DELETE', `/v1/ProgramMedia/delete/${id}`, '#md-Addlampiran', null, null, function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Dihapus", null,"success");
                    getListLampiran();
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}

function SubmitProgram() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var id = $('#EditProgram-Id').val();
            RequestData('POST', `/v1/Program/submit/${id}`, '#Section-EditProgram', null, null, function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Dihapus", null, "success");
                    window.open(window.location.origin + `/Program/Index`)
                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}