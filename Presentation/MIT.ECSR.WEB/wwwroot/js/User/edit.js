$(document).ready(function () {

	var getUrl = (window.location).href;
	var id = getUrl.substring(getUrl.lastIndexOf('=') + 1);
	//$('#EditInternal-User-Id, #EditExternal-User-Id').val(id);

	editUserDialog();

});

function editUserDialog() {

	var getUrl = (window.location).href;
	var id = getUrl.substring(getUrl.lastIndexOf('=') + 1);

	//var param = {
	//	id
	//}

	//console.log('param', param);
	RequestData('GET', `/v1/User/get/${id}`, 'EditUserPage', null, null, function (data_item) {
		if (data_item.Succeeded) {
			if (data_item.Data.Role.Id == 1 || data_item.Data.Role.Id == 5) {

				$('#EditUserInternalPage').removeAttr('hidden', true);

				$('#EditInternal-User-Id').val(data_item.Data.Id);
				$('#EditInternal-User-Username').val(data_item.Data.Username),
				$('#EditInternal-User-Role').val(data_item.Data.Role.Id),
				$('#EditInternal-User-Fullname').val(data_item.Data.Fullname),
				$('#EditInternal-User-Mail').val(data_item.Data.Mail),
				$('#EditInternal-User-PhoneNumber').val(data_item.Data.PhoneNumber)

			} else if (data_item.Data.Role.Id == 2) {

				$('#EditUserExternalPage').removeAttr('hidden', true);

				$('#EditExternal-User-Id').val(data_item.Data.Id);
				$('#EditExternal-User-Username').val(data_item.Data.Username),
				$('#EditExternal-User-Role').val(data_item.Data.Role.Id),
				$('#EditExternal-User-Fullname').val(data_item.Data.Fullname),
				$('#EditExternal-User-Mail').val(data_item.Data.Mail),
				$('#EditExternal-User-PhoneNumber').val(data_item.Data.PhoneNumber),
				$('#EditExternal-User-AlamatPerusahaan').val(data_item.Data.Perusahaan.Alamat),
				$('#EditExternal-User-BidangUsaha').val(data_item.Data.Perusahaan.BidangUsaha),
				$('#EditExternal-User-NamaPerusahaan').val(data_item.Data.Perusahaan.NamaPerusahaan)

			}

		} else if (data_item.code == 401) {
			AlertMessage(data_item.message);
		} else
			ShowNotif(`${data_item.message} : ${data_item.description}`, "error");
	});

	$('#UserInternal-edit_btn').unbind();
	$('#UserInternal-edit_btn').on('click', function () {
		editUserInternalSave();
	});

	$('#UserExternal-edit_btn').unbind();
	$('#UserExternal-edit_btn').on('click', function () {
		editUserExternalSave();
	});

}

function editUserInternalSave() {
	ConfirmMessage('Apakah Anda Yakin Akan Mengubah Data Ini?', isConfirm => {
		if (isConfirm) {
			var getUrl = (window.location).href;
			var id = getUrl.substring(getUrl.lastIndexOf('=') + 1);

			var param = {
				Username: $('#EditInternal-User-Username').val(),
				IdRole: $('#EditInternal-User-Role').val(),
				Fullname: $('#EditInternal-User-Fullname').val(),
				Mail: $('#EditInternal-User-Mail').val(),
				PhoneNumber: $('#EditInternal-User-PhoneNumber').val()
			}
			RequestData('PUT', `/v1/User/edit/${id}`, 'EditUserInternalPage', null, JSON.stringify(param), function (data_obj) {
				if (data_obj.Succeeded) {
					openMenu('/User/Index');
					getListUser();
					ShowNotif("Data Berhasil Dirubah", "success");
				}
				else if (data_obj.code == 401) { //unathorized
					AlertMessage(data_obj.Message);
				} else
					ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
			});
		}
	});
}

function editUserExternalSave() {
	ConfirmMessage('Apakah Anda Yakin Akan Mengubah Data Ini?', isConfirm => {
		if (isConfirm) {
			var getUrl = (window.location).href;
			var id = getUrl.substring(getUrl.lastIndexOf('=') + 1);

			var param = {
				Username: $('#EditExternal-User-Username').val(),
				IdRole: $('#EditExternal-User-Role').val(),
				Fullname: $('#EditExternal-User-Fullname').val(),
				Mail: $('#EditExternal-User-Mail').val(),
				PhoneNumber: $('#EditExternal-User-PhoneNumber').val(),
				Perusahaan: {
					Alamat: $('#EditExternal-User-AlamatPerusahaan').val(),
					BidangUsaha: $('#EditExternal-User-BidangUsaha').val(),
					NamaPerusahaan: $('#EditExternal-User-NamaPerusahaan').val()
				}
			}

			RequestData('PUT', `/v1/User/edit/${id}`, 'EditUserExternalPage', null, JSON.stringify(param), function (data_obj) {
				if (data_obj.Succeeded) {
					openMenu('/User/Index');
					getListUser();
					ShowNotif("Data Berhasil Dirubah", "success");
				}
				else if (data_obj.code == 401) { //unathorized
					AlertMessage(data_obj.Message);
				} else
					ShowNotif(`${data_obj.Message} : ${data_obj.Description}`, "error");
			});
		}
	});
}