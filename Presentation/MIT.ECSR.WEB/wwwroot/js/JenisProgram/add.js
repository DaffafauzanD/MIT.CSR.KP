$(document).ready(function () {
    $('#btnJenisProgram_Add').on('click', function () {
        $('.clear').val('');
        $('#md-JenisProgram-Add').modal('show');
        $('#JenisProgram-create_btn').unbind();
        $('#JenisProgram-create_btn').on('click', function () {
            addJenisProgramSave();
        });
    });
});

function addJenisProgramSave() {
    ConfirmMessage('Apakah Anda Yakin?', function (isConfirm) {
        if (isConfirm) {
            var param = {
                Name: $('#Add-JenisProgram-Nama').val(),
                Active: $('#Add-JenisProgram-Status').is(":checked")
            }
            RequestData('POST', `/v1/JenisProgram/add`, '#md-JenisProgram-Add', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-JenisProgram-Add').modal('hide');
                    getListJenisProgram();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}