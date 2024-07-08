$(document).ready(function () {
    $('#UserExternal-create_btn').on('click', function () {
        UpdateProfile();
    });
    ListNotifications();
});

function Login() {
    var param = {
        username: $('#login_username').val(),
        password: $('#login_password').val()
    }
    RequestData('POST', `/v1/User/login`, '#modal_login', null, JSON.stringify(param), function (data_obj) {
        if (data_obj.succeeded) {
            console.log(data_obj);
            document.cookie = `MIT.ECSR.token=${data_obj.data.rawToken}; expires=${new Date(data_obj.data.expiredAt).toGMTString()}`;
            location.reload();
        }
        else if (data_obj.code == 401) { //unathorized
            AlertMessage(data_obj.message);
        } else
            ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
    });
}

function Logout() {
    ConfirmMessage('Apakah Anda Yakin akan keluar?', isConfirm => {
        if (isConfirm) {
            document.cookie = `MIT.ECSR.token=; Path=/; Expires=${new Date()};`;
            window.location.href = window.location.origin;
        }
    });
}

function Profil() {
   
    $('.clear').val('');
    $('#modal_profil').modal('show');
}

function UpdateProfile() {
    var param = {
        fullname: $('#Profile-Fullname').val(),
        mail: $('#Profile-Email').val(),
        phonenumber: $('#Profile-NoTelpon').val(),
        username: $('#Profile-Username').val(),
        password: $('#Profile-Password').val(),
        perusahaan: {
            namaperusahaan: $('#Profile-NamaPerusahaan').val(),
            nib: $('#Profile-Nib').val(),
            jenisperseroan: $('#Profile-Jenis').val(),
            npwp: $('#Profile-Npwp').val(),
            bidangusaha: $('#Profile-Bidang').val(),
            alamat: $('#Profile-AlamatPerusahaan').val(),
        }
    }
    RequestData('PUT', `/v1/User/profile`, '#modal_profil', null, JSON.stringify(param), function (data_obj) {
        if (data_obj.succeeded) {
            AlertMessage("Update Profile Berhasil");
        }
        else if (data_obj.code == 401) { //unathorized
            AlertMessage(data_obj.message);
        } else
            ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
    });
}

function ListNotifications() {
    var page = 1;
    var pageSize = 100;
    var param = {
        filter: [
            {
                field: "id",
                search: ""
            },
        ],
        sort: {
            field: "id",
            type: 1
        },
        start: page,
        length: pageSize
    }
    RequestData('POST', "/v1/Notification/list", $('#list-notification'), $('#list-notification'), JSON.stringify(param), function (data) {
        if (data.succeeded) {
            $('#count-notification').text(data.count);
            $('#count-notification-text').text(`${data.count} New`);
            $('#item-notification').html('');
            if (data.list.length > 0) {
                data.list.forEach(function (item) {
                    var elItem = `<a href="${item.navigation}">
                    <div class="media">
                        <div class="media-left align-self-center"><i class="ft-plus-square icon-bg-circle bg-cyan"></i></div>
                        <div class="media-body">
                            <h6 class="media-heading">${item.subject}</h6>
                            <p class="notification-text font-small-3 text-muted">${item.description}</p>
                            <small>
                                <time class="media-meta text-muted">${timeSince(item.createDate)} ago</time>
                            </small>
                        </div>
                    </div>
                </a>`;
                    $('#item-notification').append(elItem);

                });
            } else {
                $('#count-notification').text('');
                $('#count-notification-text').text(`0 New`);
                $('#item-notification').html('');
            }
        }
    });
}

function ReadNotification() {
    RequestData('PUT', `/v1/Notification/open_all`, '', '', null, function (data) {
        if (data.succeeded) {
            ListNotifications();
        } else
            ShowNotif(`${data.message} : ${data.description}`, "error");
    });
}