var idlampiranpenawaran; 
var idlampiranprogram;
function detailPenawaranExternalDialog(el) {
    console.log("hai");
    var data = $(el).data('detail');
    $('.clear').val('');
    $('#md-DetailPengajuanUlang').modal('show');

    idlampiranpenawaran = data.Lampiran[0].Id;
    idlampiranprogram = data.Program.Lampiran[0].Id;


    console.log("Data id", data);
    $('#Detail-PengajuanUlang-NamaProgram').val(data.Program.NamaProgram);
    $('#Detail-PengajuanUlang-JenisProgram').val(data.Program.JenisProgram.Nama);
    $('#Detail-PengajuanUlang-Tgl').val(DateToStringFormat(data.Program.TglPelaksanaan));
    $('#Detail-PengajuanUlang-tempat').val(data.Program.Kecamatan + "," + data.Program.Kelurahan);
    $('#Detail-PengajuanUlang-Deskripsi').val(data.Program.Deskripsi);
    $('#Detail-PengajuanUlang-Lampiran').val(data.Program.Lampiran);
    $('#Detail-PengajuanUlang-Comment').val(data.Deskripsi);
    data.Program.Items.forEach(function (list) {
        $('#tbody-DetailPengajuanUlang').append(`
            <tr>
				<td class="text-center">${list.Nama}</td>
				<td class="text-center">${list.Jumlah}</td>								
                <td class="text-center">${list.SatuanJenis.Nama}</td>
           </tr>
        `)
    })

    if (data.Program.Lampiran.length > 0) {
        $('#Detail-LampiranProgram').text(data.Program.Lampiran[0].Filename)
    }
    $('#Detail-LampiranProgram').click(function () {
        GetFileDetailProgramPengajuan(idlampiranprogram);
    })


    $('#Detail-LampiranPenawaran').text(data.Lampiran[0].Filename)
    $('#Detail-LampiranPenawaran').click(function () {
        GetFileDetailPengajuanPenawaran(idlampiranpenawaran);
    })

}
function GetFileDetailProgramPengajuan(Id) {
    if (Id == "") {
        AlertMessage("Lampiran Not Found");
    } else {
        RequestData('GET', `/v1/Media/download/${Id}`, '', null, null, function (data_obj) {
            if (data_obj.Succeeded) {
                Base64ToFile(data_obj.Data.Filename, "application/octet-stream", data_obj.Data.Base64);
                /*            $('#Detail-LampiranProgram').text(data_obj.Data.Filename, 'data:application/pdf;base64,' + data_obj.Data.Base64);*/
            }
            else if (data_obj.code == 401) { //unathorized
                AlertMessage(data_obj.message);
            } else
                ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
        });
    }
    
}
function GetFileDetailPengajuanPenawaran(Id) {
    if (Id == "") {
        AlertMessage("Lampiran Not Found");
    }
    else {
        console.log();
        RequestData('GET', `/v1/Media/download/${Id}`, '', null, null, function (data_obj) {
            if (data_obj.Succeeded) {
                Base64ToFile(data_obj.Data.Filename, "application/octet-stream", data_obj.Data.Base64);
                /*            $('#Detail-LampiranPenawaran').attr(data_obj.Data.Filename, 'data:application/pdf;base64,' + data_obj.Data.Base64);*/
            }
            else if (data_obj.code == 401) { //unathorized
                AlertMessage(data_obj.message);
            } else
                ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
        });
    }
    
}