$(document).ready(function () {
    $('#program-addBtn').on('click', function () {
        $('.clear').val('');
        $('#itemProgramAppend').html('');
        $('#lampiranProgramAppend').html('');
        $('#md-Program-add').modal('show');

        $('#Program-create_btn').unbind();
        $('#Program-create_btn').on('click', function () {
            addProgramSave();
        });

        $('#Program-draft_btn').unbind();
        $('#Program-draft_btn').on('click', function () {
            addProgramSaveAsDraft();
        });
    });
    ListJenisProgram();
    ListDatiProgram();
    getListSatuanJenis();
});

var idDynamic = 0;
var idDynamicLampiran = 0;
//var idDynamicPhoto = 0;

var arrayIdDynamic = [];
var arrayIdDynamicLampiran = [];
//var arrayIdDynamicPhoto = [];

$('#programAppend').click(function () {
    idDynamic++;
    arrayIdDynamic.push({
        id: idDynamic,
        item: `#Add-Program-NamaItem-${idDynamic}`,
        qty: `#Add-Program-JumlahItem-${idDynamic}`,
        jenis: `#Add-Program-idSatuanJenis-${idDynamic}`
    });

    $('#itemProgramAppend').append(`
                <div class="row" id="deleteParent-${idDynamic}">
                    <div class="col-md-6">
                        <label>Nama Item</label>
                        <input type="text" class="form-control form-control-sm" id="Add-Program-NamaItem-${idDynamic}">
                    </div>
                    <div class="col-md-2">
                        <label>Qty</label>
                        <input type="number" class="form-control form-control-sm" id="Add-Program-JumlahItem-${idDynamic}">
                    </div>
                    <div class="col-md-2">
                        <label>Satuan</label>
                        <select class="form-control form-control-sm d-block Add-Program-idSatuanJenis" id="Add-Program-idSatuanJenis-${idDynamic}">
                        </select>
                    </div>
                    <div class="col-md-1">
                        <label>&nbsp</label>
                        <button class="btn btn-sm btn-danger d-block" onclick="deleteAppend(${idDynamic})"><i class="fa fa-trash"></i></button>
                    </div>
                </div>
    `)
    getListSatuanJenis();
});

$('#programReset').click(function () {
    $('#itemProgramAppend').html('');
     arrayIdDynamic = [];
});

function deleteAppend(id) {
    $('#deleteParent-' + id).remove();
    arrayIdDynamic = arrayIdDynamic.filter(function (obj) {
        return obj.id !== id;
    });
}

