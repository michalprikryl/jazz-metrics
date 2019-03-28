async function addColumn() {
    const select = document.getElementById("MetricTypeId");
    if (select) {
        const isNumberColumn = select.options[select.selectedIndex].text.toLowerCase().startsWith('number');
        const request = {
            index: document.getElementsByClassName(isNumberColumn ? 'number-column' : 'coverage-column').length,
            type: isNumberColumn ? 'number' : 'coverage'
        };

        const response = await postToServer('/Setting/Metric/AddColumn', undefined, request);
        document.getElementById('columns').insertAdjacentHTML('beforeend', response);

        hideProcessing();
    } else {
        swal('Error', 'Unknown metric type!', 'error');
    }
}

let selectedTypeId;
!function () {
    selectedTypeId = document.getElementById("MetricTypeId").value;
}();

async function onSelectChange() {
    const select = document.getElementById("MetricTypeId");
    const isNumberColumn = select.options[select.selectedIndex].text.toLowerCase().startsWith('number');
    if (document.getElementsByClassName(isNumberColumn ? 'coverage-column' : 'number-column').length > 0) {
        const result = await swal({
            title: 'Are you sure?',
            text: 'You change a metric type to different type. If you really want to do this, all your metric columns must be deleted. Do you want to change metric type?',
            icon: "warning",
            buttons: ["Cancel", "Yes, change!"]
        });

        if (result) {
            selectedTypeId = select.value;

            const columns = document.getElementById('columns');
            while (columns.firstChild) {
                columns.removeChild(columns.firstChild);
            }
        } else {
            select.value = selectedTypeId;
        }
    }
}

function dropColumn(column) {
    const deleted = document.getElementById(`del-${column}`);
    const inputs = document.querySelectorAll(`#${column} input[type=text]`);
    if (inputs[0].value !== "" || (inputs.length === 2 && inputs[1].value !== "")) {
        const span = document.querySelector(`#${column} .pointer`);
        if (deleted.value === "True") {
            inputs.forEach(i => {
                i.title = "";
                i.disabled = false;
            });
            span.title = "Drop attribute";
            span.style.color = "#495057";
            deleted.value = "False";
        } else {
            inputs.forEach(i => {
                i.title = "Metric attribute will be deleted!";
                i.disabled = true;
            });
            span.title = "Restore attribute";
            span.style.color = "red";
            deleted.value = "True";
        }
    } else {
        deleted.value = "True";
        document.getElementById(column).classList.add('d-none');
    }
}