function openMenu(url) {
    window.open(window.location.origin + url, '_self').focus();
}

function ShowLoading(target, effect = 'bounce', message = '', callback = null) {
    //$(target).waitMe({
    //    effect: effect,
    //    text: message !== undefined || message !== null ? message : '',
    //    bg: 'rgba(255,255,255,0.7)',
    //    color: '#000',
    //    maxSize: '',
    //    waitTime: -1,
    //    textPos: 'vertical',
    //    fontSize: '',
    //    source: '',
    //    onClose: callback !== undefined && callback !== null ? callback : function () {
    //    }
    //});
    $(target).block({
        message: '<div class="ft-refresh-cw icon-spin font-medium-2"></div>',
        overlayCSS: {
            backgroundColor: '#fff',
            opacity: 0.8,
            cursor: 'wait'
        },
        css: {
            border: 0,
            padding: 0,
            backgroundColor: 'transparent'
        }
    });
}

function ShowNotif(message, type, time) {
    toastr.error(message, 'Error!');
}

function SetTableData(haveData, colspan, el, data, callback) {
    var el_tbody = el != undefined || el != null ? el.tbody : '#tbody';
    var el_from = el != undefined || el != null ? el.from : '#from_page';
    var el_to = el != undefined || el != null ? el.to : '#to_page';
    var el_total = el != undefined || el != null ? el.total : '#total_count';
    var el_pagination = el != undefined || el != null ? el.pagination : '#pagination';
    var el_item_pagination = el != undefined || el != null ? el.item_pagination : 'item-page';

    var from = haveData ? ((data.page - 1) * data.pageSize) + 1 : 0;
    var totalPage = haveData ? data.count % data.pageSize == 0 ? Math.floor(data.count / data.pageSize) : Math.floor(data.count / data.pageSize) + 1 : 0;
    var to = haveData ? totalPage == data.page ? data.count : (data.page * data.pageSize) : 0;

    $(el_from).html(from);
    $(el_to).html(to);
    $(el_total).html(haveData ? data.count : 0);
    $(el_pagination).html(`<ul class="pagination justify-content-end" id="pagination-page_${el_item_pagination}"></ul>`);

    if (haveData) {
        SetPagination(`pagination-page_${el_item_pagination}`, data.page, totalPage, data.function_name, data.function_param);
        return callback(from);
    } else {
        $(el_tbody).append(`<tr><td colspan="${colspan}"><center>Data Not Found!</center></td></tr>`);
        $(`#pagination-page_${el_item_pagination}`).append(`
            <li class="page-item disabled"><a class="page-link">First</a></li>
            <li class="page-item disabled"><a class="page-link">0</a></li>
            <li class="page-item disabled"><a class="page-link">Last</a></li>
        `);
    }
}

function SetPagination(pagination_id, page, totalPage, function_name, function_param) {
    if (page == 1) {
        $(`#${pagination_id}`).append('<li class="page-item disabled"><a class="page-link" href="#">First</a></li>');
        $(`#${pagination_id}`).append('<li class="page-item disabled"><a class="page-link" href="#">Previous</a></li>');
    }
    else {
        var firstPageParam = function_param !== undefined ? `('${function_param}', 1)` : `(1)`;
        var pageParam = function_param !== undefined ? `('${function_param}', ${(page - 1)})` : `(${(page - 1)})`;
        $(`#${pagination_id}`).append(`<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="${function_name}${firstPageParam}">First</a></li>`);
        $(`#${pagination_id}`).append(`<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="${function_name}${pageParam}">Previous</a></li>`);
    }

    var i = page == 1 ? page : page == 2 ? page - 1 : page == 3 ? page - 2 : page == 4 ? page - 3 : page == totalPage ? totalPage - 4 : page == totalPage - 1 ? totalPage - 4 : page == totalPage - 2 ? totalPage - 4 : page == totalPage - 3 ? totalPage - 5 : page == totalPage - 4 ? totalPage - 6 : page - 2;
    var toPage = totalPage <= 5 ? totalPage : page == 1 ? page + 4 : page == 2 ? page + 3 : page == 3 ? page + 2 : page == totalPage ? totalPage : page == totalPage - 1 ? totalPage : page == totalPage - 2 ? totalPage : page == totalPage - 3 ? totalPage - 1 : page + 2;
    for (i; i <= toPage; i++) {
        if (i == page) {
            $(`#${pagination_id}`).append(`<li class="page-item disabled"><a class="page-link" href="javascript:void(0);"><b>${i}</b></a></li>`);
        } else {
            var pageParam = function_param !== undefined ? `('${function_param}', ${i})` : `(${i})`;

            $(`#${pagination_id}`).append(`<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="${function_name}${pageParam}"><b>${i}</b></a></li>`);
        }
    }

    if (page == totalPage) {
        $(`#${pagination_id}`).append('<li class="page-item disabled"><a class="page-link" href="#">Next</a></li>');
        $(`#${pagination_id}`).append('<li class="page-item disabled"><a class="page-link" href="#">Last</a></li>');
    }
    else {
        var pageParam = function_param !== undefined ? `('${function_param}', ${(page + 1)})` : `(${(page + 1)})`;
        var lastPageParam = function_param !== undefined ? `('${function_param}', ${totalPage})` : `(${totalPage})`;

        $(`#${pagination_id}`).append(`<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="${function_name}${pageParam}">Next</a></li>`);
        $(`#${pagination_id}`).append(`<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="${function_name}${lastPageParam}">Last</a></li>`);
    }
}

