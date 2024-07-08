$(document).ready(function () {
    $('#Search-Dasboard-Status').on('change', function () {
        ListDashboard();
    });

    ListDashboardYear();
});
function ListDashboard() {
    RequestData('GET', '/v1/Program/dashboard?year=' + $('#Search-Dasboard-Status').val(), '', '', null, function (data) {
        if (data.succeeded) {
            getBarGraph('main', JSON.stringify(data.data.items), JSON.stringify(data.data.dataPage));
            $('#Program-Tr-Head').empty();
            var elHead = `<th width="5%" class="text-center">Status</th>`;
            data.data.dataPage.forEach(function (item) {
                elHead += `<th width="10%">${item}</th>`;
            });
            $('#Program-Tr-Head').append(elHead);

            $('#Program-tbody').empty();
            data.data.items.forEach(function (item) {
                var elBody = `<tr><td>${item.name}</td>`;
                item.data.forEach(function (itemData) {
                    elBody += `<td>${itemData}</td>`;
                });
                elBody += `</tr>`;
                $('#Program-tbody').append(elBody);
            });
            
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}

function ListDashboardYear() {
    $('#Search-Dasboard-Status').empty();
    RequestData('GET', '/v1/Program/list-year-dashboard', '', '', null, function (data) {
        if (data.succeeded) {
            if (data.data.year != null) {
                var el = `<option>--- PILIH TAHUN ---</option>`;
                data.data.year.forEach(function (item) {
                    el += `<option value="${item}">${item}</option>`;
                    $('#Search-Dasboard-Status').append(el);
                });
            }
        } else {
            ShowNotif(`${data.message} : ${data.description}`, "error");
        }
    });
}
