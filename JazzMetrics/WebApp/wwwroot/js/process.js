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