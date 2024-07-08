$(document).ready(function () {
    ListRole('#AddUser-Role-Container', '#AddUser-Role', null, true, null, false);
    $('#save_btn').on('click', function () {
        SaveUser();
    });
    $('#div-forum').hide();
    $('#AddUser-Role').on('change', function () {
        var role = $("#AddUser-Role option:selected").text().toLowerCase();
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
                Username: $('#AddUser-Username').val(),
                IdRole: $('#AddUser-Role').val(),
                Fullname: $('#AddUser-Fullname').val(),
                Mail: $('#AddUser-Mail').val(),
                PhoneNumber: $('#AddUser-PhoneNumber').val(),
                Perusahaan: {
                    Alamat: $('#AddUser-AlamatPerusahaan').val(),
                    BidangUsaha: $('#AddUser-BidangUsaha').val(),
                    NamaPerusahaan: $('#AddUser-NamaPerusahaan').val(),
                    Nib: $('#AddUser-Nib').val(),
                    JenisPerseroan: $('#AddUser-JenisPerusahaan').val(),
                    Npwp: $('#AddUser-Npwp').val(),
                },
                Password: $('#AddUser-Password').val()
            }
            RequestData('POST', `/v1/User/register`, '#ExternalIndex', null, JSON.stringify(param), function (data_obj) {
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