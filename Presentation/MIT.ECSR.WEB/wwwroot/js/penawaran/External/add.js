var idlampiranprogram;
function PengajuanExternalDialog(el) {
    var list_id = [];
    var data = $(el).data('detail');
    data.Lampiran.forEach(function (el) {
        console.log("TES :", el);
        list_id.push(el.Id);
    });
    console.log("ID NYA", list_id);
    $('.clear').val('');
    $('#md-Add-Penawaran').modal('show');
    console.log(data);
    addPengajuanReset();
    $('#Add-Penawaran-NamaProgram').val(data.NamaProgram);
    $('#Add-Penawaran-JenisProgram').val(data.JenisProgram.Nama);
    $('#Add-Penawaran-Tgl').val(DateToStringFormat(data.TglPelaksanaan));
    $('#Add-Penawaran-tempat').val(data.Kecamatan + "," + data.Kelurahan);
    $('#Add-Penawaran-Deskripsi').val(data.Deskripsi);
    data.Items.forEach(function (list) {
        $('#tbody-PengajuanProgram').append(`
            <tr>
				<td class="text-center">${list.Nama}</td>
				<td class="text-center">${list.Jumlah}</td>								
                <td class="text-center">${list.SatuanJenis.Nama}</td>
           </tr>
        `)
    });
    //if (data.PhotoUrl != null) {
    //    $('#Add-Penawaran-LampiranPhoto').val(data.PhotoUrl);
    //    $('#Add-Penawaran-LampiranPhoto').click(function () {
    //        window.location.href = data.PhotoUrl;
    //    });
    //}
    //else {
    //    $('#Add-Penawaran-LampiranPhoto').text('');
    //}

    $('#Add-Penawaran-Lampiran').html("");
    data.Lampiran.forEach(function (Lampiran) {
        $('#Add-Penawaran-Lampiran').append(`
            <div class="card col-4 mx-1">
                <h4 onclick="GetFileProgram('${Lampiran.Id}')" class="card-title text-truncate">${Lampiran.Filename} <i class="fa fa-download"></i></h4>
            </div>
        `)
    })

    $('#md-Add-Penawaran').data('Id', data.Id);
    $('#Pengajuan-create_btn').unbind();
    $('#Pengajuan-create_btn').on('click', function () {
            addPengajuanSave();
    });

}

var idDynamicLampiran = 0;
var arrayIdDynamicLampiran = [];
//lampiran dynamic
$('#lampiranAppend').click(function () {
    idDynamicLampiran++;
    arrayIdDynamicLampiran.push({
        id: idDynamicLampiran,
        lampiran: `#Add-Program-Lampiran-${idDynamicLampiran}`
    });

    $('#lampiranPenawaranAppend').append(`
                <div class="row" id="deleteParentLampiran-${idDynamicLampiran}">
                    <div class="col-md-6">
                        <label>File</label>
                        <input type="file" class="form-control form-control-sm" id="Add-Program-Lampiran-${idDynamicLampiran}">
                    </div>
                    <div class="col-md-1">
                        <label>&nbsp</label>
                        <button class="btn btn-sm btn-danger d-block" onclick="deleteAppendLampiran(${idDynamicLampiran})"><i class="fa fa-trash"></i></button>
                    </div>
                </div>
            `);
});

$('#lampiranReset').click(function () {
    $('#lampiranPenawaranAppend').html('');
    arrayIdDynamicLampiran = [];
});

function deleteAppendLampiran(id) {
    $('#deleteParentLampiran-' + id).remove();
    arrayIdDynamicLampiran = arrayIdDynamicLampiran.filter(function (obj) {
        return obj.id !== id;
    });
}


function addPengajuanSave() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {

            var submitAttachment = "";
            var file_attach = $('#Add-Penawaran-File')[0].files[0];
            if (file_attach != undefined && file_attach != null) {
                await FileToBase64(file_attach)
                    .then(dataBase64 => submitAttachment = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }
            if (file_attach == null) {
                ShowNotif("Data gagal", "failed");
                $('#Add-Penawaran-FileFailed').append(`Data Tidak Boleh Kosong`);
            }

            //var arrayParamLampiran = [];
            //console.log("isi", arrayParamLampiran.Data);
            //for (let x = 0; x < arrayIdDynamicLampiran.length; x++) {
            //    if ($(arrayIdDynamicLampiran[x].lampiran)[0].files[0] != undefined && $(arrayIdDynamicLampiran[x].lampiran)[0].files[0] != null) {
            //        var submitLampiran = "";
            //        await FileToBase64($(arrayIdDynamicLampiran[x].lampiran)[0].files[0])
            //            .then(dataBase64 => submitLampiran = dataBase64)
            //            .catch(error => {
            //                AlertMessage(error);
            //                return;
            //            });
            //        fileLampiranAttachment = $(arrayIdDynamicLampiran[x].lampiran)[0].files[0].name;
            //        arrayParamLampiran.push({
            //            Base64: submitLampiran,
            //            Filename: fileLampiranAttachment
            //        });
            //    };
            //}

            var param = {
                Deskripsi: $('#Add-Pengajuan-Deskripsi').val(),
                Lampiran: [
                    {
                        Filename: file_attach.name,
                        Base64: submitAttachment
                    }
                ]
/*                Lampiran: arrayParamLampiran*/


            }
            console.log("file ATTACK", param);
            RequestData('POST', `/v1/Penawaran/add/${$('#md-Add-Penawaran').data('Id')}`, '#md-Add-Penawaran .modal-content', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    console.log("Berhasil", data_obj);
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-Add-Penawaran').modal('hide');
                    $('#lampiranPenawaranAppend').html('');
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
                $('#Add-Penawaran-DeskFailed').append(`Data Tidak Boleh Kosong`);
            });
        }
    });
}
function addPengajuanReset() {

            $('.clear').val('');
    $('#Add-Pengajuan-Deskripsi').val('');
    $('#Add-Penawaran-DeskFailed').text('');
    $('#Add-Penawaran-File').val('');

}
function addPengajuanResetFile() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            $('.clear').val('');
            $('#Add-Penawaran-File').val('');
        }
    });
}

function GetFileProgram(Id) {
    console.log(Id);
    if (Id == "") {
        AlertMessage("Lampiran Not Found");
    } else {
        RequestData('GET', `/v1/Media/download/${Id}`, '', null, null, function (data_obj) {
            if (data_obj.Succeeded) {
                Base64ToFile(data_obj.Data.Filename, "application/octet-stream", data_obj.Data.Base64);
                /*            $('#Detail-LampiranProgram').text(data_obj.Data.Filename, 'data:application/pdf;base64,' + data_obj.Data.Base64);*/
            }
            else if (data_obj.code == 401) { //unathorized
                AlertMessage(data_obj.message);
            } else
                ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
        });
    }

}


