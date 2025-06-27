document.addEventListener("DOMContentLoaded", async function (e) {
  // Получение значения cookie по имени
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

  // Извлечение ID пользователя из JWT токена
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

  // Получение ID пользователя из cookie
  function getUserID() {
    const token = getCookie("tasty-cookies");

    let userId = null;
    if (token !== "") {
      userId = getUserIDFromJWT(token);
      return userId;
    }
  }

  // Запрос и отображение списка модулей пользователя
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

  // Обработчик для удаления модуля
  document.body.addEventListener("click", async function (e) {
    const btn = e.target.closest(".delete-btn");
    if (!btn) return;
    const moduleId = btn.dataset.id
      || btn.closest('.module-card')?.dataset.id;

    if (!moduleId) return;
    try {
      await axios.delete(`/modules/${moduleId}`);
      await showListModules();
    } catch (errpr) {
      console.error(err);
      showModal("Ошибка", "Не удалось удалить модуль. Попробуйте ещё раз.");
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
      checkError(error);
    }
  });

  // Функция для отображения одного модуля в списке
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

  // Функция для очистки всех cookies
  function clearAllCookies() {
    // Получаем все доступные куки для текущего домена
    const cookies = document.cookie.split(";");

    // Проходимся по каждому куки и устанавливаем его срок действия в прошлое
    cookies.forEach(cookie => {
      const cookieName = cookie.split("=")[0].trim();
      document.cookie = `${cookieName}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/`;
    });

    console.log("Все куки успешно удалены.");
  }

  const logoutBtn = document.getElementById('logout').addEventListener('click', function (e) {
    e.preventDefault();
    clearAllCookies();
    window.location.href = "login.html";
  })

  // Обработчик отправки формы для создания нового модуля
  const moduleForm = document.getElementById("moduleForm");
  moduleForm.addEventListener("submit", async function (e) {
    e.preventDefault();
    const name = document.getElementById("moduleName").value.trim();
    const desc = document.getElementById("moduleDesc").value.trim();

    try {
      const response = await axios.post("/modules", {
        name: name,
        description: desc,
        userId: userId,
      });
    }
    catch (error) {
      checkError(error);
    }
    await showListModules();

    document.getElementById("moduleForm").reset();
    const modal = bootstrap.Modal.getInstance(
      document.getElementById("createModuleModal")
    );
    modal.hide();
  });

  // Первоначальная загрузка данных пользователя и списка модулей
  const userId = getUserID();
  await showListModules();

  // Обработчик для переключения видимости боковой панели
  const sidebarToggle = document.getElementById("sidebarToggle");
  const sidebar = document.getElementById("sidebarMenu");
  const mainContent = document.querySelector("main");

  if (sidebarToggle && sidebar) {
    sidebarToggle.addEventListener("click", function (event) {
      event.stopPropagation();
      sidebar.classList.toggle("show");
    });
  }

  // Обработчик для закрытия боковой панели при клике вне её
  document.addEventListener("click", function (event) {
    const isClickInsideSidebar = sidebar.contains(event.target);
    const isClickOnToggler = sidebarToggle.contains(event.target);

    if (sidebar.classList.contains('show') && !isClickInsideSidebar && !isClickOnToggler) {
        sidebar.classList.remove('show');
    }
  });
});
