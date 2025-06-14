document.addEventListener('DOMContentLoaded', async function () {
    const params = new URLSearchParams(window.location.search);
    const moduleId = params.get('id');
    const response = await axios.get(`/modules/${moduleId}`);
    const name = response.data.name;
    const desc = response.data.description;
    const progress = response.data.progress;
    const countCards = response.data.countCards;
    const countLearned = response.data.countLearned;
    const countNotLearned = countCards - countLearned;
    const cards = response.data.cards;

    if (!moduleId) {
        alert('Не передан id модуля');
        location.href = 'index.html';
        return;
    }
    const learnBtn = document.getElementById('learn');
    learnBtn.addEventListener('click', function (e) {
        window.location.href = `/card.html?id=${encodeURIComponent(moduleId)}`;
    })

    const namePlace = document.getElementById('moduleTitle');
    namePlace.insertAdjacentText("afterbegin", name);

    const progressBar = `<div class="progress">
                  <div class="progress-fill" style="width: ${progress}%;"></div>
                </div>
                <span class="progress-percent">${progress}%</span>`;
    const progressPlace = document.getElementById('progressPlace');
    progressPlace.insertAdjacentHTML("beforeend", progressBar);

    const toEditBtn = document.getElementById('toEdit');

    toEditBtn.addEventListener('click', function () {
        window.location.href = `/module_edit.html?id=${encodeURIComponent(moduleId)}`;
    })


    const redCountPlace = document.getElementById('unknown-count');
    redCountPlace.insertAdjacentHTML("beforeend", countNotLearned);
    const greenCountPlace = document.getElementById('known-count');
    greenCountPlace.insertAdjacentHTML("beforeend", countLearned);

    const addBtn = document.getElementById('addButton');
    addBtn.addEventListener('click', function () {
        window.location.href = `/module_edit.html?id=${encodeURIComponent(moduleId)}`
    })

    const descriptionPlace = document.getElementById('desc');
    if (desc) {
        descriptionPlace.innerText = desc;
    }

    let cardsLearned = [];
    let cardsNotLearned = [];
    cards.forEach(card => {
        if (card.isLearned) cardsLearned.push(card)
        else cardsNotLearned.push(card)
    });

    const learnedPlace = document.getElementById('learned-list');
    const notLearnedPlace = document.getElementById('not-learned-list');

    function showCardsInFolder(cards, place) {
        let enumerator = 1;
        cards.forEach(card => {
            const cardToPush = `<li class="card-item">
        <span class="card-index">${enumerator}</span>
        <div class="card-content">
          <textarea class="card-term autoresize">${card.frontSide}</textarea><textarea class="card-value autoresize">${card.backSide}</textarea>
        </div>
      </li>`
            enumerator++;
            place.insertAdjacentHTML("beforeend", cardToPush);
        })

    }
showCardsInFolder(cardsLearned, learnedPlace);
showCardsInFolder(cardsNotLearned, notLearnedPlace);

})