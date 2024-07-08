$(document).ready(function () {
    getNotificationCount();
});

/*Table Penawaran*/
function getNotificationCount() {
    console.log("Notification");
    RequestData('POST', `/v1/Notification/list`, null, null, JSON.stringify({
        "filter": [
            {
                "field": "IsOpen",
                "search": "false"
            }
        ],
        "sort": {
            "field": "string",
            "type": 0
        },
        "start": 1,
        "length": 3
    }), function (result) {
        if (result.Succeeded) {
            $('#JumlahNotif').text(result.Count);
            console.log("RESURL", result.Count);
            result.List.forEach(function (item) {
                $('#notification-list').append(` <a href="#" onclick="Redirection('${item.Id}','${item.Navigation}')">
                <div class="media">
                    <div class="media-left align-self-center"><i class="ft-alert-circle text-warning mr-0"></i></div>
                    <div class="media-body">
                        <time class="media-meta text-muted" datetime="${item.CreateDate}">${item.CreateDate}</time>
                        <h6 class="media-heading font-height-bold">${item.Subject}</h6>
                        <small>
                            <a class="notification-text font-small-3 text-muted">${item.Description} </a>
                        </small>
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
