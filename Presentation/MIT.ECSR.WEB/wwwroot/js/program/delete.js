function deleteDraftProgramDialog(el) {
    var data = $(el).data('detail');
    ConfirmMessage('Apakah Anda Yakin Draft Akan Dihapus?', isConfirm => {
        if (isConfirm) {
            var element = {
                tbody: '#ProgramInternal-tbody',
                tcontainer: '#ProgramInternal-table',
            };
            RequestData('DELETE', `/v1/Program/delete/${data.Id}`, element.tcontainer, element.tbody, null, function (data) {
                if (data.Succeeded) {
                    ShowNotif("Data Deleted Successfully ...", "success");
                    getListProgramInternal();
                } else
                    ShowNotif(`${data.Message} : ${data.Description}`, "error");
            });
        }
    });
}