async function addColumn() {
    const result = await swal({
        icon: 'info',
        title: 'Question',
        text: 'Which property type you want to create? Please choose one.',
        buttons: {
            cancel: true,
            type1: {
                text: "Number column",
                value: "number",
            },
            type2: {
                text: "Coverage",
                value: "coverage",
            },
        },
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
    document.getElementById(`del-${column}`).value = "True";
    document.getElementById(column).classList.add('d-none');
}