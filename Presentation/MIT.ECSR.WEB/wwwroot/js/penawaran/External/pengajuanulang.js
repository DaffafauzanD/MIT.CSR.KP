//function editPenawaranExternalDialog(el) {
//    console.log("hai");
//    var data = $(el).data('detail');
//    $('.clear').val('');
//    $('#md-PengajuanUlang').modal('show');
//    $('#Add-PengajuanUlang-NamaProgram').val(data.Program.NamaProgram);
//    $('#Add-PengajuanUlang-JenisProgram').val(data.Program.JenisProgram.Nama);
//    $('#Add-PengajuanUlang-Tgl').val(DateToStringFormat(data.Program.TglPelaksanaan));
//    $('#Add-PengajuanUlang-tempat').val(data.Program.Kecamatan + "," + data.Program.Kelurahan);
//    $('#Add-PengajuanUlang-Deskripsi').val(data.Program.Deskripsi);   
//    $('#Add-PengajuanUlang-Lampiran').val(data.Program.Lampiran);
//    $('#PengajuanUlang-Deskripsi').val(data.Deskripsi);
//    data.Program.Items.forEach(function (list) {
//        $('#tbody-PengajuanUlang').append(`
//            <tr>
//				<td class="text-center">${list.Nama}</td>
//				<td class="text-center">${list.Jumlah}</td>								
//                <td class="text-center">${list.SatuanJenis.Nama}</td>
//           </tr>
//        `)
//    })

//    /*    Lampiran Program*/
//    data.Program.Lampiran.forEach(function (IdLampiran) {
//        if (IdLampiran.Id != null) {
//            GetFileProgramPengajuanUlang(IdLampiran.Id);
//            $('#Add-previewDokumenProgram').removeClass('display-none');
//        }
//        else {
//            $('#Add-previewDokumenProgram').addClass('display-none');
//        }
//    })
//    /*    Lampiran Penawaran*/
//    data.Lampiran.forEach(function (IdLampiranPenawaran) {
//        if (IdLampiranPenawaran.Id != null) {
//            GetFilePenawaranSebelumnya(IdLampiranPenawaran.Id);
//            $('#Add-previewDokumenPenawaran').removeClass('display-none');
//        }
//        else {
//            $('#Add-previewDokumenPenawaran').addClass('display-none');
//        }
//    })

//    $('#md-PengajuanUlang').data('Id', data.Id);
//    $('#PengajuanUlang-create_btn').unbind();
//    $('#PengajuanUlang-create_btn').on('click', function () {
//        editPengajuanUlangSave();
//    });
//}
//function GetFileProgramPengajuanUlang(Id) {
//    RequestData('GET', `/v1/Media/download/${Id}`, '', null, null, function (data_obj) {
//        if (data_obj.Succeeded) {
//            $('#Add-PengajuanUlang-Lampiran').text(data_obj.Data.Filename, 'data:application/pdf;base64,' + data_obj.Data.Base64);
//        }
//        else if (data_obj.code == 401) { //unathorized
//            AlertMessage(data_obj.message);
//        } else
//            ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
//    });
//}

//function GetFilePenawaranSebelumnya(Id) {
//    RequestData('GET', `/v1/Media/download/${Id}`, '', null, null, function (data_obj) {
//        if (data_obj.Succeeded) {
//            $('#add-Penawaran-LampiranSebelumnya').text(data_obj.Data.Filename, 'data:application/pdf;base64,' + data_obj.Data.Base64);
//        }
//        else if (data_obj.code == 401) { //unathorized
//            AlertMessage(data_obj.message);
//        } else
//            ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
//    });
//}


function editPengajuanUlangSave() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var submitAttachment = "";
            var file_attach = $('#Add-PengajuanUlang-File')[0].files[0];
            if (file_attach != undefined && file_attach != null) {
                await FileToBase64(file_attach)
                    .then(dataBase64 => submitAttachment = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }
            var param = {
                Deskripsi: $('#Add-PengajuanUlang-Comment').val(),
                Lampiran: [
                    {
                        Filename: file_attach.name,
                        Base64: submitAttachment
                    }
                ]

            }
            console.log("file ATTACK", param);
            RequestData('PUT', `/v1/Penawaran/edit/${$('#md-PengajuanUlang').data('Id')}`, '#md-PengajuanUlang .modal-content', null, JSON.stringify(param), function (data_obj) {
                console.log(data_obj);
                if (data_obj.Succeeded) {

                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-PengajuanUlang').modal('hide');
                    getListPenawaranProgramExternal();
                    getListPenawaranExternal();
                    getListHistoryExternal();
                    getListPenawaranExternalFilter();
                    getListHistoryExternalFilter();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.message);
                } else
                    ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
            });
        }
    });
}
function addPengajuanUlangReset() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            $('.clear').val('');
            $('#md-PengajuanUlang').modal('show');
            $('#PengajuanUlang-Deskripsi').val('');
            $('#Add-PengajuanUlang-File').val('');
        }
    });
}

