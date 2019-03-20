﻿function showMetric(id) {
    $(`#${id}`).collapse('show');
}

function hideMetric(id) {
    $(`#${id}`).collapse('hide');
}

function random_rgba() {
    var o = Math.round, r = Math.random, s = 255;
    return 'rgba(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ',0.3)';
}

function makeBarChart(canvasId, data, labels, title) {
    const colors = labels.map(() => random_rgba());
    new Chart(document.getElementById(canvasId).getContext('2d'), {
        type: 'bar',
        data: {
            labels,
            datasets: [{
                label: 'Counts',
                data,
                backgroundColor: colors,
                borderColor: colors.map((color) => color.replace(',0.3', ',1')),
                borderWidth: 1
            }]
        },
        options: {
            ...getCommonOptions(title),
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: Math.max(...data)
                    },
                    scaleLabel: {
                        display: true,
                        labelString: 'Count'
                    }
                }]
            },
            tooltips: {
                callbacks: {
                    label: (tooltipItem) => `Count: ${tooltipItem.yLabel}`
                }
            }
        }
    });
}

function makeLineChart(canvasId, data, labels, title) {
    const color = random_rgba();
    let zipped = labels.map((x, i) => [x, data[i]]);
    new Chart(document.getElementById(canvasId).getContext('2d'), {
        type: 'line',
        data: {
            datasets: [{
                label: 'Coverage',
                data: zipped.map(item => {
                    return {
                        x: item[0],
                        y: item[1]
                    };
                }),
                backgroundColor: [
                    color
                ],
                borderColor: [
                    color.replace(',0.3', ',1')
                ],
                borderWidth: 1
            }]
        },
        options: {
            ...getCommonOptions(title),
            scales: {
                xAxes: [{
                    type: 'time',
                    time: {
                        tooltipFormat: "MMM D, YYYY h:mm:ss a"
                    },
                    display: true,
                    ticks: {
                        major: {
                            fontStyle: 'bold',
                            fontColor: '#FF0000'
                        }
                    }
                }],
                yAxes: [{
                    display: true,
                    ticks: {
                        min: 0,
                        max: 100
                    },
                    scaleLabel: {
                        display: true,
                        labelString: 'Coverage'
                    }
                }]
            },
            tooltips: {
                callbacks: {
                    label: (tooltipItem) => `Coverage: ${tooltipItem.yLabel} %`
                }
            }
        }
    });
}

function getCommonOptions(title) {
    return {
        layout: {
            padding: {
                left: 30,
                right: 30,
                top: 30,
                bottom: 30
            }
        },
        animation: {
            duration: 3000
        },
        title: {
            display: true,
            text: title || ''
        }
    };
}

/*
[
'rgba(255, 99, 132, 0.2)',
'rgba(54, 162, 235, 0.2)',
'rgba(255, 206, 86, 0.2)',
'rgba(75, 192, 192, 0.2)',
'rgba(153, 102, 255, 0.2)',
'rgba(255, 159, 64, 0.2)'
],
*/

/*
[
'rgba(255, 99, 132, 1)',
'rgba(54, 162, 235, 1)',
'rgba(255, 206, 86, 1)',
'rgba(75, 192, 192, 1)',
'rgba(153, 102, 255, 1)',
'rgba(255, 159, 64, 1)'
],
*/

//'rgba(54, 162, 235, 0.2)'
//'rgba(54, 162, 235, 1)'