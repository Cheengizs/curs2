function renderProduct(product) {
  const mainImage = document.querySelector(".product-main-image img");
  const thumbnailsContainer = document.querySelector(".product-thumbnails");
  const thumbnails = document.querySelector(".colorway-thumbnails");

  const nameEl = document.querySelector(".product-name");
  const priceEl = document.querySelector(".product-price");

  const details = {
    type: document.querySelector(".detail-type"),
    colorway: document.querySelector(".detail-colorway"),
    colors: document.querySelector(".detail-colors"),
    materials: document.querySelector(".detail-materials"),
    releaseYear: document.querySelector(".detail-release-year"),
    currentlyMade: document.querySelector(".detail-currently-made"),
    country: document.querySelector(".detail-country"),
  };

  const sizesList = document.querySelector(".sizes-list");
  const commentsList = document.querySelector(".comments-list");
  const commentForm = document.querySelector(".comment-form");

  // Устанавливаем основные данные
  mainImage.src = product.photos[0];
  mainImage.alt = product.name;
  nameEl.textContent = product.name;
  priceEl.textContent = product.price + " руб.";

  details.type.textContent = product.type;
  details.colorway.textContent = product.coloration;
  details.colors.textContent = product.colors.join(", ");
  details.materials.textContent = product.materials.join(", ");
  details.releaseYear.textContent = product.year;
  details.currentlyMade.textContent = product.currentlyMade ? "Да" : "Нет";
  details.country.textContent = product.country;

  thumbnails.innerHTML = "";
  product.other.forEach((elem, idx) => {
    const newThumb = document.createElement("img");
    newThumb.src = elem.photoPath;
    newThumb.addEventListener("click", () => {
      window.location.href = `/product.html?productId=${elem.id}`; // раскомментировать для реального перехода
    });

    thumbnails.appendChild(newThumb);
  });

  // Очистка и создание миниатюр
  thumbnailsContainer.innerHTML = "";
  product.photos.forEach((photo, idx) => {
    const thumb = document.createElement("img");
    thumb.src = photo;
    if (idx === 0) thumb.classList.add("active");

    thumb.addEventListener("click", () => {
      mainImage.style.opacity = 0;
      setTimeout(() => {
        mainImage.src = photo;
        mainImage.alt = thumb.alt;
        mainImage.style.opacity = 1;
      }, 200);

      thumbnailsContainer
        .querySelectorAll("img")
        .forEach((img) => img.classList.remove("active"));
      thumb.classList.add("active");
    });

    thumbnailsContainer.appendChild(thumb);
  });

  // Размеры
  sizesList.innerHTML = "";
  product.sizes.forEach((size) => {
    const container = document.createElement("div");
    container.className = "tooltip-container";

    const btn = document.createElement("button");
    btn.textContent = size.rus_size;
    btn.className = "size-btn";
    btn.addEventListener("click", () => {
      sizesList
        .querySelectorAll("button")
        .forEach((b) => b.classList.remove("selected"));
      btn.classList.add("selected");
      console.log(`Выбран размер: ${size.rus_size}`);
      order["sizeId"] = size.id;
    });

    const tooltip = document.createElement("span");
    tooltip.className = "tooltip-text";
    tooltip.textContent = `US: ${size.us_size} UK:${size.uk_size}\n Осталось${size.amount}`;

    container.appendChild(btn);
    container.appendChild(tooltip);
    sizesList.appendChild(container);
  });

  // Комментарии (заглушка)
  commentsList.innerHTML = "";

  // Отправка комментария
  commentForm.addEventListener("submit", (e) => {
    e.preventDefault();
    const textarea = commentForm.querySelector("textarea");
    const text = textarea.value.trim();
    if (!text) return;

    // Добавляем новый комментарий локально (для примера)
    const newComment = document.createElement("div");
    newComment.className = "comment";
    newComment.innerHTML = `<strong>Вы:</strong> <p>${text}</p>`;
    commentsList.appendChild(newComment);

    textarea.value = "";
    commentsList.scrollTop = commentsList.scrollHeight;

    // Здесь можно добавить отправку комментария на сервер
    console.log("Отправлен комментарий:", text);
  });
}

async function getSneakerById(api) {
  const responce = await fetch(api);
  if (!responce.ok) {
    throw new Error();
  }

  const data = await responce.json();
  return data;
}

const params = new URLSearchParams(window.location.search);
const id = params.get("productId");

let order = { sizeId: NaN, sneakerColorId: id, amount: 1 };

async function loadProduct() {
  try {
    const product = await getSneakerById(
      `http://192.168.1.107:5212/api/v1/catalog/${id}`
    );
    renderProduct(product);
  } catch (error) {
    console.error(error);
    // Можно показать ошибку пользователю
  }
}

loadProduct();

document.addEventListener("DOMContentLoaded", () => {
  const qtyInput = document.querySelector(".qty-input");
  const btnMinus = document.querySelector(".qty-btn.minus");
  const btnPlus = document.querySelector(".qty-btn.plus");

  btnMinus.addEventListener("click", () => {
    let value = parseInt(qtyInput.value) || 1;
    if (value > 1) qtyInput.value = value - 1;
    order["amount"] = qtyInput.value;
  });

  btnPlus.addEventListener("click", () => {
    let value = parseInt(qtyInput.value) || 1;
    qtyInput.value = value + 1;
    order["amount"] = qtyInput.value;
  });
});

const addToCart = document.querySelector(".add-to-cart-btn");
addToCart.addEventListener("click", async () => {
  console.log(order);
  if (Number.isNaN(order["sizeId"])) {
    showToast("Выберите размер для добавления в корзину");
    return;
  }
  const token = localStorage.getItem("jwtToken");

  fetch("http://192.168.1.107:5212/api/v1/cart", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`, // Вставляем токен сюда
    },
    body: JSON.stringify(order),
  })
    .then((response) => {
      if (response.status === 401) {
        // Если сервер ответил 401, значит неавторизован, можно перенаправить на страницу логина
        window.location.href = "/account/login.html";
      } else if (!response.ok) {
        showToast("Этот товар уже есть в корзине");
        return response.json().then((err) => {
          console.error("Ошибка:", err);
        });
      } else {
        return response.json().then((data) => {
          showToast("Добавлено в корзину!");
          console.log("Успех:", data);
        });
      }
    })
    .catch((error) => {
      console.error("Ошибка сети:", error);
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
