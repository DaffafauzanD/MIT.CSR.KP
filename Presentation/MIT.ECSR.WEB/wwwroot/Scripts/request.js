var cookie_token = "MIT.ECSR.token";

function RequestData(type, url, container, field, params, callback, isJson = true, is_annonymous = false, is_append = false) {
    var url_api = "http://api-ecsr.mediaindoteknologi.com/api";
    var token = getCookie(cookie_token);

    var config = {
        async: true,
        type: type,
        url: url_api + url,
        data: params,
        headers: {
            "Content-Type": 'application/json'
        },
        beforeSend: function (xhr) {
            if (container != undefined && container != null && container != "")
                ShowLoading(container);
        },
        error: function (err) {
            if (container != undefined && container != null && container != "")
                $(container).unblock();
            if (err.responseJSON != undefined && err.responseJSON != null) {
                if (callback != undefined && callback != null && callback != "")
                    return callback(err.responseJSON);
                else
                    ShowNotif(err.responseJSON.message, "error");
            } else
                ShowNotif("Something went wrong!", "error");
        },
        success: function (data) {
            if (field != undefined && field != null && field != "" && is_append == false)
                $(field).html('');

            if (container != undefined && container != null && container != "")
                $(container).unblock();

            if (callback != undefined && callback != null && callback != "")
                return callback(data);
        }
    };
    if (!is_annonymous) {
        config.headers = {
            "Authorization": "bearer " + token,
            "Content-Type": 'application/json'
        };
    }
    if (isJson)
        config.dataType = 'json';

    $.ajax(config);
}



function RequestDataFormData(type, url, container, field, params, callback) {

    $.ajax({
        async: true,
        type: type,
        cache: false,
        contentType: false,
        processData: false,
        url: window.location.origin + url,
        data: params,
        beforeSend: function (xhr) {
            if (container != undefined && container != null && container != "")
                ShowLoading(container);
        },
        error: function (err) {
            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');

            console.log(err);
            ShowNotif("Something went wrong", "error");
        },
        success: function (data) {
            if (field != undefined && field != null && field != "")
                $(field).html('');

            if (container != undefined && container != null && container != "")
                $(container).waitMe('hide');

            if (callback != undefined && callback != null && callback != "")
                return callback(data);
        }
    });
}


function RequestReferensi(referensi_name, url, data, select_container, select_data, setdata, callback, with_code, show_title, is_append) {
    RequestData("POST", url, select_container, select_data, JSON.stringify(data), function (data) {
        if (data.succeeded) {
            if (data.list.length > 0) {
                var counter = 0;
                var selected = "";
                if (show_title)
                    $(select_data).append(`<option value="" data-kode="" selected disabled>----PILIH ${referensi_name}----</option>`);
                data.list.forEach(function (item) {
                    var text = item.nama;
                    if (with_code != undefined || with_code != null || with_code == true)
                        text = (item.kode == undefined ? item.id : item.kode) + " - " + item.nama;
                    if (setdata != null || setdata != "") {
                        if (setdata == "any")
                            selected = counter == 0 ? "selected" : "";
                        else
                            selected = item.id == setdata ? "selected" : "";
                    } else {
                        if (show_title == null || show_title == false)
                            selected = counter == 0 ? "selected" : "";
                    }
                    $(select_data).append(`<option value="${item.id}" data-kode="${item.kode}" ${selected}>${text}</option>`);
                    counter++;
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
                data.succeeded = false;
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
        if (callback != undefined || callback != null) {
            return callback(data.succeeded);
        }
    }, true, false, is_append);
}


function RequestComboBox(title, endpoint, select_container, select_data, setdata, callback, with_code, show_title, is_append, show_all = false) {
    var param = {
        filter: [{
            field: "active",
            search: "true"
        }],
        sort: {
            field: "id",
            type: 0
        }
    };
    if (show_all) {
        param = { sort: { field: "id", type: 0 } };
    }
    RequestReferensi(title, `/v1/${endpoint}/list`, param, select_container, select_data, setdata, callback, with_code, show_title, is_append);
}