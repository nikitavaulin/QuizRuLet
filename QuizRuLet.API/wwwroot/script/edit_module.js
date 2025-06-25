document.addEventListener('DOMContentLoaded', async function () {
  // 1. достаём ?id=...
  const params = new URLSearchParams(window.location.search);
  const moduleId = params.get('id');

  if (!moduleId) {
    showModal("Ошибка", 'Не передан id модуля');
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
      let cards;
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
    nextBtn.addEventListener('click', async () => {
      if (currentStep < steps.length) {
        goToStep(currentStep + 1);
      } else {
        const bsModal = bootstrap.Modal.getInstance(modalElement);
        if (bsModal) {
          bsModal.hide();
        }

        const response = await axios.post(`/import/save/${moduleId}`, { cards: cards });
        try {
          if (response.status === 200) {
            showModal("Сообщение", "Карточки успешно сохранены");
            fetchCards();
          }
        }
        catch {
          showModal("Ошибка", "Не удалось сохранить карточки в модуле");
          return;
        }
        // Или отправить форму
        // modalElement.querySelector('form').submit();
      }
    });

    // Инициализация при первом запуске или сбросе
    showStep(currentStep);

    // Обработка данных для импорта
    if (modalElement.id === 'importModal') {
      const importTextarea = modalElement.querySelector('.step-content[data-step="1"] textarea');
      const separatorSelect = modalElement.querySelector('.step-content[data-step="2"] #separator_inline');
      const separatorLinesSelect = modalElement.querySelector('.step-content[data-step="2"] #separator_line');
      const previewPre = modalElement.querySelector('#preview');
      cards = [];

      updatePreview = async () => {
        let text = importTextarea ? importTextarea.value : '';
        let separator = separatorSelect.value;
        let separatorLines = separatorLinesSelect.value;

        if (separatorLines.length === 0 || separator.length === 0) { showModal("Ошибка", "Выберите все разделители") }

       


        text = text.replace(/\r?\n/g, "*LINES*");


        text = text.replaceAll(separatorLines, "*LINES*");


        text = text.replaceAll(separator, "*PAIR*");
        try {
          const response = await axios.post("/import", { // Адрес дописать
            data: text,
            lineSeparator: "*LINES*",
            pairSeparator: "*PAIR*"
          })
          text = [];
          const previewText = separatingPreview(response);
          previewPre.textContent = previewText;
        }
        catch (error) {
          if (error.response) {
            // Сервер вернул ответ (например, 400, 409 и т.д.)
            const { status, data } = error.response;
            if (status === 400 || status === 409) {
              showModal(
                "Ошибка",
                typeof data === "string" ? data + "\nПопробуйте выбрать оба разделителя" : JSON.stringify(data) + "\nПопробуйте выбрать оба разделителя"
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
      }
    }
    if (modalElement.id === 'importAiModal') {
      const importTextarea = modalElement.querySelector('.step-content[data-step="1"] textarea');
      const countArea = modalElement.querySelector('.step-content[data-step="2"] #count');

      const previewPre = modalElement.querySelector('#preview');
      cards = [];

      updatePreview = async () => {
        let text = importTextarea ? importTextarea.value : '';
        let count = countArea ? countArea.value : '';

        let previewText = "";
        text = text.replaceAll("\n", " ");
        try {
          const response = await axios.post("/import/ai", { // Адрес дописать
            data: text,
            countCards: count
          })

          previewText = separatingPreview(response);
        }
        catch (error) {
          if (error.response) {
            // Сервер вернул ответ (например, 400, 409 и т.д.)
            const { status, data } = error.response;
            if (status === 400 || status === 409) {
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
        previewPre.textContent = previewText;
      }


    }
  }

  function separatingPreview(response) {
    try {
      let enumerator = 1;
      let previewText = '';
      if (response.status === 200) {
        cards = response.data;

        if (true) {

          cards.forEach(card => {
            card.frontSide.length > 15 ? card.frontSide = card.frontSide.slice(0, 15) + '..' : card.frontSide;
            card.backSide.length > 15 ? card.backSide = card.backSide.slice(0, 15) + '..' : card.backSide;
            previewText += enumerator + ') ' + card.frontSide + ' | ' + card.backSide + '\n';
            enumerator++;
          });
        } else if (previewPre) {
          previewPre.textContent = 'Данные здесь...';
        }
        cards = [];
        return previewText;
      }
    }
    catch (error) {
      if (error.response) {
        // Сервер вернул ответ (например, 400, 409 и т.д.)
        const { status, data } = error.response;
        if (status === 400 || status === 409) {
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
      return;
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
        showModal("Ошибка", 'Пожалуйста, заполните обе стороны карточки.');
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
        if (error.response) {
          // Сервер вернул ответ (например, 400, 409 и т.д.)
          const { status, data } = error.response;
          if (status === 400 || status === 409) {
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

      if (response.status === 200) {
        const moduleData = response.data;
        renderCardList(moduleData.cards);
        updateModuleName(response.data.name, response.data.description);
      } else {
        throw new Error('Ошибка при получении списка карточек');
      }
    } catch (error) {

      showModal("Ошибка", 'Произошла ошибка при загрузке карточек.');
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
    createCardBody.setAttribute('id', 'createNewCard');


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
        renderCardDetails(card.frontSide, card.backSide, card.id);
      });

      listItem.appendChild(cardBody); // Добавляем div внутрь li
      cardListElement.appendChild(listItem); // Добавляем карточку в список
    });
    updateSideBarSelected();
  }
  // Функция для обновления названия и описания модуля
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

  }

  // Функция для отображения деталей карточки в правом блоке
  function renderCardDetails(front, back, cardId) {
    const termTextarea = document.querySelector('.card-termin .card-txt');
    const meaningTextarea = document.querySelector('.card-meaning .card-txt');

    // Заполняем текстовые поля в правом блоке
    termTextarea.value = front;
    meaningTextarea.value = back;
    termTextarea.removeAttribute('readonly');
    meaningTextarea.removeAttribute('readonly');
    termTextarea.dataset.id = cardId;



  }


  //Сохранение измененных названия и описания
  document.getElementById('saveModuleBtn').addEventListener('click', async function () {
    const name = document.getElementById('moduleName').value.trim();
    const description = document.getElementById('moduleDescription').value.trim();

    // Простая валидация
    if (!name || !description) {
      showModal('Ошибка', 'Пожалуйста, заполните все поля');
      return;
    }

    if (name.length > 50 || name.length < 3) {
      showModal('Ошибка', "Имя модуля должно содержать от 3 до 50 символов");
      return;
    }

    if (description.length > 350) {
      showModal('Ошибка', "Описание модуля должно содержать не более 350 символов");
      return;
    }

    try {
      const responseModal = await axios.patch(`/modules/edit/${moduleId}`, {
        name: name,
        description: description
      });


      if (responseModal.status === 200) {
        const modalEl = document.getElementById('editModal');

        const modal = bootstrap.Modal.getInstance(modalEl);
        modal.hide();
        updateModuleName(name, description);


      }
    } catch (error) {

      if (error.response) {
        // Сервер вернул ответ (например, 400, 409 и т.д.)
        const { status, data } = error.response;
        if (status === 400 || status === 409) {
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
  });

  const deleteBtn = document.getElementById("delete-card");
  const saveBtn = document.getElementById("save-card");


  deleteBtn.addEventListener('click', async function (e) { // Кнопка удаления внизу страницы
    e.preventDefault();
    const cardTxt = document.querySelector('.card-termin .card-txt')

    const cardId = cardTxt.dataset.id;

    try {
      const response = await axios.delete(`/cards/${cardId}`);
      if (response.status === 200) {
        showModal("Сообщение", "Карточка удалена");
      }
      fetchCards();
    }
    catch (error) {
      if (error.response) {
        // Сервер вернул ответ (например, 400, 409 и т.д.)
        const { status, data } = error.response;
        if (status === 400 || status === 409) {
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
  })

  saveBtn.addEventListener('click', async function (e) { // Кнопка сохранения внизу страницы
    e.preventDefault();
    const cardTxtTermin = document.querySelector('.card-termin .card-txt');
    const cardTxtMeaning = document.querySelector('.card-meaning .card-txt');
    const cardTermin = cardTxtTermin.value.trim();
    const cardMeaning = cardTxtMeaning.value.trim();

    const cardId = cardTxtTermin.dataset.id;

    try {
      const response = await axios.patch(`/cards/update/${cardId}`, {
        frontSide: cardTermin,
        backSide: cardMeaning
      });
      if (response.status === 200) {
        showModal("Сообщение", "Карточка изменена");
      }
      fetchCards();
    }
    catch (error) {
      if (error.response) {
        // Сервер вернул ответ (например, 400, 409 и т.д.)
        const { status, data } = error.response;
        if (status === 400 || status === 409) {
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

  })

  // Инициализация при загрузке страницы

  await fetchCards(); // Получить и отобразить список карточек
  createNewCard();

  const learnBtn = document.getElementById('btn-learn');

  learnBtn.addEventListener('click', function (e) {
    window.location.href = `/card.html?id=${encodeURIComponent(moduleId)}`;
  })

  // Выделение выбранной карточки в сайдбаре
  function updateSideBarSelected() {
    document.querySelectorAll('.card').forEach(card => {
      card.addEventListener('click', function (event) {
        // Убираем выделение у всех карточек
        document.querySelectorAll('.card').forEach(c => c.classList.remove('selected'));
        // Добавляем класс выбранной карточке
        if (event.target.id !== "createNewCard")
          card.classList.add('selected');
      });
    });
  }

  updateSideBarSelected();
});