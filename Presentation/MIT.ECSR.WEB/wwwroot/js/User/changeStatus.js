function deactivateUserDialog(el) {
    var data = $(el).data('detail');
    console.log("id :", data.Id);
    ConfirmMessage('Apakah Anda Yakin Menonaktifkan User Ini?', isConfirm => {
        if (isConfirm) {
            var element = {
                tbody: '#User-tbody',
                tcontainer: '#User-table',
            };
            RequestData('PUT', `/v1/User/active/${data.Id}/false`, element.tcontainer, element.tbody, null, function (data) {
                if (data.Succeeded) {
                    ShowNotif("User Berhasil Dinonaktifkan ...", "success");
                    getListUser();
                } else
                    ShowNotif(`${data.Message} : ${data.Description}`, "error");
            });
        }
    });
}

function activateUserDialog(el) {
    var data = $(el).data('detail');
    console.log("id :", data.Id);
    ConfirmMessage('Apakah Anda Yakin Mengaktifkan User Ini?', isConfirm => {
        if (isConfirm) {
            var element = {
                tbody: '#User-tbody',
                tcontainer: '#User-table',
            };
            RequestData('PUT', `/v1/User/active/${data.Id}/true`, element.tcontainer, element.tbody, null, function (data) {
                if (data.Succeeded) {
                    ShowNotif("User Berhasil Diaktifkan ...", "success");
                    getListUser();
                } else
                    ShowNotif(`${data.Message} : ${data.Description}`, "error");
            });
        }
    });
}