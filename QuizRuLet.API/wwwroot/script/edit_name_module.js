document.getElementById('saveModuleBtn').addEventListener('click', async function () {
    const name = document.getElementById('moduleName').value.trim();
    const description = document.getElementById('moduleDescription').value.trim();

    // Простая валидация
    if (!name || !description) {
        showModal('Ошибка', 'Пожалуйста, заполните все поля');
        return;
    }

    

    try {
        const response = await axios.put(`/api/modules/${moduleId}`, {
            name,
            description
        });

        if (response.status === 200) {
            const modalEl = document.getElementById('editModuleModal');
            const modal = bootstrap.Modal.getInstance(modalEl);
            modal.hide(); // Закрываем модальное окно

            
            // Здесь можно обновить интерфейс или вызвать callback
        }
    } catch (error) {
        console.error('Ошибка при сохранении:', error);
        alert('Не удалось сохранить изменения');
    }
});