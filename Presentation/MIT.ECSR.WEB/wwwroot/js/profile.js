$(document).ready(function () {
    getDetailUser();
});

$('#changePsswdBtn').on('click', function () {
    showModalChangePassword();
});

function getDetailUser() {
    var userId = $('#userId').val();
    RequestData('GET', `/v1/User/get/${userId}`, '.content-body', null, null, function (result) {
        if (result.Succeeded) {
            console.log('Data profile', result);
            if (result.Data.Role.Id == 1 || result.Data.Role.Id == 5) {
                //$('#profile-fullname').val(result.Data.Fullname);
                $('#profile-mail').val(result.Data.Mail);
                $('#profile-phoneNumber').val(result.Data.PhoneNumber);
            } else if (result.Data.Role.Id == 2) {
                $('#showProfileExternal').removeAttr('hidden', true);

                //$('#profile-fullname').val(result.Data.Fullname);
                $('#profile-mail').val(result.Data.Mail);
                $('#profile-phoneNumber').val(result.Data.PhoneNumber);
                $('#profile-namaPerusahaan').val(result.Data.Perusahaan.NamaPerusahaan);
                $('#profile-alamatPerusahaan').val(result.Data.Perusahaan.Alamat);
                $('#profile-bidangUsaha').val(result.Data.Perusahaan.BidangUsaha);
            }   
        }
        else {
            ShowNotif(`${result.Message} : ${result.Description}`, "error");
        }
    });
}

function editInfoProfile() {
    ConfirmMessage('Apakah Anda Yakin Akan Mengubah Data Ini?', isConfirm => {
        if (isConfirm) {
            var param = {
                Id: $('#userId').val(),
                Mail: $('#profile-mail').val(),
                PhoneNumber: $('#profile-phoneNumber').val(),
                Perusahaan: {
                    Alamat: $('#profile-alamatPerusahaan').val(),
                    BidangUsaha: $('#profile-bidangUsaha').val(),
                    NamaPerusahaan: $('#profile-namaPerusahaan').val()
                }
            }
            RequestData('PUT', `/v1/User/edit_info`, '.content-body', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    swal("Save Data", "Data Berhasil Dirubah", "success");
                }
                else if (data_obj.Code == 401) { //unathorized
                    swal("Failed Save", data_obj.Message, "warning");
                } else
                    swal("Failed Save", `${data_obj.Message} : ${data_obj.Description}`, "warning");
            });
        }
    });
}

function showModalChangePassword() {
    var userId = $('#userId').val();
    RequestData('GET', `/v1/User/get/${userId}`, '.content-body', null, null, function (result) {
        if (result.Succeeded) {
            $('.clear').val('');
            $('#md-Profile-ChangePassword').modal('show');

            $('#Edit-Profile-Username').val(result.Data.Username);

            $('#md-Profile-ChangePassword').data('id', result.Data.Id);
            $('#ChangePassword-Edit_btn').unbind();
            $('#ChangePassword-Edit_btn').on('click', function () {
                editPasswordSave();
            });
        }
        else {
            ShowNotif(`${result.Message} : ${result.Description}`, "error");
        }
    });
}

function editPasswordSave() {
    ConfirmMessage('Apakah Anda Yakin?', isConfirm => {
        if (isConfirm) {
            var param = {
                Username: $('#Edit-Profile-Username').val(),
                Password: $('#Edit-Profile-OldPassword').val(),
                NewPassword: $('#Edit-Profile-NewPassword').val()
            }
            console.log('param chg psswd', param);
            RequestData('POST', `/v1/User/change_password`, '#md-Profile-ChangePassword .modal-content', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Dirubah", "success");
                    $('#md-Profile-ChangePassword').modal('hide');
                    location.reload();
                }
                else if (data_obj.Code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}