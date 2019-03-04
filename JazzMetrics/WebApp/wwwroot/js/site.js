// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showProcessing = () => {
    const process = document.getElementById("process");
    if (process.style.display === 'none') {
        process.style.display = '';
    }
};

hideProcessing = () => {
    const process = document.getElementById("process");
    if (process.style.display === '') {
        process.style.display = 'none';
    }
};