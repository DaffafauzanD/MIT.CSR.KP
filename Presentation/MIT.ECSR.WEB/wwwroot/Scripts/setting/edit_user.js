$(document).ready(function () {
    ListRole('#EditUser-Role-Container', '#EditUser-Role', $('#EditUser-RoleId').val(), true, null, false);
    $('#save_btn').on('click', function () {
        SaveUser();
    });

    if ($('#EditUser-RoleName').val().toLowerCase().includes("forum"))
        $('#div-forum').show();
    else
        $('#div-forum').hide();

    $('#EditUser-Role').on('change', function () {
        var role = $("#EditUser-Role option:selected").text().toLowerCase();
        if (role.includes("forum")) {
            $('#div-forum').show();
        } else {
            $('#div-forum').hide();
        }
    });
});

function SaveUser() {
    ConfirmMessage('Apakah Anda Yakin?', function (isConfirm) {
        if (isConfirm) {
            var param = {
                Username: $('#EditUser-Username').val(),
                IdRole: $('#EditUser-Role').val(),
                Fullname: $('#EditUser-Fullname').val(),
                Mail: $('#EditUser-Mail').val(),
                PhoneNumber: $('#EditUser-PhoneNumber').val(),
                Perusahaan: {
                    Alamat: $('#EditUser-AlamatPerusahaan').val(),
                    BidangUsaha: $('#EditUser-BidangUsaha').val(),
                    NamaPerusahaan: $('#EditUser-NamaPerusahaan').val(),
                    Nib: $('#EditUser-Nib').val(),
                    JenisPerseroan: $('#EditUser-JenisPerusahaan').val(),
                    Npwp: $('#EditUser-Npwp').val(),
                },
                Password: $('#EditUser-Password').val()
            }
            RequestData('PUT', `/v1/User/edit/${$('#EditUser-Id').val()}`, '#ExternalIndex', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    openMenu('/Setting/User');
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });
        }
    });
}