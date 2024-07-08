$(document).ready(function () {
    SetPicture('#photo_file', '#photo_img', null);
    getListLampiran();
});

function getListLampiran() {
    var id = $('#DetailProgram-Id').val();
    RequestData('GET', `/v1/ProgramMedia/list/${id}`, '#DetailProgram-Lampiran', null, null, function (data) {
        if (data.succeeded) {
            $('#DetailProgram-ListLampiran').html('');
            if (data.list.length > 0) {
                data.list.forEach(function (item) {
                    $('#DetailProgram-ListLampiran').append(`
                           <div class="col-md-12 my-1">
                               <div class="bs-callout-blue-grey callout-border-left callout-bordered callout-transparent">
                                   <div class="card-header">
                                       <div class="row">
                                           <div class="d-flex align-self-center col-md-6 justify-content-start">
                                               <a href="${item.url.original}" target="_blank">${item.fileName}</a>
                                           </div>
                                       
                                       </div>
                                   </div>
                               </div>
                           </div>
                       `);
                });
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }

    })
}
