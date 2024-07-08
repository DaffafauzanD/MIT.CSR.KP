$(document).ready(function () {

    getCount();
});

function getCount() {
    RequestData('GET', `/v1/Monitoring/list_progress/${$('#Id_Program').val()}`, element.table, element.tbody, JSON.stringify(param), function (data) {
        if (data.Succeeded) {
            console.log('data', data.Count);

        }
    });
}


// column chart
var chartProgram = {
    chart: {
        height: 350,
        type: 'bar',
        toolbar: {
            show: false
        }
    },
    plotOptions: {
        bar: {
            horizontal: false,
            endingShape: 'rounded',
            columnWidth: '55%',
        },
    },
    dataLabels: {
        enabled: false
    },
    stroke: {
        show: true,
        width: 2,
        colors: ['transparent']
    },
    series: [{
        name: 'Open',
        data: [44, 55, 57, 56, 61, 58, 63, 60, 66, 60, 81, 66]
    }, {
        name: 'On Progress',
        data: [76, 85, 101, 98, 87, 105, 91, 114, 94, 78, 88, 67]
    }, {
        name: 'Closed',
        data: [35, 41, 36, 26, 45, 48, 52, 53, 41, 66, 54, 34]
    }],
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    },
    yaxis: {
        title: {
            text: 'Jumlah Program'
        }
    },
    fill: {
        opacity: 1

    },
    tooltip: {
        y: {
            formatter: function (val) {
                return val + " program"
            }
        }
    },
    colors: ['#05a105', '#a9d1cd', '#82013d']
}

var chartOption = new ApexCharts(
    document.querySelector("#column-chart-program"),
    chartProgram
);

chartOption.render();

