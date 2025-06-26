
function checkError(error) {
    if (error.response) {
        // Сервер вернул ответ (например, 400, 409 и т.д.)
        const { status, data } = error.response;
        if (status === 400) {
            showModal(
                "Ошибка",
                typeof data === "string" ? data : JSON.stringify(data)
            );
        } else {
            showModal("Ошибка", "Неизвестная ошибка сервера");
        }
    } else if (error.request) {

        showModal("Ошибка", "Сервер не отвечает");
    } else {

        showModal("Ошибка", "Произошла ошибка: " + error.message);
    }
}