//lampiran dynamic
$('#lampiranAppend').click(function () {
    idDynamicLampiran++;
    arrayIdDynamicLampiran.push({
        id: idDynamicLampiran,
        lampiran: `#Add-Program-Lampiran-${idDynamicLampiran}`
    });

    $('#lampiranProgramAppend').append(`
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
    $('#lampiranProgramAppend').html('');
    arrayIdDynamicLampiran = [];
});

function deleteAppendLampiran(id) {
    $('#deleteParentLampiran-' + id).remove();
    arrayIdDynamicLampiran = arrayIdDynamicLampiran.filter(function (obj) {
        return obj.id !== id;
    });
}
//end lampiran dynamic

//photo dynamic
//$('#photoAppend').click(function () {
//    idDynamicPhoto++;
//    arrayIdDynamicPhoto.push({
//        id: idDynamicPhoto,
//        photo: `#Add-Program-Photo-${idDynamicPhoto}`
//    });

//    $('#photoProgramAppend').append(`
//                <div class="row" id="deleteParentPhoto-${idDynamicPhoto}">
//                    <div class="col-md-6">
//                        <label>Photo</label>
//                        <input type="file" class="form-control form-control-sm" id="Add-Program-Photo-${idDynamicPhoto}">
//                    </div>
//                    <div class="col-md-1">
//                        <label>&nbsp</label>
//                        <button class="btn btn-sm btn-danger d-block" onclick="deleteAppendPhoto(${idDynamicPhoto})"><i class="fa fa-trash"></i></button>
//                    </div>
//                </div>
//            `);
//});

//$('#photoReset').click(function () {
//    $('#photoProgramAppend').html('');
//    idDynamicPhoto = [];
//});

//function deleteAppendPhoto(id) {
//    $('#deleteParentPhoto-' + id).remove();
//    arrayIdDynamicPhoto = arrayIdDynamicPhoto.filter(function (obj) {
//        return obj.id !== id;
//    });
//}
//end photo dynamic

function addProgramSave() {

    ConfirmMessage('Apakah Anda Yakin Akan Menyimpan Data Ini?', async isConfirm => {
        if (isConfirm) {

            var arrayParam = [];

            arrayIdDynamic.forEach(function (item) {
                arrayParam.push({
                    IdSatuanJenis: $(item.jenis).val(),
                    Jumlah: $(item.qty).val(),
                    Nama: $(item.item).val()
                });
            });

            var arrayParamLampiran = [];
            for (let x = 0; x < arrayIdDynamicLampiran.length; x++) {
                if ($(arrayIdDynamicLampiran[x].lampiran)[0].files[0] != undefined && $(arrayIdDynamicLampiran[x].lampiran)[0].files[0] != null) {
                    var submitLampiran = "";
                    await FileToBase64($(arrayIdDynamicLampiran[x].lampiran)[0].files[0])
                        .then(dataBase64 => submitLampiran = dataBase64)
                        .catch(error => {
                            AlertMessage(error);
                            return;
                        });
                    fileLampiranAttachment = $(arrayIdDynamicLampiran[x].lampiran)[0].files[0].name;
                    arrayParamLampiran.push({
                        Base64: submitLampiran,
                        Filename: fileLampiranAttachment
                    });
                };
            }

            var submitAttachment = "";
            var file_attach = $('#Add-Program-Photo')[0].files[0];
            if (file_attach != undefined && file_attach != null) {
                console.log("file_attach", file_attach);
                await FileToBase64(file_attach)
                    .then(dataBase64 => submitAttachment = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }

            var param = {
                Deskripsi: $('#Add-Program-Deskripsi').val(),
                IdDati: $('#Add-Program-Tempat').val(),
                IdJenisProgram: $('#Add-Program-JenisProgram').val(),
                NamaProgram: $('#Add-Program-NamaProgram').val(),
                TglPelaksanaan: $('#Add-Program-TglPelaksanaan').val(),
                Photo: {
                    Filename: file_attach.name,
                    Base64: submitAttachment
                },
                Items: arrayParam,
                Lampiran: arrayParamLampiran
            }
            console.log("Param ", param);

            RequestData('POST', `/v1/Program/add/${false}`, '#md-Program-add', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-Program-add').modal('hide');
                    getListProgramInternal();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });

    

    //var arrayParamPhoto = [];


    //for (let i = 0; i < arrayIdDynamicPhoto.length; i++) {
    //    if ($(arrayIdDynamicPhoto[i].photo)[0].files[0] != undefined && $(arrayIdDynamicPhoto[i].photo)[0].files[0] != null) {
    //        var submitAttachment = "";
    //        await FileToBase64($(arrayIdDynamicPhoto[i].photo)[0].files[0])
    //            .then(dataBase64 => submitAttachment = dataBase64)
    //            .catch(error => {
    //                AlertMessage(error);
    //                return;
    //            });
    //        fileNameAttachment = $(arrayIdDynamicPhoto[i].photo)[0].files[0].name;
    //        arrayParamPhoto.push({
    //            Base64: submitAttachment,
    //            Filename: fileNameAttachment
    //        });
    //    };
    //};

    //arrayIdDynamicLampiran.forEach(async function (item) {
        
    //});


}

function addProgramSaveAsDraft() {
    ConfirmMessage('Apakah Anda Yakin Akan Menyimpan Data Ini?', async isConfirm => {
        if (isConfirm) {

            var arrayParam = [];

            arrayIdDynamic.forEach(function (item) {
                arrayParam.push({
                    IdSatuanJenis: $(item.jenis).val(),
                    Jumlah: $(item.qty).val(),
                    Nama: $(item.item).val()
                });
            });

            var arrayParamLampiran = [];

            for (let x = 0; x < arrayIdDynamicLampiran.length; x++) {
                if ($(arrayIdDynamicLampiran[x].lampiran)[0].files[0] != undefined && $(arrayIdDynamicLampiran[x].lampiran)[0].files[0] != null) {
                    var submitLampiran = "";
                    await FileToBase64($(arrayIdDynamicLampiran[x].lampiran)[0].files[0])
                        .then(dataBase64 => submitLampiran = dataBase64)
                        .catch(error => {
                            AlertMessage(error);
                            return;
                        });
                    fileLampiranAttachment = $(arrayIdDynamicLampiran[x].lampiran)[0].files[0].name;
                    arrayParamLampiran.push({
                        Base64: submitLampiran,
                        Filename: fileLampiranAttachment
                    });
                };
            }

            var submitAttachmentDraft = "";
            var file_attach_draft = $('#Add-Program-Photo')[0].files[0];
            if (file_attach_draft != undefined && file_attach_draft != null) {
                await FileToBase64(file_attach_draft)
                    .then(dataBase64 => submitAttachmentDraft = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }

            var param = {
                Deskripsi: $('#Add-Program-Deskripsi').val(),
                IdDati: $('#Add-Program-Tempat').val(),
                IdJenisProgram: $('#Add-Program-JenisProgram').val(),
                NamaProgram: $('#Add-Program-NamaProgram').val(),
                TglPelaksanaan: $('#Add-Program-TglPelaksanaan').val(),
                Photo: {
                    Filename: file_attach_draft.name,
                    Base64: submitAttachmentDraft
                },
                Items: arrayParam,
                Lampiran: arrayParamLampiran
            }
            //console.log("Param ", param);

            RequestData('POST', `/v1/Program/add/${true}`, '#md-Program-add', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-Program-add').modal('hide');
                    getListProgramInternal();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}