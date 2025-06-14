document.addEventListener("DOMContentLoaded", async function (e) {




  function getCookie(name) {
    const cookies = document.cookie.split("; ");
    let decoded = null;
    cookies.forEach((cookie) => {
      const [cookieName, cookieValue] = cookie.split("=");

      if (cookieName === name) {
        decoded = decodeURIComponent(cookieValue);

        return decoded;
      }
    });
    return decoded;
  }

  function getUserIDFromJWT(token) {
    try {
      const parts = token.split(".");
      if (parts.length !== 3) {
        throw new Error("Неверный формат JWT");
      }
      const payloadBase64 = parts[1];
      const payload = JSON.parse(atob(payloadBase64));

      const userId = payload.userId;

      if (!userId) {
        throw new Error("Поле id пользователя не найдено в JWT");
      }
      return userId;
    } catch (error) {
      console.error("Ошибка при извлечении id пользователя из JWT", error);
      return null;
    }
  }

  function getUserID() {
    const token = getCookie("tasty-cookies");

    let userId = null;
    if (token !== "") {
      userId = getUserIDFromJWT(token);
      return userId;
    }
  }

  async function showListModules() {
    if (userId === null) {
      window.location.href = "/login.html";
    }

    const response = await axios.get(`/users/${userId}`);
    const modules = response.data.modules;
    const name = response.data.login;
    
    const username = document.querySelector(".username_text");
    username.innerHTML = "";
    username.insertAdjacentText("beforeend", name);
    const container = document.querySelector(".module-list");
    container.innerHTML = "";
    modules.forEach((element) => {
      showModule(element.name, element.progress, element.id, element.countCards);
    });

  }


  document.body.addEventListener('click', async function (e) {
    const btn = e.target.closest('.delete-btn');
    if (!btn) return;
    const moduleId = btn.dataset.id
      || btn.closest('.module-card')?.dataset.id;

    if (!moduleId) return;
    try {
      await axios.delete(`/modules/${moduleId}`);
      await showListModules();
    } catch (err) {
      console.error(err);
      alert('Не удалось удалить модуль. Попробуйте ещё раз.');
    }

  });



  document.body.addEventListener('click', async function (e) {
    const btn = e.target.closest('.edit-btn');
    if (!btn) return;
    const moduleId = btn.dataset.id
      || btn.closest('.module-card')?.dataset.id;

    if (!moduleId) return;
    try {
      window.location.href = `/module_edit.html?id=${encodeURIComponent(moduleId)}`;

    } catch (err) {
      console.error(err);
      alert('Не удалось удалить модуль. Попробуйте ещё раз.');
    }

  });

  function showModule(name, progress, id, countCards) {
    const container = document.querySelector(".module-list");

    const moduleHTML = `
        <div class="module-card d-flex justify-content-between align-items-center flex-wrap id="${id}"">
        <div class="module-title mb-2 mb-md-0">
        <a class="module-name-link" href="/module.html?id=${encodeURIComponent(id)}">${name}</a>
        </div>
        <div class="module-info d-flex align-items-center mb-2 mb-md-0 me-md-3">
        <span class="me-3">Карточек: ${countCards}</span>
                <div class="progress me-2" style="width: 100px;">
                <div class="progress-bar" role="progressbar" style="width: ${progress}%;" aria-valuenow="${progress}" aria-valuemin="0" aria-valuemax="100"></div>
                </div>
                <span>${progress}%</span>
                </div>
            <div class="dropdown module-actions">
            <button class="btn btn-menu" type="button" data-bs-toggle="dropdown" aria-expanded="false">
            <i class="fas fa-ellipsis-v"></i>
                </button>
                <ul class="dropdown-menu dropdown-menu-end">
                
                <li><a class="dropdown-item edit-btn" href="#" data-id="${id}"><i class="fas fa-edit me-2"></i> Редактировать</a></li>
                <li><hr class="dropdown-divider"></li>
                <li><a class="dropdown-item text-danger delete-btn" href="#" data-id="${id}"><i class="fas fa-trash-alt me-2"></i> Удалить</a></li>
                </ul>
                </div>
                </div>
                `;
    container.insertAdjacentHTML("beforeend", moduleHTML);
  }


  const moduleForm = document.getElementById("moduleForm");
  moduleForm.addEventListener("submit", async function (e) {
    e.preventDefault();
    const name = document.getElementById("moduleName").value.trim();
    const desc = document.getElementById("moduleDesc").value.trim();
    const progress = 0;



    if (name !== "") console.log("!name");

    const response = await axios.post("/modules", {
      name: name,
      description: desc,
      userId: userId,
    });
    await showListModules();

    // Очистить форму и закрыть модалку
    document.getElementById("moduleForm").reset();
    const modal = bootstrap.Modal.getInstance(
      document.getElementById("createModuleModal")
    );
    modal.hide();
  });
  const userId = getUserID();
  await showListModules();
});
