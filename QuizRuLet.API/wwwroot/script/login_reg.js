

async function loginFunc(userLogin, userPass) {
  const response = await axios.post("/login", {
    login: userLogin,
    password: userPass,
  });

  if (response.status === 200) {
    
    window.location.href = "/index.html";
  } else {
    alert(response.text());
  }
}

const loginButton = document.querySelector("#submitLogin");
const regButton = document.querySelector("#submitRegister");

if (regButton) {
  regButton.addEventListener("click", async function register(e) {
    //Обработать badRequest
    e.preventDefault();

    const login = document.getElementById("loginInput").value.trim();
    const pass = document.getElementById("passwordInput").value.trim();
    const secondPass = document
      .getElementById("passwordConfirmInput")
      .value.trim();

    if (login === "") {
      showModal("Ошибка", "Логин пустой");
      return;
    }

    if (pass !== secondPass) {
      showModal("Ошибка", "Пароли не совпадают");
      return;
    }

    try {
      const response = await axios.post("/register", {
        login: login,
        password: pass,
      });
      if (response.status === 200) {
        await loginFunc(login, pass);
      }
    } catch (error) {
      if (error.response) {
        // Сервер вернул ответ (например, 400, 409 и т.д.)
        const { status, data } = error.response;
        // console.error("Ошибка регистрации:", status, data);

        if (status === 400 || status === 409) {
          showModal(
            "Ошибка",
            typeof data === "string"   ? data : JSON.stringify(data)
          );
        } else {
          showModal("Ошибка", "Неизвестная ошибка сервера");
        }
      } else if (error.request) {
        // Запрос был отправлен, но нет ответа (сетевая ошибка)
        showModal("Ошибка", "Сервер не отвечает");
      } else {
        // Неожиданная JS-ошибка
        showModal("Ошибка", "Произошла ошибка: " + error.message);
      }
    }
  });
}

if (loginButton) {
  loginButton.addEventListener("click", async function (e) {
    e.preventDefault();
    const userLogin = document.querySelector("#loginInput");
    const userPass = document.querySelector("#passwordInput");
    const loginValue = userLogin.value.trim();
    const passValue = userPass.value.trim();

    await loginFunc(loginValue, passValue);

    // await login(userLogin, userPass);
  });
}
