$(document).on("keyup", "#Username,#Password", function (e) {
    if (e.which == 13) {
        Login();
    }
})

function Login() {
    var param = {
        username: $('#Username').val(),
        password: $('#Password').val()
    }
    if ((param.username == null && param.password == null) || (param.username == "" && param.password == "")) {
        swal("Error Login", "Username & Password Cannot null");
    } else if ((param.username == null) || (param.username == "")) {
        swal("Error Login", "Username Cannot null");
    }
    else if ((param.password == null) || (param.password == "")) {
        swal("Error Login", "Password Cannot null");
    } else {
        RequestData('POST', `api/v1/User/login`, 'body', null, JSON.stringify(param), function (data_obj) {
            if (data_obj.succeeded) {
                setCookie("MIT.ECSR.token", data_obj.data.rawToken, 1);
                window.location = window.location.origin;
            }
            else if (data_obj.code == 400) { //unathorized
                ShowNotif(`${data_obj.message} : ${data_obj.description}`, "error");
            }
            else
                swal("Error Login", data_obj.message, "warning");
        }, true, true);
    }


}