$(document).ready(function () {
    $('#btnSatuanUnit_Add').on('click', function () {
        $('.clear').val('');
        $('#md-SatuanUnit-Add').modal('show');
        $('#SatuanUnit-create_btn').unbind();
        $('#SatuanUnit-create_btn').on('click', function () {
            addSatuanSave();
        });
    });
});

function addSatuanSave() {
    ConfirmMessage('Apakah Anda Yakin?', function (isConfirm) {
        if (isConfirm) {
            var param = {
                Active: $('#Add-SatuanUnit-Status').is(":checked"),
                Kode: $('#Add-SatuanUnit-kode').val(),
                Name: $('#Add-SatuanUnit-nama').val()
            }
            RequestData('POST', `/v1/SatuanJenis/add`, '#md-SatuanUnit-Add', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-SatuanUnit-Add').modal('hide');
                    getListSatuanUnit();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}