function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function AlertMessage(message, callback, icon) {
    swal({
        text: message,
        showCancelButton: false,
        icon: icon,
        buttons: {
            confirm: {
                text: "OK",
                value: true,
                visible: true,
                className: "btn-danger",
                closeModal: true,
            }
        }
    }).then(isConfirm => {
        if (callback != undefined && callback != null && callback != "")
            return callback(isConfirm);
    });
}

function ConfirmMessage(message, callback) {
    swal({
        text: message,
        showCancelButton: true,
        buttons: {
            cancel: {
                text: "No",
                value: null,
                visible: true,
                className: "btn-danger",
                closeModal: true,
            },
            confirm: {
                text: "Yes",
                value: true,
                visible: true,
                className: "btn-primary",
                closeModal: true,
            }
        }
    }).then(isConfirm => {
        return callback(isConfirm);
    });
}

function FileToBase64(file) {
    return new Promise((resolve, reject) => {
        var reader = new FileReader();
        reader.onload = e => {
            resolve(/base64,(.+)/.exec(e.target.result)[1]);
        };
        reader.onerror = error => reject(error);
        reader.readAsDataURL(file);
    });
}

function Base64ToFile(filename, mimeTypeValue, base64String) {
    let downloadLink = document.createElement('a');

    downloadLink.href = `data:${mimeTypeValue};base64,${base64String}`;;
    downloadLink.download = filename;
    downloadLink.click();
}

function getExtension(filename) {
    return filename.split('.').pop().toLowerCase();
}

function SetPicture(elInputFile = null,target= null, callback = null) {
    elInputFile = elInputFile !== null ? elInputFile : '.custom-file-input';
    target = target !== null ? target : '#img-preview';
    $(elInputFile).on('change', function () {
        let file = this.files[0];
        $(this).siblings('.custom-file-label').text(file.name);

        if (file.type.includes('image')) {
            let reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = (e) => $(target).attr("src", e.target.result);
        }
        if (callback !== null)
            callback();
    });
}
function Setfile(elInputFile = null) {
    elInputFile = elInputFile !== null ? elInputFile : '.custom-file-input';
    $(elInputFile).on('change', function () {
        let file = this.files[0];
        $(this).siblings('.custom-file-label').text(file.name);
    });
}
function formatNumber(x) {
    return isNaN(x) ? "" : x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function FormValidate(formId) {
    var result = true
    $(`#${formId}`).find('input[type="text"], input[type="number"], input[type="password"], select, textarea').not('.not-validate').each(function (i, el) {
        if (el.value == "") {
            if ($(el).parents('.input-group').length != 0) {
                $(el).parents('.input-group').siblings('.txt-msg').show();
                $(el).parents('.input-group').addClass('input-error');
            } else {
                $(el).siblings('.txt-msg').show();
                $(el).addClass('input-error');
            }
            result = false;
        } else {
            if ($(el).parents('.input-group').length != 0) {
                $(el).parents('.input-group').siblings('.txt-msg').hide();
                $(el).parents('.input-group').removeClass('input-error');
            } else {
                $(el).siblings('.txt-msg').hide();
                $(el).removeClass('input-error');
            }
        }
    });

    return result;
}
function maxNumber(object, max) {
    if (object.value > max)
        object.value = max;
}



/// PENGGUNAAN CHART  : BERIKUT CONTOH PENGGUNAANNYA
//var json = [ //DATA JSON WAJIB SEPERTI INI
//    {
//        "name": "Complete Project",
//        "type": "bar",
//        "data": [
//            2,
//            4.9,
//            7,
//            23.2,
//            25.6,
//            76.7,
//            135.6,
//            162.2,
//            32.6,
//            20,
//            6.4,
//            3.3
//        ]
//    },
//    {
//        "name": "New Project",
//        "type": "bar",
//        "data": [
//            2.6,
//            5.9,
//            9,
//            26.4,
//            28.7,
//            70.7,
//            175.6,
//            182.2,
//            48.7,
//            18.8,
//            6,
//            2.3
//        ]
//    }
//];
//var data_page = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
//getBarGraph('main', 'Chart Data', JSON.stringify(json), JSON.stringify(data_page))

function getBarGraph(element, data, data_header) {
    
    var dom = document.getElementById(element);
    var myChart = echarts.init(dom);
    var option = {
    
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            data: ["Open", "On Progres", "Closed"]
        },
        xAxis: [
            {
                type: 'category',
                data: JSON.parse(data_header) //['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
            }
        ],
        yAxis: [{ type: 'value' }],

        series: JSON.parse(data),
    };
    myChart.setOption(option);

}