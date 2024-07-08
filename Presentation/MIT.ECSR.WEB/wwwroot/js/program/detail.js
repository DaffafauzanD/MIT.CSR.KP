function detailProgramDialog(el) {
    var data = $(el).data('detail');

    $('.clear').val('');
    $('#tbody-itemProgram').html('');
    $('#tbody-lampiranProgram').html('');
    $('#md-Program-detail').modal('show');

    $('#Detail-Program-NamaProgram').val(data.NamaProgram);
    $('#Detail-Program-JenisProgram').val(data.JenisProgram.Nama);
    $('#Detail-Program-TglPelaksanaan').val(DateToStringFormat(data.TglPelaksanaan));
    $('#Detail-Program-Tempat').val(data.Kecamatan + ", " + data.Kelurahan);
    $('#Detail-Program-Deskripsi').val(data.Deskripsi);
    data.Items.forEach(function (list) {
        $('#tbody-itemProgram').append(`
            <tr>
				<td class="text-center">${list.Nama}</td>
				<td class="text-center">${list.Jumlah}</td>								
                <td class="text-center">${list.SatuanJenis.Nama}</td>
           </tr>
        `);
    });
    data.Lampiran.forEach(function (item) {
        $('#tbody-lampiranProgram').append(`
            <tr>
				<td class="text-center">${item.Filename}</td>
           </tr>
        `)
    });
    $('#Detail-Program-Photo').val(data.PhotoUrl);
    $('#Detail-Program-Photo').click(function () {
        window.location.href = data.PhotoUrl;
    });
}