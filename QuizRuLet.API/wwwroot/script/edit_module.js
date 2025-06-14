document.addEventListener('DOMContentLoaded', async function () {
  // 1. достаём ?id=...
  const params = new URLSearchParams(window.location.search);
  const moduleId = params.get('id');

  if (!moduleId) {
    alert('Не передан id модуля');
    location.href = 'index.html';
    return;
  }
  // --- Инициализация Bootstrap Tooltips (если нужны) ---
  var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
  var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl);
  });

  // --- Функция для инициализации логики шагов внутри конкретного модального окна ---
  async function initializeModalSteps(modalElement) {
    const steps = modalElement.querySelectorAll('.step');
    const contents = modalElement.querySelectorAll('.step-content');
    const prevBtn = modalElement.querySelector('.btn-back-modal');
    const nextBtn = modalElement.querySelector('.btn-next-modal');

    // Если элементы для шагов не найдены, значит, это модальное окно без шагов, выходим.
    if (steps.length === 0 || !prevBtn || !nextBtn) {
      return;
    }

    let currentStep = 1; // Всегда начинаем с первого шага для данного модального окна
    let updatePreview;

    async function showStep(step) {
      // Обновляем активные шаги
      steps.forEach(s => {
        s.classList.toggle('active', parseInt(s.dataset.step) === step);
      });

      // Показываем нужный контент
      contents.forEach(c => {
        c.classList.toggle('active', parseInt(c.dataset.step) === step);
      });

      // Управление кнопками
      prevBtn.disabled = step === 1;
      prevBtn.style.display = step === 1 ? 'none' : 'inline-block';

      nextBtn.disabled = false;
      nextBtn.textContent = step === steps.length ? 'Готово' : 'Далее';
      nextBtn.classList.toggle('btn-primary', step < steps.length);
      nextBtn.classList.toggle('btn-success', step === steps.length);

      // --- Дополнительно: вызов updatePreview() только на шаге 3 ---
      if (step === 3 && updatePreview) {
        await updatePreview(); // Вызываем обновление превью, если это шаг 3 и модалка importModal
      }
    }

    async function goToStep(step) {
      if (step < 1 || step > steps.length) return;
      currentStep = step;
      await showStep(currentStep);
    }

    // Обработчики кликов по шагам (для переключения по клику на кружок)
    steps.forEach(stepEl => {
      stepEl.addEventListener('click', () => {
        const targetStep = parseInt(stepEl.dataset.step);
        goToStep(targetStep);
      });
    });

    // Кнопка "Назад"
    prevBtn.addEventListener('click', () => {
      if (currentStep > 1) {
        goToStep(currentStep - 1);
      }
    });

    // Кнопка "Вперед"
    nextBtn.addEventListener('click', () => {
      if (currentStep < steps.length) {
        goToStep(currentStep + 1);
      } else {
        const bsModal = bootstrap.Modal.getInstance(modalElement);
        if (bsModal) {
          bsModal.hide();
        }
        // Или отправить форму
        // modalElement.querySelector('form').submit();
      }
    });

    // Инициализация при первом запуске или сбросе
    showStep(currentStep);


    if (modalElement.id === 'importModal') {
      const importTextarea = modalElement.querySelector('.step-content[data-step="1"] textarea');
      const separatorSelect = modalElement.querySelector('.step-content[data-step="2"] #separator_inline');
      const separatorLinesSelect = modalElement.querySelector('.step-content[data-step="2"] #separator_line');
      const previewPre = modalElement.querySelector('#preview');
      let cards = [];

      updatePreview = async () => {
        let text = importTextarea ? importTextarea.value : '';
        let separator = separatorSelect ? separatorSelect.value : '';
        let separatorLines = separatorLinesSelect ? separatorLinesSelect.value : '';
        let enumerator = 1;

        text = text.replaceAll(separatorLines, "*LINES*");
        text = text.replaceAll(separator, "*PAIR*");
        console.log(text);

        // cards = await axios.post("/test-cards.json", { // Адрес дописать
        //   data: text,
        //   lineSeparator: separatorLines,
        //   pairSeparator: separator
        // })

        const response = await axios.get("/test-cards.json");
        cards = response.data.cards;
        console.log(cards);
        if (true) {

          let previewText = '';
          cards.forEach(card => {
            console.log(card);
            previewText += enumerator + ') ' + card.frontSide.slice(0, 15) + '..' + ' | ' + card.backSide.slice(0, 25) + '..' + '\n';
            enumerator++;
          });
          previewPre.textContent = previewText;
        } else if (previewPre) {
          previewPre.textContent = 'Данные здесь...';
        }

      }

      // if (importTextarea) importTextarea.addEventListener('input', updatePreview);
      // if (separatorSelect) separatorSelect.addEventListener('input', updatePreview);
      // if (separatorLinesSelect) separatorLinesSelect.addEventListener('input', updatePreview);

      // Обновить предпросмотр при переходе на шаг 3 или при открытии модалки (если уже на шаге 3)
      // Это можно привязать к событию showStep, но для простоты примера оставим так.
      // Лучше: обновить предпросмотр, когда `showStep` показывает шаг 3.

      // Также, можно вызывать updatePreview при каждом изменении шага, если это нужно.
    }
  }

  function createNewCard() {

    const frontTextarea = document.getElementById('front');
    const backTextarea = document.getElementById('back');
    const createCardBtn = document.getElementById('createCardBtn');


    createCardBtn.addEventListener('click', async (e) => {


      const frontSide = frontTextarea.value.trim();
      const backSide = backTextarea.value.trim();


      if (!frontSide || !backSide) {
        alert('Пожалуйста, заполните обе стороны карточки.');
        return;
      }
      try {

        const response = await axios.post(`/cards/${moduleId}`, {
          frontSide,
          backSide,
        });


        if (response.status === 200) {

          const modal = bootstrap.Modal.getInstance(document.getElementById('newCard'));
          modal.hide();


          showModal("Сообщение", "Карточка успешно создана!");
          fetchCards();
        } else {
          throw new Error('Ошибка при создании карточки');
        }
      } catch (error) {

        console.error('Error creating card:', error);
        alert('Произошла ошибка при создании карточки. Попробуйте снова.');
      }
    });
  }

  // --- Инициализация всех модальных окон при загрузке страницы ---
  document.querySelectorAll('.modal').forEach(modalElement => {
    // Каждый раз, когда модальное окно открывается, инициализируем его логику шагов
    // Это гарантирует, что оно всегда начинается с первого шага
    modalElement.addEventListener('show.bs.modal', function () {
      initializeModalSteps(modalElement);
    });
  });

  // Инициализация переменных

  const cardListElement = document.querySelector('.sidebarList ul.nav.flex-column'); // Левый блок для списка карточек
  const cardDetailsElement = document.querySelector('.card-show'); // Правый блок для деталей карточки

  // Функция для получения списка карточек
  async function fetchCards() {
    try {
      const response = await axios.get(`/modules/${moduleId}`);
      console.log(response);
      if (response.status === 200) {
        const moduleData = response.data;
        renderCardList(moduleData.cards);
        updateModuleName(response.data.name, response.data.description);
      } else {
        throw new Error('Ошибка при получении списка карточек');
      }
    } catch (error) {
      console.error('Ошибка:', error);
      alert('Произошла ошибка при загрузке карточек.');
    }
  }

  // Функция для отображения списка карточек
  function renderCardList(cards) {


    cardListElement.innerHTML = '';

    // Создаем кнопку "Создать новую карточку"
    const createCardButton = document.createElement('li');
    createCardButton.classList.add('card', 'm-2'); // Добавляем стилизацию

    const createCardBody = document.createElement('div');
    createCardBody.classList.add('card-body'); // Используем существующий класс для стилизации
    createCardBody.textContent = 'Создать новую карточку'; // Текст кнопки
    createCardBody.setAttribute('data-bs-toggle', 'modal'); // Атрибуты для открытия модального окна
    createCardBody.setAttribute('data-bs-target', '#newCard');

    createCardButton.appendChild(createCardBody); // Добавляем div внутрь li
    cardListElement.appendChild(createCardButton); // Добавляем кнопку в начало списка

    // Добавляем остальные карточки
    cards.forEach(card => {
      const listItem = document.createElement('li');
      listItem.classList.add('card', 'm-2'); // Используем существующие классы для стилизации

      const cardBody = document.createElement('div');
      cardBody.classList.add('card-body'); // Добавляем класс для стилизации
      cardBody.textContent = card.frontSide; // Отображаем переднюю сторону как название
      cardBody.dataset.cardId = card.id; // Сохраняем ID карточки для дальнейшего использования

      // Обработчик клика на карточку
      cardBody.addEventListener('click', () => {
        renderCardDetails(card.frontSide, card.backSide);
      });

      listItem.appendChild(cardBody); // Добавляем div внутрь li
      cardListElement.appendChild(listItem); // Добавляем карточку в список
    });
  }

  function updateModuleName(name, desc) {
    let moduleName = document.getElementById('module-name');
    moduleName.setAttribute('data-bs-original-title', desc);
    moduleName.innerHTML = `${name}
              <i
                class="fa-solid fa-pen ms-2"
                data-bs-toggle="modal"
                data-bs-target="#editModal"
              ></i>`;

    let nameModal = document.getElementById('moduleName');
    let descriptionModal = document.getElementById('moduleDescription');
    nameModal.value = name;
    descriptionModal.value = desc;
    console.log(nameModal, descriptionModal);
  }

  // Функция для отображения деталей карточки в правом блоке
  function renderCardDetails(front, back) {
    const termTextarea = document.querySelector('.card-termin .card-txt');
    const meaningTextarea = document.querySelector('.card-meaning .card-txt');

    // Заполняем текстовые поля в правом блоке
    termTextarea.value = front;
    meaningTextarea.value = back;

    // Если нужно, добавляем дополнительные детали (например, выучена или нет)
    console.log('Карточка загружена:', card);
  }



  document.getElementById('saveModuleBtn').addEventListener('click', async function () {
    const name = document.getElementById('moduleName').value.trim();
    const description = document.getElementById('moduleDescription').value.trim();

    // Простая валидация
    if (!name || !description) {
      showModal('Ошибка', 'Пожалуйста, заполните все поля');
      return;
    }



    try {
      const response = await axios.patch(`/modules/edit/${moduleId}`, {
        name: name,
        description: description
      });
      console.log(response);

      if (response.status === 200) {
        const modalEl = document.getElementById('editModal');
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide(); // Закрываем модальное окно
        updateModuleName(name, description);

        // Здесь можно обновить интерфейс или вызвать callback
      }
    } catch (error) {
      console.error('Ошибка при сохранении:', error);
      alert('Не удалось сохранить изменения');
    }
  });




  // Инициализация при загрузке страницы

  await fetchCards(); // Получить и отобразить список карточек
  createNewCard();

});