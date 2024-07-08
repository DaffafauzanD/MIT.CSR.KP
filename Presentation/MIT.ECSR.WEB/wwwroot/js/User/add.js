$(document).ready(function () {
    getListRoleUser();
});

$('#UserInternal-create_btn').on('click', function () {
    addUserInternalSave();
});

$('#UserExternal-create_btn').on('click', function () {
    addUserExternalSave();
});

function addUserInternalSave() {
    ConfirmMessage('Apakah Anda Yakin?', function (isConfirm) {
        if (isConfirm) {
            var param = {
                Username: $('#AddInternal-User-Username').val(),
                IdRole: $('#AddInternal-User-Role').val(),
                Fullname: $('#AddInternal-User-Fullname').val(),
                Mail: $('#AddInternal-User-Mail').val(),
                PhoneNumber: $('#AddInternal-User-PhoneNumber').val(),
                Password: $('#AddInternal-User-Password').val()
            }
            RequestData('POST', `/v1/User/register`, '#InternalIndex', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    openMenu('/User/Index');
                    ShowNotif("Data Berhasil Disimpan", "success");
                    getListUser();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}

function addUserExternalSave() {
    ConfirmMessage('Apakah Anda Yakin?', function (isConfirm) {
        if (isConfirm) {
            var param = {
                Username: $('#AddExternal-User-Username').val(),
                IdRole: $('#AddExternal-User-Role').val(),
                Fullname: $('#AddExternal-User-Fullname').val(),
                Mail: $('#AddExternal-User-Mail').val(),
                PhoneNumber: $('#AddExternal-User-PhoneNumber').val(),
                Perusahaan: {
                    Alamat: $('#AddExternal-User-AlamatPerusahaan').val(),
                    BidangUsaha: $('#AddExternal-User-BidangUsaha').val(),
                    NamaPerusahaan: $('#AddExternal-User-NamaPerusahaan').val()
                },
                Password: $('#AddExternal-User-Password').val()
            }
            RequestData('POST', `/v1/User/register`, '#ExternalIndex', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    openMenu('/User/Index');
                    ShowNotif("Data Berhasil Disimpan", "success");
                    getListUser();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}