function updateProgressDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#modalUpdateProgress-monitoring').modal('show');

    $('#Detail-Monitoring-NamaProgram').val(data.Penawaran.Program.NamaProgram);
    $('#Detail-Monitoring-JenisProgram').val(data.Penawaran.Program.JenisProgram.Nama);
    $('#Detail-Monitoring-WaktuPelaksanaan').val(DateToStringFormat(data.Penawaran.Program.TglPelaksanaan));
    $('#Detail-Monitoring-Tempat').val(data.Penawaran.Program.Kecamatan + "," + data.Penawaran.Program.Kelurahan);
    $('#Detail-Monitoring-Deskripsi').val(data.Penawaran.Program.Deskripsi);

}
