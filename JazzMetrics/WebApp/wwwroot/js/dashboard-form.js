function showMetric(id) {
    $(`.${id}`).collapse('show');
}

function hideMetric(id) {
    $(`.${id}`).collapse('hide');
}

async function updateMetrics(projectId, projectMetricId) {
    const result = await swal({
        title: 'Information',
        text: 'Update may last long time, do you really want to update metric data?',
        icon: 'info',
        buttons: ["Cancel", "Yes, update!"]
    });

    if (result) {
        showProcessing();

        const response = await fetch(projectMetricId ? `/Project/${projectId}/Update/${projectMetricId}` : `/Project/${projectId}/Update`, {
            method: "GET",
            headers: {
                "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
            }
        });

        if (response.status >= 200 && response.status <= 304) {
            const data = await response.json();
            if (data.length === 0) {
                swalWithReload("Error", "Server not found!", "error");
            } else {
                if (data.success) {
                    swalWithReload('Result', data.message, 'success');
                } else {
                    swalWithReload('Error', data.message, "error");
                }
            }
        } else {
            swal('Error', 'Error occurred while processing you request! Try again please.', 'error');
        }

        hideProcessing();
    }
}

function random_rgba() {
    const o = Math.round, r = Math.random, s = 255;
    return 'rgba(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ',0.3)';
}

