document.addEventListener('DOMContentLoaded', function () {
    const params = new URLSearchParams(window.location.search);
    const moduleId = params.get('id');

    if (!moduleId) {
        alert('Не передан id модуля');
        location.href = 'index.html';
        return;
    }
    console.log(moduleId);
    
    let cards = [];
    let currentIndex = 0;
    let currentCardElement = null;
    const flipButton = document.querySelector('.button-grey');
    let countNotLearned;
    let countLearned;

    flipButton.addEventListener('click', () => {
        currentCardElement.checked = !currentCardElement.checked;
    });
    
    
    // Функция загрузки карточек с сервера
    async function loadCards() {
        try {
            const stat = await axios.get(`modules/statistic/${moduleId}`); 
            countNotLearned = stat.data.countNotLearned;
            countLearned = stat.data.countLearned;
            const response = await axios.get(`/learning-mode/${moduleId}`);
            cards = response.data;
            if (cards.length === 0 && countNotLearned === 0){
                window.location.href = `/gratz.html?id=${encodeURIComponent(moduleId)}`;
            }
            // if (cards.length === 0) {
            //     showModal("Сообщение", "Все карточки пройдены, режим изучения недоступен");
            //     return;
            // }
            if (cards.length > 0) {
                showCard(cards[0]);
            } else {
                document.querySelector('.card-front').textContent = 'Нет карточек для изучения';
                document.querySelector('.card-back').textContent = '';
            }
        } catch (error) {
            checkError(error);
        }
    }


    // Функция обновления счетчика пройденных карточек
    function updateCounter() {
        const counter = document.querySelector('.number-of-pages');
        counter.textContent = `${currentIndex + 1}/${cards.length}`;
        const notLearnedPlace = document.querySelector('.count-not-learned');
        const learnedPlace = document.querySelector('.count-learned');
        notLearnedPlace.textContent = countNotLearned;
        learnedPlace.textContent = countLearned;
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
        <div class="card-inner" data-id="${card.id}" id="card-inner">
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
        
    }



    document.querySelector('.button-red').addEventListener('click', async () => {
        if (currentIndex >= cards.length) return;
        const card_inner = document.getElementById('card-inner');

        let cardId = card_inner.dataset.id;
        const currentCard = cards[currentIndex];
        currentCard.isLearned = false;
        
        await axios.patch(`/cards/update-flag/${cardId}`, { isLearned: false });
        try {

            console.log('Карточка отмечена как не выученная:', currentCard);


            currentIndex++;
            if (currentIndex < cards.length) {
                showCard(cards[currentIndex]);
            } else {
                window.location.href = `/gratz.html?id=${encodeURIComponent(moduleId)}`;
            }
        } catch (error) {
            checkError(error);
        }
    });


    document.querySelector('.button-green').addEventListener('click', async () => {
        if (currentIndex >= cards.length) return;
        const card_inner = document.getElementById('card-inner');

        let cardId = card_inner.dataset.id;

        countLearned++;
        countNotLearned--;
        const currentCard = cards[currentIndex];    //Надо придумать откуда взять cardId
        currentCard.isLearned = true;
        await axios.patch(`/cards/update-flag/${cardId}`, { isLearned: true });
        try {
            console.log('Карточка отмечена как выученная:', currentCard);

            currentIndex++;
            if (currentIndex < cards.length) {
                showCard(cards[currentIndex]);
            } else {
                window.location.href = `/gratz.html?id=${encodeURIComponent(moduleId)}`;
            }
        } catch (error) {
            checkError(error);
        }
    });

   

   
    

    document.querySelector('.close-btn').addEventListener('click', function () {
        window.location.href = `/module.html?id=${encodeURIComponent(moduleId)}`;
    } )




    loadCards();
});