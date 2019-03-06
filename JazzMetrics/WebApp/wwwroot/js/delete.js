function deleteItem(endpoint) {
    swal({
        title: 'Are you sure?',
        text: "Are you sure you want to delete this item? This operation is irreversible!",
        icon: "warning",
        buttons: ["Cancel", "Yes, delete it!"]
    }).then(async (result) => {
        if (result) {
            showProcessing();

            const response = await fetch(endpoint, {
                method: "POST",
                headers: {
                    "RequestVerificationToken": document.getElementsByName("__RequestVerificationToken")[0].value
                }
            });

            if (response.status >= 200 && response.status <= 304) {
                const data = await response.json();
                if (data.length === 0) {
                    swalWithProcess("Error", "Server not found!", "error");
                } else {
                    if (data.success) {
                        swalWithProcess('Deleted!', data.message, 'success');
                    } else {
                        swalWithProcess("Error", data.Message, "error");
                    }
                }
            } else {
                swalWithProcess("Error", "There was a processing error, please try again.", "error");
            }
        }
    });
}

function swalWithProcess(name, message, type) {
    swal(name, message, type)
        .then(() => {
            location.reload();
        });
}