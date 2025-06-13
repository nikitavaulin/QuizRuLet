document.addEventListener('DOMContentLoaded', function () {

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
            previewText += enumerator + ') ' + card.frontSide.slice(0,15)+'..' + ' | ' + card.backSide.slice(0,25)+'..' + '\n';
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

  // --- Инициализация всех модальных окон при загрузке страницы ---
  document.querySelectorAll('.modal').forEach(modalElement => {
    // Каждый раз, когда модальное окно открывается, инициализируем его логику шагов
    // Это гарантирует, что оно всегда начинается с первого шага
    modalElement.addEventListener('show.bs.modal', function () {
      initializeModalSteps(modalElement);
    });
  });

});