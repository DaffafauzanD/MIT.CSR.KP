$(document).ready(function () {
    getDetailNotificationCount();
});

/*Table Penawaran*/
function getDetailNotificationCount() {
    RequestData('POST', `/v1/Notification/list`, null, null, JSON.stringify({
        "filter": [
            {
                "field": "",
                "search": ""
            }
        ],
        "sort": {
            "field": "",
            "type": 2
        },
        "start": 0,
        "length": 0
    }), function (result) {
        if (result.Succeeded) {
            console.log("Notification Sukses");
            result.List.forEach(function (item) {
                $('#detailnotification-list').append(` <a href="#" onclick="Redirection('${item.Id}','${item.Navigation}')">
                   <div class="col-12">
                        <div class="card bg-white rounded-lg m-0">
                          <div class="row justify-content-end text-end px-2">
                            <div class="col-2">
                                <time class="media-meta text-muted" datetime="${DateToStringFormat(item.CreateDate)}">${DateToStringFormat(item.CreateDate)}</time>
                            </div>
                         </div>
                         <div class="row p-2">
                        <h6 class="media-heading font-height-bold">${item.Subject}</h6>
                        <small>
                            <a class="notification-text font-small-3 text-muted">${item.Description} </a>
                        </small>
                         </div>
                    </div>
                 </div>
                
            </a>`);
            });
        }
        else {
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
    });
}

function Redirection(Id, url) {
    RequestData('PUT', `/v1/Notification/open/` + Id, null, null, null, function (result) {
        if (result.Succeeded) {
            window.open(url);
        }
        else {
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
    });
}
