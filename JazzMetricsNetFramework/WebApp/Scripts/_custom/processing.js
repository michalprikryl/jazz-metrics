function showProcessing() {
    var process = document.getElementById("process");
    if (process.style.display === 'none') {
        process.style.display = '';
    }
}

function hideProcessing() {
    var process = document.getElementById("process");
    if (process.style.display === '') {
        process.style.display = 'none';
    }
}