var idlampiranpenawaran;
$(document).ready(function () {
    var getUrl = (window.location).href;
    var id = getUrl.substring(getUrl.lastIndexOf('=') + 1);

    detailPenawaranExternalDialog();
});

function detailPenawaranExternalDialog() {
    var getUrl = (window.location).href;
    var id = getUrl.substring(getUrl.lastIndexOf('=') + 1);

    var param = {
        id: id
    }
    console.log("param", param);

    RequestData('GET', `/v1/Penawaran/get_by_id/${id}`, '#DetailPengajuan', null, null, function (data) {
        if (data.Succeeded) {
            console.log("Data", data.Data);
            idlampiranpenawaran = data.Data.Lampiran[0].Id;
            $('#DetailInternal-PengajuanUlang-NamaProgram').val(data.Data.Program.NamaProgram);
            $('#DetailInternal-PengajuanUlang-JenisProgram').val(data.Data.Program.JenisProgram.Nama);
            $('#DetailInternal-PengajuanUlang-Tgl').val(DateToStringFormat(data.Data.Program.TglPelaksanaan));
            $('#DetailInternal-PengajuanUlang-tempat').val(data.Data.Program.Kecamatan + "," + data.Data.Program.Kelurahan);
            $('#DetailInternal-PengajuanUlang-Deskripsi').val(data.Data.Program.Deskripsi);
            $('#DetailInternal-PengajuanUlang-Lampiran').val(data.Data.Program.Lampiran);
            $('#DetailInternal-PengajuanUlang-Comment').val(data.Data.Deskripsi);

            $('#DetailInternal-Pengajuan-Perusahaan').val(data.Data.Perusahaan.NamaPerusahaan);
            $('#DetailInternal-Pengajuan-BidangUsaha').val(data.Data.Perusahaan.BidangUsaha);
            $('#DetailInternal-Pengajuan-Alamat').val(data.Data.Perusahaan.Alamat);
            $('#DetailInternal-PengajuanUlang-File').text(data.Data.Lampiran[0].Filename)
            $('#DetailInternal-PengajuanUlang-File').click(function () {
                GetFileDetailPengajuanPenawaran(idlampiranpenawaran);
            })

            data.Data.Program.Items.forEach(function (list) {
                console.log(list.SatuanJenis.Nama);
                $('#tbody-DetailPengajunInternal').append(`
                     <tr>
				        <td class="text-center">${list.Nama}</td>
				        <td class="text-center">${list.Jumlah}</td>								
                        <td class="text-center">${list.SatuanJenis.Nama}</td>
                     </tr>
        `       )
            });   
            $('#DetailInternal-PengajuanUlang-Lampiran').html("");
            data.Data.Program.Lampiran.forEach(function (Lampiran) {
                $('#DetailInternal-PengajuanUlang-Lampiran').append(`
                    <div class="card col-4 mx-1">
                        <h4 onclick="GetFileProgramInternal('${Lampiran.Id}')" class="card-title text-truncate">${Lampiran.Filename} <i class="fa fa-download"></i></h4>
                    </div>
                `)
            })

        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
    
}
function detailPengajuanDialog(el) {
    var dataDetail = $(el).data('detail');
    openMenu(`/Penawaran/External`);
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
function GetFileProgramInternal(Id) {
    console.log(Id);
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
