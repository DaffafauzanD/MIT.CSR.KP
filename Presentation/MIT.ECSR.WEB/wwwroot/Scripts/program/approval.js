function showApproval(el, callback)
{
    var data = $(el).data('detail');
    console.log(data)
    $('#md-Program_approval').modal('show');
    $('#Program_approval-Notes').val('');

    $('#Program_approval-save').unbind();
    $('#Program_approval-save').on('click', function () {
        approvePenawaran(data, $('#Program_Approval-Status').val(), $('#Program_approval-Notes').val(), callback);
    });
}

function approvePenawaran(id, status, notes, callback) {
    ConfirmMessage('Apakah Anda Yakin?', async isConfirm => {
        if (isConfirm) {
            var param = {
                id: id,
                isApprove: status == "true" ? true : false,
                notes: notes
            }

            RequestData('POST', `/v1/program/approve`, '#md-Program_approval', null, JSON.stringify(param), function (data_obj) {
                if (data_obj.succeeded) {
                    AlertMessage("Data Berhasil Disimpan", null, "success");
                    $('#md-Program_approval').modal('hide');

                    if (callback)
                        return callback();

                } else {
                    AlertMessage(data_obj.message);
                }
            });
        }
    });
}

function approvalCallback() {
    location.reload();
}