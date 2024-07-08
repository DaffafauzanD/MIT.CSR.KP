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
            $('#Detail-PengajuanUlang-NamaProgram').val(data.Data.Program.NamaProgram);
            $('#Detail-PengajuanUlang-JenisProgram').val(data.Data.Program.JenisProgram.Nama);
            $('#Detail-PengajuanUlang-Tgl').val(DateToStringFormat(data.Data.Program.TglPelaksanaan));
            $('#Detail-PengajuanUlang-tempat').val(data.Data.Program.Kecamatan + "," + data.Data.Program.Kelurahan);
            $('#Detail-PengajuanUlang-Deskripsi').val(data.Data.Program.Deskripsi);
            $('#Detail-PengajuanUlang-Lampiran').val(data.Data.Program.Lampiran);
            $('#Detail-PengajuanUlang-Comment').val(data.Data.Deskripsi);

            $('#Detail-Pengajuan-Status').text(data.Data.StatusDescription);
            $('#Detail-Pengajuan-Perusahaan').val(data.Data.Perusahaan.NamaPerusahaan);
            $('#Detail-Pengajuan-BidangUsaha').val(data.Data.Perusahaan.BidangUsaha);
            $('#Detail-Pengajuan-Alamat').val(data.Data.Perusahaan.Alamat);
            $('#Detail-PengajuanUlang-File').text(data.Data.Lampiran[0].Filename)
            $('#Detail-PengajuanUlang-File').click(function () {
                GetFileDetailPengajuanPenawaran(idlampiranpenawaran);
            })

            if (data.Data.Status == 1) {
                $('#Penawaran-Button').append(`
                   <button class="btn-danger btn-sm" type="button" onclick="deletePenawaranDetailDialog(this);">Delete</button>                   
                   <button class="btn-black btn-sm" type="button" onclick="detailPengajuanDialog(this);">Back</button>
                `)
            }
            if (data.Data.Status == 2) {
                $('#Penawaran-Button').append(`
                   <button class="btn-black btn-sm" type="button" onclick="detailPengajuanDialog(this);">Back</button>
                `)
            }
            if (data.Data.Status == 3) {
                $('#Penawaran-Button').append(`
                   <button class="btn-success btn-sm" type="button" onclick="editPengajuanUlangDetailSave(this);">Ajukan Kembali</button> 
                   <button class="btn-black btn-sm" type="button" onclick="detailPengajuanDialog(this);">Back</button>
        `       )
                $('#Add-LampiranPengajuanUlang').append(`
                    <div class="col-md-6">
                        <label>File</label>
                        <input type="file" class="form-control form-control-sm" id="Add-PengajuanUlang-File">
                    </div>
                 `)
                $('#Add-PengajuanUlang-Comment').removeAttr('hidden', true)
                $('#Detail-PengajuanUlang-Comment').hide();


            }
            
            $('#Detail-PengajuanUlang-Lampiran').html("");
            data.Data.Program.Lampiran.forEach(function (Lampiran) {
                $('#Detail-PengajuanUlang-Lampiran').append(`
                    <div class="card col-4 mx-1">
                        <h4 onclick="GetFileProgram('${Lampiran.Id}')" class="card-title text-truncate">${Lampiran.Filename} <i class="fa fa-download"></i></h4>
                    </div>
                `)
            })

            data.Data.Program.Items.forEach(function (list) {
                console.log(list.SatuanJenis.Nama);
                $('#tbody-DetailProgramUnit').append(`
                     <tr>
				        <td class="text-center">${list.Nama}</td>
				        <td class="text-center">${list.Jumlah}</td>								
                        <td class="text-center">${list.SatuanJenis.Nama}</td>
                     </tr>
        `       )
            });          

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
function GetFileProgram(Id) {
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


