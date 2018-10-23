document.addEventListener("DOMContentLoaded", function (event) {
    var controller = document.getElementById('controller_name');
    var method = document.getElementById('method_name');
    var message = document.getElementById('ex_message');
    var type = document.getElementById('ex_type');

    if (controller !== null && method !== null && message !== null) {
        var controller_text = controller.innerText;
        var input = {
            'module': controller_text.slice(0, controller_text.indexOf('Controller')),
            'function': method.innerText,
            'message': message.innerText,
            'exceptiontype': type.innerText
        };

        var inner_message = document.getElementById('inner_ex_message');
        if (inner_message !== null) {
            input.innerMessage = inner_message.innerText;
        }
        showProcessing();
        createError(input);
        hideProcessing();
    }
});

function createError(input) {
    $.ajax({
        url: "../Error/CreateError",
        type: "post",
        data: JSON.stringify(input),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result !== null && result.Success) {
                document.getElementById('message').removeAttribute('hidden');
            } else {
                showError();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showError();
        }
    });
}

function showError() {
    swal("Chyba", "Nepodařilo se odeslat chybóvé hlášení. Pokud si přejete reportovanou chybu opravit, kontaktuje prosím e-mailem TP+, kde prosím přiložte screenshot obrazovky.", "error");
}