

async function loginFunc(userLogin, userPass) {
  try {
    const response = await axios.post("/login", {
      login: userLogin,
      password: userPass,
    });

    if (response.status === 200) {
      window.location.href = "/index.html";
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

    if (login === "" || pass === "" || secondPass === "") {
      showModal("Ошибка", "Заполните все поля");
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

if (loginButton) {
  loginButton.addEventListener("click", async function (e) {
    e.preventDefault();
    const userLogin = document.querySelector("#loginInput");
    const userPass = document.querySelector("#passwordInput");
    const loginValue = userLogin.value.trim();
    const passValue = userPass.value.trim();

    if (loginValue === "" || passValue === "") {
      showModal("Ошибка", "Заполните все поля");
      return;
    }



    await loginFunc(loginValue, passValue);


  });
}
