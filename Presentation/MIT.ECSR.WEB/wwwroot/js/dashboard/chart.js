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
        data: [0]
    }, {
        name: 'On Progress',
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0]
    }, {
        name: 'Closed',
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
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