
function detailSatuanUnitDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#md-SatuanUnit-Detail').modal('show');
    $('#Detail-SatuanUnit-Kode').val(data.Kode);
    $('#Detail-SatuanUnit-Nama').val(data.Name);
    $('#Detail-SatuanUnit-Status').prop('checked', data.Active);

}