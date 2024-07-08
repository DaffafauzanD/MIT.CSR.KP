//function SetDatePicker(el, format, minDate, maxDate, autoUpdateInput, startDate) {
function SetDatePicker(el, format, minDate, maxDate, autoUpdateInput, startDate, callback = null) {
    var formatDate = format == undefined || format == null ? 'DD MMMM, YYYY' : format;
    var config = {
        autoUpdateInput: autoUpdateInput == undefined || autoUpdateInput == null ? true : autoUpdateInput,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: formatDate,
            monthNames: monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        }
    }

    if (minDate != undefined && minDate != null) {
        config.minDate = minDate;
    }

    if (maxDate != undefined && maxDate != null) {
        config.maxDate = maxDate;
    }

    if (startDate != undefined && startDate != null) {
        config.startDate = startDate;
    }

    if (callback)
        $(el).daterangepicker(config, callback);  
    else
        $(el).daterangepicker(config);

    if (!autoUpdateInput) {
        $(el).on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format(formatDate));
        });
    }
}

function StringToDate(date, format) {
    var result = null;
    try {
        if (date != undefined && date != null && date != "") {
            if (format != undefined && format != null && format != "") {
            }
            else {
                format = "DD MMMM, YYYY";
            }
            if (format == "DD-MM-YYYY") {
                var d = date.split('-');
                var day = d[0];
                var month = d[1] - 1;
                var year = d[2];
                result = new Date(year, month, day);
            } else if (format == "DD MMMM, YYYY") {
                var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                var d = date.split(' ');
                var month_string = d[1].substr(0, d[1].length - 1);
                var day = d[0];
                var year = d[2];
                var month = 0;
                monthNames.forEach(function (item, index) {
                    if (item == month_string) {
                        month = index;
                    }
                });
                result = new Date(year, month, day);
            }
            else if (format == "MMMM, YYYY") {
                var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                var d = date.split(' ');
                var month_string = d[0].substr(0, d[0].length - 1);
                var year = d[1];
                var month = 0;
                monthNames.forEach(function (item, index) {
                    if (item == month_string) {
                        month = index;
                    }
                });
                result = new Date(year, month, 1);
            }
            else if (format == "DD/MM/YYYY") {
                var d = date.split('/');
                var day = d[0];
                var month = d[1] - 1;
                var year = d[2];
                result = new Date(year, month, day);
            }
            else if (format == "default") {
                console.log(d)
                result = d;
            }
        }
        else
            console.log("Value Date Kosong!" + date);
    }
    catch (err) {
        console.log(err);
    }
    return result;
}

function DateToString(date, format) {
    var result = "";
    try {
        if (date != undefined && date != null && date != "") {
            if (format != undefined && format != null && format != "") {
            }
            else
                format = "DD MMMM, YYYY";

            var day = date.getDate();
            day < 10 ? day = "0" + day : day;
            var month = date.getMonth() + 1;
            month < 10 ? month = "0" + month : month;
            var year = date.getFullYear();

            if (format == "DD-MM-YYYY") {
                result = day + '-' + month + '-' + year;
            }
            else if (format == "DD MMMM, YYYY") {
                var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                month = month - 1;
                result = day + ' ' + monthNames[month] + ', ' + year;
            }
            else if (format == "MMMM, YYYY") {
                var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                month = month - 1;
                result = monthNames[month] + ', ' + year;
            }
            else if (format == "DD/MM/YYYY") {
                result = day + '/' + month + '/' + year;
            }
            else if (format == "YYYY-MM-DD") {
                result = year + '-' + month + '-' + day;
            }
            else if (format == "MM-DD-YYYY") {
                result = month + '-' + day + '-' + year;
            }
        }
        else
            console.log("Value Date Kosong!" + el);
    }
    catch (err) {
        console.log(err);
    }
    return result;
}

function DateToStringTime(date) {
    var result = "";
    try {
        if (date != undefined && date != null && date != "") {
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var seconds = date.getSeconds();
            var minutesString = "";
            var secondString = "";
            if (minutes < 10)
                minutesString = "0" + minutes + "";
            else
                minutesString = minutes;
            if (seconds < 10)
                secondString = "0" + seconds + "";
            else
                secondString = seconds;

            result = hours + ':' + minutesString + ':' + secondString;
        }
    }
    catch (err) {
        console.log(err);
    }
    return result;
}

function DateToTick(date) {
    var result = null;
    if (date != undefined && date != date != null && date != "") {
        val = date.getTime();
        result = "/Date(" + val + ")/";
    }
    return result
}

function TickToDate(date) {
    var result = null;
    if (date != undefined && date != date != null && date != "") {
        result = new Date(parseInt(date.substr(6)));
    }
    return result
}
function TickToStringUTC(date) {
    var result = null;
    if (date != undefined && date != date != null && date != "") {
        //var tzoffset = (new Date()).getTimezoneOffset() * 60000;
        //console.log((new Date()).getTimezoneOffset());
        var tzoffset = -420 * 60000; //timezone jakarta
        var date = TickToDate(date);
        //result = date.toISOString();
        result = (new Date(date - tzoffset)).toISOString();
    }
    return result
}


function UTCToDate(val) {
    var date = null
    if (val != undefined && val != null && val != "") {
        var date = new Date(val);
    }
    return date;
}

function DateToStringFormat(el, format) {
    var result = "";
    if (el != undefined && el != null && el != "") {
        var date = UTCToDate(el);
        if (date != null) {
            result = DateToString(date, format);
        }
        else
            console.log("value tidak sesuai", el);
    }
    return result;
}

function DateToStringTimeFormat(el) {
    var result = "";
    if (el != undefined && el != null && el != "") {
        var date = UTCToDate(el);
        if (date != null) {
            result = DateToStringTime(date);
        }
        else
            console.log("value tidak sesuai", el);
    }
    return result;
}

function StringToDateFormat(el, format) {
    var result = "";
    if (el != undefined && el != null && el != "") {
        var date = StringToDate(el, format);
        if (date != null) {
            var tzoffset = -420 * 60000; //timezone jakarta
            result = (new Date(date - tzoffset)).toISOString();
        }
        else
            console.log("value tidak sesuai", el);
    }
    return result;
}

function monthDiff(d1, d2) {
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth();
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}



function timeSince(date) {

    var seconds = Math.floor((new Date() - new Date(date)) / 1000);
  
    var interval = seconds / 31536000;
  
    if (interval > 1) {
      return Math.floor(interval) + " years";
    }
    interval = seconds / 2592000;
    if (interval > 1) {
      return Math.floor(interval) + " months";
    }
    interval = seconds / 86400;
    if (interval > 1) {
      return Math.floor(interval) + " days";
    }
    interval = seconds / 3600;
    if (interval > 1) {
      return Math.floor(interval) + " hours";
    }
    interval = seconds / 60;
    if (interval > 1) {
      return Math.floor(interval) + " minutes";
    }
    return Math.floor(seconds) + " seconds";
  }
