function detailUserDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');

    if (data.Role.Id == 1 || data.Role.Id == 5) {
        $('#md-UserInternal-Detail').modal('show');

        $('#Detail-UserInternal-Fullname').val(data.Fullname);
        $('#Detail-UserInternal-Username').val(data.Username);
        $('#Detail-UserInternal-Mail').val(data.Mail);
        $('#Detail-UserInternal-PhoneNumber').val(data.PhoneNumber);
        $('#Detail-UserInternal-Role').val(data.Role.Nama);

    } else if (data.Role.Id == 2) {
        $('#md-UserExternal-Detail').modal('show');

        $('#Detail-UserExternal-Fullname').val(data.Fullname);
        $('#Detail-UserExternal-Username').val(data.Username);
        $('#Detail-UserExternal-Mail').val(data.Mail);
        $('#Detail-UserExternal-PhoneNumber').val(data.PhoneNumber);
        $('#Detail-UserExternal-Role').val(data.Role.Nama);
        $('#Detail-UserExternal-NamaPerusahaan').val(data.Perusahaan.NamaPerusahaan);
        $('#Detail-UserExternal-BidangUsaha').val(data.Perusahaan.BidangUsaha);
        $('#Detail-UserExternal-Alamat').val(data.Perusahaan.Alamat);

    }
    

}