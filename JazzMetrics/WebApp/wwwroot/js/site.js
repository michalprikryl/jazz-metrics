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

!function () {
    $('.date').datepicker({
        orientation: "bottom left",
        daysOfWeekHighlighted: "0,6"
    });
}();