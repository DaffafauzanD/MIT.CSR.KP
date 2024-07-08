function editDraftProgramDialog(el) {
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#tbody-editItemProgram').html('');
    $('#tbody-editLampiranProgram').html('');
    $('#md-edit-draftProgram').modal('show');

    $('#Edit-Program-NamaProgram').val(data.NamaProgram);
    $('#Edit-Program-JenisProgram').val(data.JenisProgram.Id);
    $('#Edit-Program-TglPelaksanaan').val(DateToString(new Date(data.TglPelaksanaan), "YYYY-MM-DD"));
    $('#Edit-Program-Deskripsi').val(data.Deskripsi);
    console.log('dati', data);
    $('#Edit-Program-Tempat').val(data.Dati.Id);
    data.Items.forEach(function (list) {
        $('#tbody-editItemProgram').append(`
            <tr>
				<td class="text-center">${list.Nama}</td>
				<td class="text-center">${list.Jumlah}</td>								
                <td class="text-center">${list.SatuanJenis.Nama}</td>
           </tr>
        `)
    });
    data.Lampiran.forEach(function (item) {
        $('#tbody-editLampiranProgram').append(`
            <tr>
				<td class="text-center">${item.Filename}</td>
           </tr>
        `)
    });
    $('#Edit-Program-Photo').val(data.PhotoUrl);
    $('#Edit-Program-Photo').click(function () {
        window.location.href = data.PhotoUrl;
    });
    $('#md-edit-draftProgram').data('id', data.Id);
    $('#draftProgram-edit_btn').unbind();
    $('#draftProgram-edit_btn').on('click', function () {
        editDataProgramSave();
    });
}

function editDataProgramSave() {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var submitAttachment = "";
            var file_attach = $('#Edit-Program-newPhoto')[0].files[0];
            if (file_attach != undefined && file_attach != null) {
                console.log("file_attach", file_attach);
                await FileToBase64(file_attach)
                    .then(dataBase64 => submitAttachment = dataBase64)
                    .catch(error => {
                        AlertMessage(error);
                        return;
                    });
            }

            var param = {
                Deskripsi: $('#Edit-Program-Deskripsi').val(),
                IdDati: $('#Edit-Program-Tempat').val(),
                IdJenisProgram: $('#Edit-Program-JenisProgram').val(),
                NamaProgram: $('#Edit-Program-NamaProgram').val(),
                TglPelaksanaan: $('#Edit-Program-TglPelaksanaan').val(),
                Photo: {
                    Filename: file_attach.name,
                    Base64: submitAttachment
                }
            }
            RequestData('PUT', `/v1/Program/edit/${$('#md-edit-draftProgram').data('id')}/${false}`, '#md-edit-draftProgram', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.Succeeded) {
                    ShowNotif("Data Berhasil Disimpan", "success");
                    $('#md-edit-draftProgram').modal('hide');
                    getListProgramInternal();
                }
                else if (data_obj.code == 401) { //unathorized
                    AlertMessage(data_obj.Message);
                } else
                    ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
            });

        }
    });
}