let colors, loadedId = '';
function makeBarChart(canvasId, data, labels, title, id) {
    if (loadedId !== id) {
        loadedId = id;
        colors = labels.map(() => random_rgba());
    }

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
                top: 0,
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

function saveChartToPng(id, name = 'metrics_export') {
    const chart = document.getElementById(id);
    if (chart) {
        chart.toBlob((blob) => saveAs(blob, `${name.replace(new RegExp(' ', 'g'), '_')}__${id}.png`));
    } else {
        swal('Error', 'Unknown chart to export! Try again, please.', 'warning');
    }
}

function saveChartToPptx(metrics) {
    try {
        const headingSetting = {
            x: 0,
            y: '5%',
            w: '100%',
            align: 'center',
            bold: true,
            fontFace: 'Calibri',
            fontSize: 18,
            color: '363636'
        };
        const imageSetting = {
            x: '10%',
            y: '10%',
            w: '80%',
            h: '80%'
        };

        const pptx = new PptxGenJS();
        pptx.setAuthor('jazz-metrics');
        pptx.setCompany('VSB-TUO');
        pptx.setRevision('1');
        pptx.setSubject('Metrics report');
        pptx.setTitle('Metrics report');

        for (let j = 0; j < metrics.length; j++) {
            for (let i = 0; i < metrics[j].charts.length; i++) {
                const chart = document.getElementById(metrics[j].charts[i]);
                if (chart) {
                    const slide = pptx.addNewSlide();
                    slide.addText(metrics[j].metricName, headingSetting);
                    imageSetting.data = chart.toDataURL("image");
                    slide.addImage(imageSetting);
                }
            }
        }

        pptx.save('metrics_export');
    } catch {
        swal('Error', 'Error occured within export! Try again, please.', 'warning');
    }
}

async function saveChartToXlsx(metrics) {
    try {
        const now = new Date();
        const workbook = new ExcelJS.Workbook();
        workbook.creator = 'jazz-metrics';
        workbook.lastModifiedBy = 'jazz-metrics';
        workbook.created = now;
        workbook.modified = now;
        workbook.lastPrinted = now;

        for (let j = 0; j < metrics.length; j++) {
            const worksheet = workbook.addWorksheet(metrics[j].metricName);
            const heading = worksheet.getCell('A1');
            heading.value = metrics[j].metricName;
            heading.font = {
                name: 'Calibri',
                bold: true,
                size: 20
            };

            for (let i = 0, row = 0; i < metrics[j].charts.length; i++) {
                const chart = document.getElementById(metrics[j].charts[i]);
                if (chart) {
                    const imageId = workbook.addImage({
                        base64: chart.toDataURL("image"),
                        extension: 'png'
                    });
                    worksheet.addImage(imageId, {
                        tl: { col: 0, row: 2 + row },
                        br: { col: 13, row: 22 + row },
                        editAs: 'absolute'
                    });
                    row += 20;
                }
            }
        }

        saveAs(new Blob([await workbook.xlsx.writeBuffer()]), "metrics_export.xlsx");
    } catch {
        swal('Error', 'Error occured within export! Try again, please.', 'warning');
    }
}

function saveChartToDocx(metrics) {
    try {
        const doc = new Document({
            creator: "jazz-metrics",
            title: "Charts export",
            description: "Exported charts from one or many metrics."
        });

        const emptyParagraph = new Paragraph();

        const headerParagraph = new Paragraph().heading1().center().thematicBreak();
        headerParagraph.addRun(new TextRun("Metrics export").font("Calibri"));
        doc.addParagraph(headerParagraph);

        const options = {
            floating: {
                horizontalPosition: {
                    relative: HorizontalPositionRelativeFrom.PAGE
                },
                verticalPosition: {
                    relative: VerticalPositionRelativeFrom.PAGE
                }
            }
        };

        for (let j = 0, graphCount = 0; j < metrics.length; j++) {
            const metricHeaderParagraph = new Paragraph().heading2().center();
            metricHeaderParagraph.addRun(new TextRun(metrics[j].metricName).font("Calibri").break());
            doc.addParagraph(metricHeaderParagraph);

            for (let i = 0; i < metrics[j].charts.length; i++) {
                const chart = document.getElementById(metrics[j].charts[i]);
                chart && doc.createImage(chart.toDataURL("image"), 580, 380, options);

                ++graphCount % 2 === 0 && graphCount !== 0 && doc.addParagraph(new Paragraph().pageBreak());
            }

            const metricEndParagraph = new Paragraph().thematicBreak();
            doc.addParagraph(metricEndParagraph);
        }

        doc.addParagraph(emptyParagraph);

        const packer = new Packer();
        packer.toBlob(doc).then((blob) => {
            saveAs(blob, "metrics_export.docx");
        });
    } catch {
        swal('Error', 'Error occured within export! Try again, please.', 'warning');
    }
}

async function exportAllMetrics() {
    showProcessing();

    const parents = document.getElementsByClassName('parent');

    const metrics = [];
    for (let i = 0; i < parents.length; i++) {
        const metric = {
            charts: [],
            metricName: parents[i].getElementsByClassName('center-name')[0].innerHTML
        };

        const canvas = parents[i].querySelectorAll('canvas[id^="chart-"]');
        for (let j = 0; j < canvas.length; j++) {
            metric.charts.push(canvas[j].id);
        }

        metrics.push(metric);
    }

    const promise = fetch('/Project/Export', {
        method: "POST",
        body: JSON.stringify({ metrics }),
        headers: {
            'Content-Type': 'application/json',
            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
        }
    });

    const modal = new tingle.modal({
        footer: true,
        stickyFooter: false,
        closeMethods: ['overlay', 'button', 'escape'],
        closeLabel: "Close"
    });

    modal.addFooterBtn('Export', 'tingle-btn tingle-btn--primary', function () {
        modal.close();
        showProcessing();

        const chosenMetrics = [];
        const content = modal.getContent();
        const checkboxes = content.getElementsByClassName('metric');
        for (let i = 0; i < checkboxes.length; i++) {
            checkboxes[i].checked && chosenMetrics.push(JSON.parse(checkboxes[i].value));
        }

        if (chosenMetrics.length > 0) {
            let exportType;
            const exports = content.getElementsByClassName('export');
            for (let i = 0; i < exports.length; i++) {
                if (exports[i].checked) {
                    exportType = exports[i].value;
                }
            }

            if (exportType === 'powepoint') {
                saveChartToPptx(chosenMetrics);
            } else if (exportType === 'excel') {
                saveChartToXlsx(chosenMetrics);
            } else if (exportType === 'word') {
                saveChartToDocx(chosenMetrics);
            } else {
                swal('Warning', 'Choose one export type, please!', 'warning');
            }
        } else {
            swal('Warning', 'Choose at least one metric, please!', 'warning');
        }

        hideProcessing();
    });

    modal.addFooterBtn('Cancel', 'tingle-btn tingle-btn--danger', () => modal.close());

    const response = (await Promise.all([promise]))[0];
    if (response.status >= 200 && response.status <= 304) {
        modal.setContent(await response.text());
    } else {
        modal.setContent('<h1>Error occured, try again later please.</h1>');
    }

    hideProcessing();

    modal.open();
}

function changeMetrics() {
    const form = document.getElementById('modal-form');

    const button = form.querySelector('#all-button');
    const checked = button.innerHTML.startsWith('Uncheck');
    button.innerHTML = checked ? 'Check all metrics' : 'Uncheck all metrics';

    [...form.getElementsByClassName('metric')].forEach((metric) => metric.checked = !checked);
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