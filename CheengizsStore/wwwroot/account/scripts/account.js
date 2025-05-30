document.querySelector(".logout-button").addEventListener("click", async () => {
  localStorage.removeItem("jwtToken");
  window.location.href = "/main.html";
});

document.addEventListener("DOMContentLoaded", () => {
  const token = localStorage.getItem("jwtToken");

  if (!token) {
    window.location.href = "/main.html";
  }

  fetch("http://192.168.1.107:5212/api/v1/account", {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
    .then((response) => {
      if (!response.ok) {
        window.location.href = "/account/login.html";
      }
      return response.json();
    })
    .then((items) => {
      document.querySelector(".email-lol").textContent = items.email;
      document.querySelector(".user-name").textContent = items.login;
      document.querySelector(".createdAt-lol").textContent = items.createdAt;
    });
});
