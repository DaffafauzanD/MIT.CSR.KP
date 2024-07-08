function ListReferensi(method, referensi_name, url, data, select_container, select_data, setdata, callback, with_code, show_title) {
    RequestData(method, url, select_container, select_data, data, function (result) {
        if (result.succeeded) {
            $(select_data).html('');
            if (result.list.length > 0) {
                var counter = 0;
                var selected = "";
                if (show_title)
                    $(select_data).append(`<option value="" data-kode="" selected>----PILIH ${referensi_name}----</option>`);
                result.list.forEach(function (item) {
                    var text = item.name;
                    if (with_code == true)
                        text = (item.kode == undefined ? item.Id : item.kode) + " - " + item.name;
                    if (setdata != null || setdata != "") {
                        if (setdata == "any")
                            selected = counter == 0 ? "selected" : "";
                        else
                            selected = item.name == setdata ? "selected" : "";
                    }
                    // else {
                    //     if (show_title == null ||  show_title == false)
                    //         selected = counter == 0 ? "selected" : "";
                    // }
                    $(select_data).append(`<option value="${item.name}" data-kode="${item.kode}" ${selected}>${text}</option>`);
                    counter++;
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
                data.status.succeeded = false;
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
        if (callback != undefined || callback != null) {
            return callback(result.succeeded);
        }
    });
}

function ListJenisProgram(container, select_data, set_data,is_all) {
    var params = {
        "filter": null,
        "sort": {
            "field": "id",
            "type": 1
        },
        "start": null,
        "length": null
    }
    RequestData("POST", '/v1/jenisprogram/list', container, select_data, JSON.stringify(params), function (result) {
        if (is_all)
            $(select_data).html('<option selected value="">-- All--</option>');
        else
            $(select_data).html('');

        if (result.succeeded) {
            if (result.list.length > 0) {
                var selected = "";
                result.list.forEach(function (item) {
                    var text = item.name;
                    if (set_data != null || set_data != "") {
                        selected = item.id == set_data ? "selected" : "";
                    }
                    $(select_data).append(`<option value="${item.id}" ${selected}>${text}</option>`);
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
    });
 }

function ListDati(container, select_data, set_data, text_name) {
    var params = {
        "filter": null,
        "sort": {
            "field": "id",
            "type": 1
        },
        "start": null,
        "length": null
    }
    RequestData("POST", '/v1/dati/list', container, select_data, JSON.stringify(params), function (result) {
        $(select_data).html('');
        if (result.succeeded) {
            if (result.list.length > 0) {
                var selected = "";
                result.list.forEach(function (item) {
                    var text = item[text_name];
                    if (set_data != null || set_data != "") {
                        selected = item.id == set_data ? "selected" : "";
                    }
                    $(select_data).append(`<option value="${item.id}" ${selected}>${text}</option>`);
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
    });
}

function ListKegiatanReferensi(container, select_data, set_data, is_all, filter) {
    var params = {
        "filter": filter,
        "sort": {
            "field": "id",
            "type": 1
        },
        "start": null,
        "length": null
    }
    RequestData("POST", '/v1/kegiatan/list', container, select_data, JSON.stringify(params), function (result) {
        if (is_all)
            $(select_data).html('<option selected value="">-- All--</option>');
        else
            $(select_data).html('');

        if (result.succeeded) {
            if (result.list.length > 0) {
                var selected = "";
                result.list.forEach(function (item) {
                    var text = item.name;
                    if (set_data != null || set_data != "") {
                        selected = item.id == set_data ? "selected" : "";
                    }
                    $(select_data).append(`<option value="${item.id}" ${selected}>${text}</option>`);
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
    });
}

function ListRole(container, select_data, set_data, is_all, filter, value_name = true) {
    var params = {
        "filter": filter,
        "sort": {
            "field": "id",
            "type": 1
        },
        "start": null,
        "length": null
    }
    RequestData("POST", '/v1/user/role', container, select_data, JSON.stringify(params), function (result) {
        if (is_all)
            $(select_data).html('<option selected value="">-- All--</option>');
        else
            $(select_data).html('');

        if (result.succeeded) {
            if (result.list.length > 0) {
                var selected = "";
                result.list.forEach(function (item) {
                    var text = item.name;
                    if (set_data != null || set_data != "") {
                        selected = item.id == set_data ? "selected" : "";
                    }
                    $(select_data).append(`<option value="${value_name ? text : item.id}" ${selected}>${text}</option>`);
                });
            } else {
                $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
                ShowNotif("Data " + referensi_name + " Not Found!", "warning");
            }
        } else {
            $(select_data).append(`<option value="" selected disabled>--TIDAK ADA DATA--</option>`);
            ShowNotif(`${result.message} : ${result.description}`, "error");
        }
    });
}
