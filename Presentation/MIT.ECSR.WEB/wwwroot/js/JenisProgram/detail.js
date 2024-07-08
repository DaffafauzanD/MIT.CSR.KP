
function detailJenisProgramDialog(el) {
    console.log('detail');
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#md-JenisProgram-Detail').modal('show');
    $('#Detail-JenisProgram-Nama').val(data.Name);
    $('#Detail-JenisProgram-Status').prop('checked', data.Active);

}