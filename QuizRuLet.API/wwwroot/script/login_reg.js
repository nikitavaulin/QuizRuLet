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
    checkError(error);
  }
}

const loginButton = document.querySelector("#submitLogin");
const regButton = document.querySelector("#submitRegister");

if (regButton) {
  regButton.addEventListener("click", async function register(e) {
    
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
      checkError(error);
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
