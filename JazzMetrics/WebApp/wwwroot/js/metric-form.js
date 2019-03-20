async function addColumn() {
    const result = await swal({
        icon: 'info',
        title: 'Question',
        text: 'Which property type you want to create? Please choose one.',
        buttons: {
            cancel: true,
            type1: {
                text: "Number column",
                value: "number"
            },
            type2: {
                text: "Coverage",
                value: "coverage"
            }
        }
    });

    if (result) {
        const request = {
            index: document.getElementsByClassName(result === 'number' ? 'number-column' : 'coverage-column').length,
            type: result
        };

        const response = await postToServer('/Setting/Metric/AddColumn', undefined, request);
        document.getElementById('columns').insertAdjacentHTML('beforeend', response);

        hideProcessing();
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