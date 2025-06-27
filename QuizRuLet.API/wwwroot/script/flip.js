
document.addEventListener('DOMContentLoaded', function () {
    const flipToggle = document.querySelector('.button-grey.flip-toggle');
    const cardCheckbox = document.querySelector('.card-container .flip-toggle');

    if (flipToggle && cardCheckbox) {
        flipToggle.addEventListener('click', function (e) {
            e.preventDefault();
            cardCheckbox.checked = !cardCheckbox.checked;
        });
    } else {
        console.warn("Элементы не найдены");
    }
})
