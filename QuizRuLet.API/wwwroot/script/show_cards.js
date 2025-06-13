document.addEventListener('DOMContentLoaded', function () {
    // Весь твой код внутри
    let cards = [];         // Все карточки         
    let currentIndex = 0;   // Текущая карточка
    let currentCardElement = null;
    const flipButton = document.querySelector('.button-grey');


    flipButton.addEventListener('click', () => {
        currentCardElement.checked = !currentCardElement.checked;
    });


    // Функция загрузки карточек с сервера
    async function loadCards() {
        try {
            const response = await axios.get('/test-cards.json');
            cards = response.data.cards || [];

            if (cards.length > 0) {
                showCard(cards[0]);
            } else {
                document.querySelector('.card-front').textContent = 'Нет карточек для изучения';
                document.querySelector('.card-back').textContent = '';
            }
        } catch (error) {
            showModal('Ошибка', 'Не удалось загрузить карточки');
            console.error('Ошибка загрузки карточек:', error);
        }
    }

    
    // Функция обновления счетчика пройденных карточек
    function updateCounter() {
        const counter = document.querySelector('.number-of-pages');
        counter.textContent = `${currentIndex + 1}/${cards.length}`;
    }
    // Показываем текущую карточку
    function showCard(card) {
        updateCounter()
        const cardContainer = document.querySelector('.card-container');

        // Удаляем предыдущую карточку
        const existingCard = cardContainer.querySelector('.card-flip');
        if (existingCard) {
            existingCard.remove();
        }
        // Создаем новую карточку
        const cardHTML = `
        <label class="card-flip col-5">
        <input type="checkbox" class="flip-toggle" hidden>
        <div class="card-inner">
        <div class="card-front p-3">${card.frontSide}</div>
        <div class="card-back p-3">${card.backSide}</div>
        </div>
        </label>
        `;

        const tempDiv = document.createElement('div');
        tempDiv.innerHTML = cardHTML;
        const newCard = tempDiv.firstElementChild;
        cardContainer.appendChild(newCard);

        // Активируем flip при клике
        const checkbox = newCard.querySelector('.flip-toggle');
        const cardInner = newCard.querySelector('.card-inner');
        if (checkbox && cardInner) {
            checkbox.addEventListener('change', () => {
                cardInner.classList.toggle('flipped', checkbox.checked);
            });
        }
        currentCardElement = newCard.querySelector('.flip-toggle');
        console.log(currentCardElement);
    }

    

        document.querySelector('.button-red').addEventListener('click', async () => {
            if (currentIndex >= cards.length) return;
            const currentCard = cards[currentIndex];
            currentCard.isLearned = false;
            // Отправить
            try {
                // Отправляем обновление (в реальном API)
                // Для теста мы не отправляем, но можем сохранять в консоль
                console.log('Карточка отмечена как не выученная:', currentCard);

                // Переходим к следующей карточке
                currentIndex++;
                if (currentIndex < cards.length) {
                    showCard(cards[currentIndex]);
                } else {
                    window.location.href = "/gratz.html"; 
                }
            } catch (error) {
                showModal('Ошибка', 'Не удалось сохранить изменения');
                console.error('Ошибка при сохранении:', error);
            }
        });


        document.querySelector('.button-green').addEventListener('click', async () => {
            if (currentIndex >= cards.length) return;

            const currentCard = cards[currentIndex];    
            currentCard.isLearned = true;
            // Отправить
            try {
                console.log('Карточка отмечена как выученная:', currentCard);

                currentIndex++;
                if (currentIndex < cards.length) {
                    showCard(cards[currentIndex]);
                } else {
                    window.location.href = "/gratz.html"; 
                }
            } catch (error) {
                showModal('Ошибка', 'Не удалось сохранить изменения');
                console.error('Ошибка при сохранении:', error);
            }
        });


        // Кнопка возврата на прошлую карточку
        document.querySelector('.btn-back').addEventListener('click', async () => {
            if (currentIndex >= cards.length) return;

            const currentCard = cards[currentIndex];


            try {
                console.log('Карточка отмечена как выученная:', currentCard);

                currentIndex--;
                if (currentIndex > -1) {
                    showCard(cards[currentIndex]);
                } else {
                    return; 
                }
            } catch (error) {
                showModal('Ошибка', 'Не удалось сохранить изменения');
                console.error('Ошибка при сохранении:', error);
            }
        })
    





    loadCards();
});