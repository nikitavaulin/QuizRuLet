// Функция для создания модального окна
  function createUniversalModal() {
    if (document.getElementById('universalModal')) return;

    const modal = document.createElement('div');
    modal.className = 'modal fade';
    modal.id = 'universalModal';
    modal.tabIndex = '-1';
    modal.innerHTML = `
      <div class="modal-dialog">
        <div class="modal-content" id="modal-universal">
          <div class="modal-header">
            <h5 class="modal-title" id="modalTitle">Заголовок</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body" id="modalBody">
            Здесь будет текст.
          </div>
          <div class="modal-footer">
            <button type="button" class="btn-next-modal" data-bs-dismiss="modal">Закрыть</button>
          </div>
        </div>
      </div>
    `;

    document.body.appendChild(modal);
    new bootstrap.Modal(modal);
  }

  // Функция для открытия модального окна с нужным заголовком и текстом
  window.showModal = function(title, text) {
    createUniversalModal();

    const modalTitle = document.getElementById('modalTitle');
    const modalBody = document.getElementById('modalBody');
    const modalElement = document.getElementById('universalModal');
    const bsModal = bootstrap.Modal.getInstance(modalElement);

    modalTitle.textContent = title;
    modalBody.textContent = text;

    bsModal.show();
  }