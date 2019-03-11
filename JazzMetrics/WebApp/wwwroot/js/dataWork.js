async function deleteItem(endpoint) {
    const result = await swal({
        title: 'Are you sure?',
        text: "Are you sure you want to delete this item? This operation is irreversible!",
        icon: "warning",
        buttons: ["Cancel", "Yes, delete it!"]
    });

    result && await postToServer(endpoint, 'Deleted!');
}

async function deleteUser(endpoint) {
    const result = await swal({
        title: 'Are you sure?',
        text: "Are you sure you want to delete user from this company?",
        icon: "warning",
        buttons: ["Cancel", "Yes, delete!"]
    });

    result && await postToServer(endpoint, 'Deleted!');
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

async function postToServer(endpoint, resultName, body) {
    showProcessing();

    const response = await fetch(endpoint, {
        method: "POST",
        body: body,
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
                swalWithReload(resultName, data.message, 'success');
            } else {
                swalWithReload("Error", data.Message, "error");
            }
        }
    } else {
        swalWithReload("Error", "There was a processing error, please try again.", "error");
    }
}

async function swalWithReload(name, message, type) {
    await swal(name, message, type);
    location.reload();
}