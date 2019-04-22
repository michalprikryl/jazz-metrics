function editSettingValue(id) {
    const edit = document.getElementById(`edit-${id}`), form = document.getElementById(`form-${id}`);
    if (edit && form) {
        edit.classList.add('d-none');
        form.classList.add('d-flex');
        form.classList.remove('d-none');
    } else {
        swal('Oups', 'Cannot edit setting value. Please try to reload page.', 'warning');
    }
}

async function saveSettingValue(id) {
    const input = document.getElementById(`input-${id}`), edit = document.getElementById(`edit-${id}`), form = document.getElementById(`form-${id}`);
    if (input && edit && form) {
        await postToServer('/Setting/Setting/ChangeValue', 'Saved', { id, value: input.value });

        edit.classList.remove('d-none');
        form.classList.add('d-none');
        form.classList.remove('d-flex');
    } else {
        swal('Oups', 'Cannot edit setting value. Please try to reload page.', 'warning');
    }
}

function cancelEditing(id) {
    const edit = document.getElementById(`edit-${id}`), form = document.getElementById(`form-${id}`);

    edit.classList.remove('d-none');
    form.classList.add('d-none');
    form.classList.remove('d-flex');
}
