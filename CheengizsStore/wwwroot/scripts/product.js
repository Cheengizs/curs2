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
  //   product.comments.forEach(({ user, text }) => {
  //     const c = document.createElement("div");
  //     c.className = "comment";
  //     c.innerHTML = `<strong>${user}:</strong> <p>${text}</p>`;
  //     commentsList.appendChild(c);
  //   });

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
