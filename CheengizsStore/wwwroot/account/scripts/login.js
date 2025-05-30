document.querySelector("form").addEventListener("submit", async (e) => {
  e.preventDefault();

  const login = e.target.username.value;
  const password = e.target.password.value;

  const data = {
    login,
    password,
  };

  try {
    const response = await fetch("http://192.168.1.107:5212/api/v1/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    });

    const result = await response.json();

    if (response.ok && result.token) {
      console.log(result.token);
      localStorage.setItem("jwtToken", result.token);

      showToast();
      window.location.href = "/account/account.html";
    } else {
      alert(result.error || "Ошибка входа. Проверьте данные.");
    }
  } catch (err) {
    console.error("Ошибка:", err);
    alert("Не удалось подключиться к серверу.");
  }
});

function showToast(message) {
  const toast = document.getElementById("custom-toast");
  toast.textContent = message;
  toast.classList.add("show");

  setTimeout(() => {
    toast.classList.remove("show");
  }, 3000); // Показывается 3 секунды
}
