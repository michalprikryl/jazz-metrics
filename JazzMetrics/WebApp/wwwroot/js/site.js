async function deleteItem(endpoint) {
    const result = await swal({
        title: 'Are you sure?',
        text: "Are you sure you want to delete this item? This operation is irreversible!",
        icon: "warning",
        buttons: ["Cancel", "Yes, delete it!"]
    });

    result && await postToServer(endpoint, 'Deleted!');
}

async function deleteUser(endpoint, where = 'company', body) {
    const result = await swal({
        title: 'Are you sure?',
        text: `Are you sure you want to delete user from this ${where}?`,
        icon: "warning",
        buttons: ["Cancel", "Yes, delete!"]
    });

    result && await postToServer(endpoint, 'Deleted!', body);
}

async function updateUser(endpoint) {
    const result = await swal({
        title: 'Are you sure?',
        text: "Are you sure you want to change role of this user?",
        icon: "warning",
        buttons: ["Cancel", "Yes, change!"]
    });

    result && await postToServer(endpoint, 'Changed!');
}

async function solveAppError(id, solve) {
    const result = await swal({
        title: 'Are you sure?',
        text: `Do you really want to mark this error as ${solve === 'True' ? 'solved' : 'not solved'}?`,
        icon: "warning",
        buttons: ["Cancel", "Yes, mark it!"]
    });

    result && await postToServer('AppError/Solve', solve === 'True' ? 'Solved!' : 'NOT solved!', { id, solve });
}

async function postToServer(endpoint, resultName, body) {
    showProcessing();

    const response = await fetch(endpoint, {
        method: "POST",
        body: JSON.stringify(body),
        headers: {
            'Content-Type': 'application/json',
            "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
        }
    });

    if (response.status >= 200 && response.status <= 304) {
        const text = await response.text();
        try {
            const data = JSON.parse(text);
            if (data.length === 0) {
                swalWithReload("Error", "Server not found!", "error");
            } else {
                if (data.success) {
                    swalWithReload(resultName, data.message, 'success');
                } else {
                    swalWithReload("Error", data.message, "error");
                }
            }
        } catch (err) {
            return text;
        }
    } else {
        swalWithReload("Error", "There was a processing error, please try again.", "error");
    }
}

async function swalWithReload(name, message, type) {
    await swal(name, message || "--", type);
    showProcessing();
    location.reload();
}

showProcessing = () => {
    const process = document.getElementById("process");
    if (process.style.display === 'none') {
        process.style.display = '';

        setTimeout(() => {
            const errors = document.getElementsByClassName("field-validation-error");
            if (errors.length !== 0) {
                hideProcessing();
            }
        }, 500);
    }
};

hideProcessing = () => {
    const process = document.getElementById("process");
    if (process.style.display === '') {
        process.style.display = 'none';
    }
};

window.onscroll = () => scrollFunction();

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("top-button").style.display = "block";
    } else {
        document.getElementById("top-button").style.display = "none";
    }
}

function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

!function () {
    $('.date').datepicker({
        orientation: "bottom left",
        daysOfWeekHighlighted: "0,6"
    });
}();

async function showErrorInfo(error) {
    showProcessing();

    const promise = fetch('/Setting/AppError/Info', {
        method: "POST",
        body: JSON.stringify(error),
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

    modal.addFooterBtn('Close', 'tingle-btn tingle-btn--danger', () => modal.close());

    const response = (await Promise.all([promise]))[0];
    if (response.status >= 200 && response.status <= 304) {
        modal.setContent(await response.text());
    } else {
        modal.setContent('<h1>Error occured, try again later please.</h1>');
    }

    hideProcessing();

    modal.open();
}