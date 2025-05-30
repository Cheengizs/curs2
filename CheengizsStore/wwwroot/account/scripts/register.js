document.addEventListener("DOMContentLoaded", () => {
  const form = document.querySelector("form");

  form.addEventListener("submit", async (e) => {
    e.preventDefault(); // Остановить обычную отправку

    const captchaResponse = grecaptcha.getResponse();
    if (!captchaResponse) {
      showToast("Пройди капчу, модник!");
      return;
    }

    // Собираем данные в объект
    const data = {
      login: form.username.value.trim(),
      email: form.email.value.trim(),
      password: form.password.value, // Не обрезаем пробелы в пароле
    };

    try {
      const response = await fetch(
        "http://192.168.1.107:5212/api/v1/register",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(data),
        }
      );

      const result = await response.json();

      if (response.ok) {
        showToast("Успешная регистрация, мужик");
        window.location.href = "/account/login.html";
      } else {
        showToast(result.error || `Ошибка регистрации.`);
      }
    } catch (err) {
      console.error(err);
      alert("Ошибка соединения с сервером.");
    }
  });
});

function showToast(message) {
  const toast = document.getElementById("custom-toast");
  toast.textContent = message;
  toast.classList.add("show");

  setTimeout(() => {
    toast.classList.remove("show");
  }, 3000); // Показывается 3 секунды
}
