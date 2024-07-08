$(document).ready(function () {
    SetDatePicker('#AddProgram-StartTglPelaksanaan', "DD MMMM, YYYY", null, null, true, new Date());
    SetDatePicker('#AddProgram-EndTglPelaksanaan', "DD MMMM, YYYY", null, null, true, new Date(new Date().setMonth(new Date().getMonth() + 1)));
    SetDatePicker('#AddProgram-BatasWaktu', "DD MMMM, YYYY", null, null, true, new Date(new Date().setMonth(new Date().getMonth() + 4)));
    SetPicture('#photo_file', '#photo_img', null);
    ListJenisProgram("#AddProgram-JenisProgram-Container", "#AddProgram-JenisProgram", null, true);
    ListDati('#AddProgram-LokasiKegiatan-Container', '#AddProgram-LokasiKegiatan', null, 'namaDati4');
    $('#AddProgram-JenisProgram').on('change', function () {
        ListKegiatanReferensi('#AddProgram-Kegiatan-Container', '#AddProgram-Kegiatan', null, false, [{
            "field": "idJenisProgram",
            "search": `${$(this).val()}`
        }]);
    });
});

function SaveProgram() {
    if (!FormValidate('Section-AddProgram')) {
        return;
    }
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (!isConfirm) {
            return;
        }
        var photo = null;
        if ($('#photo_file')[0].files[0] != undefined) {
            let _base64;
            await FileToBase64($('#photo_file')[0].files[0])
                .then(dataBase64 => _base64 = dataBase64)
                .catch(error => {
                    AlertMessage(error);
                    return;
                });
            photo = {
                filename: $('#photo_file')[0].files[0].name,
                base64: _base64
            }
        }

        var param = {
            deskripsi: $('#AddProgram-Deskripsi').val(),
            lokasi: $('#AddProgram-LokasiKegiatan').val(),
            idJenisProgram: $('#AddProgram-JenisProgram').val(),
            namaProgram: $('#AddProgram-Kegiatan').val(),
            startPelaksanaan: StringToDateFormat($('#AddProgram-StartTglPelaksanaan').val(), "DD MMMM, YYYY"),
            endPelaksanaan: StringToDateFormat($('#AddProgram-EndTglPelaksanaan').val(), "DD MMMM, YYYY"),
            batasWaktuProgram: StringToDateFormat($('#AddProgram-BatasWaktu').val(), "DD MMMM, YYYY"),
            photo: photo
        }

        RequestData('POST', `/v1/Program/add`, '#Section-AddProgram', null, JSON.stringify(param), function (data_obj) {
            if (data_obj.succeeded) {
                AlertMessage("Data Berhasil Disimpan",null, "success");
                location.href = window.location.origin + `/Program/Edit/${data_obj.data}`;
            }
            else
                AlertMessage(data_obj.message);
        });
    });
}