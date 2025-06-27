document.addEventListener('DOMContentLoaded', async function () {
  // 1. достаём ?id=...
  const params = new URLSearchParams(window.location.search);
  const moduleId = params.get('id');

  if (!moduleId) {
    showModal("Ошибка", 'Не передан id модуля');
    location.href = 'index.html';
    return;
  }

  function isInteger(str) {
    if (!str.trim()) return false;

    const num = Number(str);

    return Number.isInteger(num);
  }


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

    let cards = [];
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


      if (step === 3 && updatePreview) {
        const previewPre = modalElement.querySelector('#preview');
        if (previewPre) {
          previewPre.innerHTML = '<i class="fa-solid fa-hourglass-start"></i> Загрузка...';
        }
        await updatePreview();
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


    const handlePrevClick = () => {
      if (currentStep > 1) {
        goToStep(currentStep - 1);
      }
    };

    const handleNextClick = async () => {
      if (currentStep < steps.length) {
        goToStep(currentStep + 1);
      } else {
        const bsModal = bootstrap.Modal.getInstance(modalElement);
        if (bsModal) {
          bsModal.hide();
        }

        console.log(cards);
        const response = await axios.post(`/import/save/${moduleId}`, { cards: cards });
        try {
          if (response.status === 200) {
            showModal("Сообщение", "Карточки успешно сохранены");
            fetchCards();
          }
        }
        catch {
          checkError(error);
          return;
        }
      }
    };
    prevBtn.addEventListener('click', handlePrevClick);
    nextBtn.addEventListener('click', handleNextClick);

    modalElement.addEventListener('hidden.bs.modal', () => {
      prevBtn.removeEventListener('click', handlePrevClick);
      nextBtn.removeEventListener('click', handleNextClick);

      // Очищаем поля ввода, чтобы модальное окно было "чистым" при следующем открытии
      modalElement.querySelectorAll('textarea, input').forEach(input => input.value = '');
      const preview = modalElement.querySelector('#preview');
      if (preview) {
        preview.textContent = 'Данные здесь...';
      }
    }, { once: true });

    // Инициализация при первом запуске или сбросе
    showStep(currentStep);

    // Обработка данных для импорта
    if (modalElement.id === 'importModal') {
      const importTextarea = modalElement.querySelector('.step-content[data-step="1"] textarea');
      const separatorSelect = modalElement.querySelector('.step-content[data-step="2"] #separator_inline');
      const separatorLinesSelect = modalElement.querySelector('.step-content[data-step="2"] #separator_line');
      const previewPre = modalElement.querySelector('#preview');


      updatePreview = async () => {
        let text = importTextarea ? importTextarea.value : '';
        let separator = separatorSelect.value;
        let separatorLines = separatorLinesSelect.value;

        if (separatorLines.length === 0 || separator.length === 0) { showModal("Ошибка", "Выберите все разделители") }

        text = text.replace(/\t/g, "*PAIR*");
        text = text.replace(/\r?\n/g, "*LINES*");
        text = text.replaceAll(separatorLines, "*LINES*");
        text = text.replaceAll(separator, "*PAIR*");
        console.log(text);
        try {
          const response = await axios.post("/import", { // Адрес дописать
            data: text,
            lineSeparator: "*LINES*",
            pairSeparator: "*PAIR*"
          })
          const result = separatingPreview(response);
          if (result) {
            previewPre.textContent = result.previewText;
            cards = result.parsedCards; // <-- ГЛАВНЫЙ ФИКС
          } else {
            previewPre.textContent = "Не удалось обработать данные.";
            cards = [];
          }
        }
        catch (error) {
          checkError(error);
        }
      }
    }
    if (modalElement.id === 'importAiModal') {
      const importTextarea = modalElement.querySelector('.step-content[data-step="1"] textarea');
      const countArea = modalElement.querySelector('.step-content[data-step="2"] #count');
      const previewPre = modalElement.querySelector('#preview');


      updatePreview = async () => {
        let text = importTextarea ? importTextarea.value : '';
        let count = countArea ? countArea.value : '';

        if (!isInteger(count)) {
          showModal("Ошибка", "Вы ввели не целое число");
          return;
        }

        if (text.length > 2500) {
          showModal("Ошибка", "Длина конспекта не должна превышать 2500 символов");
          return;
        }
        
        text = text.replace(/\r?\n/g, " ");
        text = text.replaceAll("\n", " ");
        try {
          const response = await axios.post("/import/ai", {
            data: text,
            countCards: count
          })

          const result = separatingPreview(response);
          if (result) {
            previewPre.textContent = result.previewText;
            cards = result.parsedCards;
          } else {

            previewPre.textContent = "Ошибка обработки данных.";
            cards = [];
          }
        }
        catch (error) {
          checkError(error);
        }
        
      }


    }
  }

  function separatingPreview(response) {
    try {
      let enumerator = 1;
      let previewText = '';
      if (response.status === 200) {
        const parsedCards = response.data;

        parsedCards.forEach(card => {
          const front = card.frontSide.length > 15 ? card.frontSide.slice(0, 15) + '..' : card.frontSide;
          const back = card.backSide.length > 15 ? card.backSide.slice(0, 15) + '..' : card.backSide;
          previewText += `${enumerator}) ${front} | ${back}\n`;
          enumerator++;
        });


        return {
          previewText: previewText,
          parsedCards: parsedCards
        };


      }
    }
    catch (error) {
      checkError(error);
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
        checkError(error);
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

      checkError(error);
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
      
      cardBody.textContent = card.frontSide.length > 15 ? card.frontSide.slice(0, 15) + '...' : card.frontSide; // Отображаем переднюю сторону как название
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
    
    moduleName.innerHTML = `<i class="fa-solid fa-bars"></i><div class="name-text" data-bs-toggle="tooltip" data-bs-placement="top" title="${desc}">${name}</div>
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
      checkError(error);
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
      checkError(error);
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
      checkError(error);
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



  const importModal = document.getElementById('importModal');
  const aiImportModal = document.getElementById('importAiModal');
  const textarea = document.getElementById('textArea');
  const aiTextarea = document.getElementById('aiTextArea');

  importModal.addEventListener('hidden.bs.modal', function () {
    textarea.value = '';
    currentStep = 1;
  })
  aiImportModal.addEventListener('hidden.bs.modal', function () {
    aiTextarea.value = '';
    currentStep = 1;
  })


  updateSideBarSelected();



  const sidebarToggle = document.querySelector('.fa-bars');
  const sidebar = document.getElementById('sidebarMenu');
  const mainContent = document.querySelector('main');

  if (sidebarToggle && sidebar) {
    sidebarToggle.addEventListener('click', function (event) {
      // Предотвращаем "всплытие" события, чтобы клик по кнопке
      // не вызывал немедленное закрытие сайдбара.
      event.stopPropagation();
      sidebar.classList.toggle('show');
    });
  }
  
  // Дополнительно: закрывать сайдбар при клике вне его области
  document.addEventListener('click', function(event) {
    // Проверяем, открыт ли сайдбар и был ли клик вне сайдбара и вне кнопки
    const isClickInsideSidebar = sidebar.contains(event.target);
    const isClickOnToggler = sidebarToggle.contains(event.target);

    if (sidebar.classList.contains('show') && !isClickInsideSidebar && !isClickOnToggler) {
        sidebar.classList.remove('show');
    }
  });
});