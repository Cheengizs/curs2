document.addEventListener("DOMContentLoaded", () => {
  const token = localStorage.getItem("jwtToken");

  if (!token) {
    // Если пользователь не авторизован — редирект
    window.location.href = "/account/login.html";
    return;
  }

  fetch("http://192.168.1.107:5212/api/v1/cart", {
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
    .then((items) => renderCart(items))
    .catch((error) => {
      console.error("Ошибка:", error);
      alert("Не удалось загрузить корзину");
    });
});

function renderCart(items) {
  const container = document.querySelector(".cart-items");
  const totalPriceElement = document.querySelector(".total-price");
  let total = 0;

  container.innerHTML = ""; // Очистим содержимое

  items.forEach((item) => {
    total += item.totalPrice;

    const el = document.createElement("div");
    el.className = "cart-item";
    el.innerHTML = `
      <img src="${item.photoPath}" alt="${
      item.nameWithColoration
    }" class="cart-item-img" />
      <div class="cart-item-info">
        <h3>${item.nameWithColoration}</h3>
        <p>Цена за пару: ${item.pricePerPair.toLocaleString()} ₽</p>
        <p>Количество: ${item.amount}</p>
      </div>
      <button class="remove-button" data-id="${
        item.cartId
      }" aria-label="Удалить">
        <i class="fas fa-trash-alt"></i>
      </button>
    `;
    container.appendChild(el);

    document.querySelector(".cart-item-img").addEventListener("click", () => {
      window.location.href = `/product.html?productId=${item.sneakerColorId}`;
    });
  });

  totalPriceElement.textContent = total.toLocaleString() + " ₽";

  document.querySelectorAll(".remove-button").forEach((btn) => {
    const id = btn.getAttribute("data-id");

    btn.addEventListener("click", () => removeFromCart(id));
  });
}

function removeFromCart(cartId) {
  const token = localStorage.getItem("jwtToken");

  fetch(`http://192.168.1.107:5212/api/v1/cart/${cartId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
    .then((res) => {
      if (!res.ok) throw new Error("Ошибка удаления");
      location.reload();
      return res.json();
    })
    .then(() => {})
    .catch((error) => {
      console.error("Удаление не удалось:", error);
    });
}

document.querySelector(".order-button").addEventListener("click", async () => {
  const address = document.querySelector("#delivery-address").value;

  console.log(address);

  if (address === "") {
    console.log("asdfafs");
    showToast("Модник, куда доставлять?");
  } else {
    const token = localStorage.getItem("jwtToken");

    console.log("отправляю");
    fetch(`http://192.168.1.107:5212/api/v1/order/make-order`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: `"address"`,
    })
      .then((res) => {
        if (res.status === 409) {
          showToast("У нас нет столькоо товара, модник");
          return;
        }
        if (!res.ok) {
          showToast("Что-то пошло не так, лучше уйди с сайта, модник");
        }
        // location.reload();
      })
      .catch((error) => {
        console.error("Удаление не удалось:", error);
      });
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
