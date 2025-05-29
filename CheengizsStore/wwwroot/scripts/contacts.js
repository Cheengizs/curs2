document.addEventListener("DOMContentLoaded", () => {
  const form = document.querySelector(".contact-form");

  form.addEventListener("submit", async (event) => {
    event.preventDefault();

    // Сбор данных в объект
    const data = {
      name: form.querySelector('[name="name"]').value.trim(),
      email: form.querySelector('[name="email"]').value.trim(),
      message: form.querySelector('[name="message"]').value.trim(),
    };

    try {
      const response = await fetch(`http://192.168.1.107:5212/api/v1/email`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (!response.ok) {
        throw new Error(`Ошибка при отправке: ${response.status}`);
      }

      alert("Сообщение отправлено успешно!");
      form.reset();
    } catch (error) {
      console.error("Ошибка:", error);
      alert("Ошибка при отправке сообщения. Повторите позже.");
    }
  });
});
