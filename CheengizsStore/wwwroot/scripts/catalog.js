async function fillBrandListFilter(listToAdd) {
  try {
    const responce = await fetch("http://192.168.1.107:5212/api/v1/brands");
    if (responce.ok) {
      const brandArr = await responce.json();
      brandArr.forEach((element) => {
        const brandToAdd = document.createElement("div");
        brandToAdd.innerText += element.name;
        brandToAdd.classList.add("brand-item");
        brandToAdd.dataset.id = element.id;
        brandToAdd.addEventListener("click", () => {
          brandToAdd.classList.toggle("active");
        });
        listToAdd.appendChild(brandToAdd);
      });
    } else {
      alert(responce.status);
    }
  } catch (error) {
    alert(error);
  }
}

async function fillTypeListFilter(listToAdd) {
  try {
    const responce = await fetch("http://192.168.1.107:5212/api/v1/types");
    if (responce.ok) {
      const typeArr = await responce.json();
      typeArr.forEach((element) => {
        const typeToAdd = document.createElement("div");
        typeToAdd.innerText = element.name;
        typeToAdd.classList.add("type-item");
        typeToAdd.dataset.id = element.id;
        typeToAdd.addEventListener("click", () => {
          typeToAdd.classList.toggle("active");
        });
        listToAdd.appendChild(typeToAdd);
      });
    } else {
      alert(responce.status);
    }
  } catch (error) {
    alert(error);
  }
}

async function fillColorListFilter(listToAdd) {
  try {
    const responce = await fetch("http://192.168.1.107:5212/api/v1/colors");
    if (responce.ok) {
      const colorArr = await responce.json();
      colorArr.forEach((element) => {
        const colorToAdd = document.createElement("div");
        colorToAdd.innerText = element.name;
        colorToAdd.classList.add("color-item");
        colorToAdd.dataset.id = element.id;
        colorToAdd.addEventListener("click", () => {
          colorToAdd.classList.toggle("active");
        });
        listToAdd.appendChild(colorToAdd);
      });
    } else {
      alert(responce.status);
    }
  } catch (error) {
    alert(error);
  }
}

async function fillMaterialListFilter(listToAdd) {
  try {
    const responce = await fetch("http://192.168.1.107:5212/api/v1/materials");
    if (responce.ok) {
      const materialArr = await responce.json();
      materialArr.forEach((element) => {
        const materialToAdd = document.createElement("div");
        materialToAdd.innerText = element.name;
        materialToAdd.classList.add("material-item");
        materialToAdd.dataset.id = element.id;
        materialToAdd.addEventListener("click", () => {
          materialToAdd.classList.toggle("active");
        });
        listToAdd.appendChild(materialToAdd);
      });
    } else {
      alert(responce.status);
    }
  } catch (error) {
    alert(error);
  }
}

let sneakersJson;

async function fetchSneakers(url) {
  try {
    const response = await fetch(url);

    if (!response.ok) {
      throw new Error(`Ошибка: ${response.status}`);
    }

    sneakersJson = await response.json();

    // Предполагается, что сервер возвращает массив нужного формата
    renderSneakers(sneakersJson); // отрисовка карточек
  } catch (error) {
    console.error("Не удалось загрузить кроссовки:", error);
  }
}

const brandList = document.querySelector(".brand-list");
const typeList = document.querySelector(".type-list");
const colorList = document.querySelector(".color-list");
const materialList = document.querySelector(".material-list");

Promise.all([
  fillBrandListFilter(brandList),
  fillTypeListFilter(typeList),
  fillColorListFilter(colorList),
  fillMaterialListFilter(materialList),
]).then(() => {
  filters = getParamsFromUrl();
  applyUrlParamsToFilters(filters);
  console.log(filters);
  const urlWithFilters = "http://192.168.1.107:5212/api/v1/catalog";
  const queryString = buildQueryParams(filters);
  const fullUrl = `${urlWithFilters}?${queryString}`;
  fetchSneakers(fullUrl);
});

document
  .querySelectorAll(".availability-filter .availability-item")
  .forEach((item) => {
    item.addEventListener("click", () => {
      item.classList.toggle("active");
    });
  });

function getSelectedBrandIds() {
  const selected = document.querySelectorAll(".brand-item.active");
  return Array.from(selected).map((el) => el.dataset.id);
}

function getSelectedTypeIds() {
  const selected = document.querySelectorAll(".type-item.active");
  return Array.from(selected).map((el) => el.dataset.id);
}

function getSelectedColorIds() {
  const selected = document.querySelectorAll(".color-item.active");
  return Array.from(selected).map((el) => el.dataset.id);
}

function getSelectedMaterialIds() {
  const selected = document.querySelectorAll(".material-item.active");
  return Array.from(selected).map((el) => el.dataset.id);
}

function buildQueryParams(params) {
  return Object.entries(params)
    .flatMap(([key, value]) => {
      if (Array.isArray(value)) {
        return value.map(
          (val) => `${encodeURIComponent(key)}=${encodeURIComponent(val)}`
        );
      } else {
        return `${encodeURIComponent(key)}=${encodeURIComponent(value)}`;
      }
    })
    .join("&");
}

function getParamsFromUrl() {
  const params = new URLSearchParams(window.location.search);
  const parsed = {};
  for (const [key, value] of params.entries()) {
    if (parsed[key]) {
      parsed[key] = Array.isArray(parsed[key])
        ? [...parsed[key], value]
        : [parsed[key], value];
    } else {
      parsed[key] = value;
    }
  }
  return parsed;
}

function activateFilterItems(className, paramValues) {
  const items = document.querySelectorAll(`.${className}`);
  items.forEach((item) => {
    const id = item.dataset.id;
    if (
      Array.isArray(paramValues) ? paramValues.includes(id) : paramValues === id
    ) {
      item.classList.add("active");
    }
  });
}

function applyUrlParamsToFilters(params) {
  if (params.brandId) activateFilterItems("brand-item", params.brandId);
  if (params.typeId) activateFilterItems("type-item", params.typeId);
  if (params.colorId) activateFilterItems("color-item", params.colorId);
  if (params.materialId)
    activateFilterItems("material-item", params.materialId);

  if (params.minPrice)
    document.querySelector("#price-field-min").value = params.minPrice;
  if (params.maxPrice)
    document.querySelector("#price-field-max").value = params.maxPrice;

  if (params.pageSize)
    document.querySelector("#items-count").value = params.pageSize;
  if (params.orderBy)
    document.querySelector("#sort-select").value = params.orderBy;
  if (params.view) {
    if (params.view !== "grid") {
      activeBtnView.classList.remove("active");
      listBtnView.classList.add("active");
      activeBtnView = listBtnView;
    }
  }

  if (params.inStock === "true") {
    const el = document.querySelector(".availability-item");
    if (el) el.classList.add("active");
  }
}

let filters = {};

function redir() {
  filters = {};

  const inpMinPrice = document.querySelector("#price-field-min");
  if (inpMinPrice.value.trim()) filters["minPrice"] = inpMinPrice.value;
  const inpMaxPrice = document.querySelector("#price-field-max");
  if (inpMaxPrice.value.trim()) filters["maxPrice"] = inpMaxPrice.value;

  filters["brandId"] = getSelectedBrandIds();
  filters["typeId"] = getSelectedTypeIds();
  filters["colorId"] = getSelectedColorIds();
  filters["materialId"] = getSelectedMaterialIds();

  filters["pageSize"] = document.querySelector("#items-count").value;
  filters["orderBy"] = document.querySelector("#sort-select").value;

  const elView = document.querySelector("#grid-btn-view");
  filters["view"] =
    elView && elView.classList.contains("active") ? "grid" : "list";

  const el = document.querySelector(".availability-item");
  filters["inStock"] = el && el.classList.contains("active");

  const urlWithFilters = "http://192.168.1.107:5212/catalog.html";
  const queryString = buildQueryParams(filters);
  const fullUrl = `${urlWithFilters}?${queryString}`;
  window.location.href = fullUrl;
}

document
  .querySelector(".apply-filter-btn")
  .addEventListener("click", () => redir());
document.querySelector(".apply-btn").addEventListener("click", () => redir());

function renderSneakers(sneakers) {
  const catalog = document.querySelector(".catalog");
  catalog.innerHTML = "";

  if (filters["view"] == "grid") {
    sneakers.forEach((sneaker) => {
      const card = document.createElement("div");
      card.className = "sneaker-card";

      card.innerHTML = `
      <div class="sneaker-image">
      <img src="${sneaker.photoPath}" alt="${sneaker.name}" />
      </div>
      <div class="sneaker-info">
      <span class="sneaker-name">${sneaker.name}</span>
      <div class="price-and-button">
      <span class="sneaker-price">${sneaker.price.toLocaleString(
        "ru-RU"
      )} руб.</span>
      <button class="add-to-cart-btn" data-id="${sneaker.id}">+</button>
      </div>
      </div>
      `;

      // Обработчик клика по карточке
      card.addEventListener("click", () => {
        // Например, переход на страницу товара
        console.log(`Переход на страницу товара с id: ${sneaker.id}`);
        window.location.href = `/product.html?productId=${sneaker.id}`; // раскомментировать для реального перехода
      });

      // Отдельный обработчик для кнопки "добавить в корзину"
      const btn = card.querySelector(".add-to-cart-btn");
      btn.addEventListener("click", (event) => {
        event.stopPropagation(); // чтобы клик не сработал на карточку
        console.log(`Добавить в корзину товар с id: ${sneaker.id}`);
        // Здесь логика добавления в корзину
      });
      catalog.appendChild(card);
    });
  } else {
    sneakers.forEach((sneaker) => {
      const item = document.createElement("div");
      item.className = "sneaker-list-item";

      item.innerHTML = `
      <div class="sneaker-list-image">
        <img src="${sneaker.photoPath}" alt="${sneaker.name}" />
      </div>
      <div class="sneaker-list-info">
        <div class="sneaker-list-name">${sneaker.name}</div>
        <div class="sneaker-list-bottom-row">
          <span class="sneaker-list-price">${sneaker.price.toLocaleString(
            "ru-RU"
          )} руб.</span>
          <button class="sneaker-list-add-btn" data-id="${
            sneaker.id
          }">+</button>
        </div>
      </div>
    `;

      // Клик по всей карточке
      item.addEventListener("click", () => {
        console.log(
          `Переход на страницу товара с id: ${sneaker.id}   /product.html?productId=${sneaker.id}`
        );
        window.location.href = `/product.html?productId=${sneaker.id}`; // раскомментировать для реального перехода
      });

      // Клик по кнопке добавления в корзину
      const btn = item.querySelector(".sneaker-list-add-btn");
      btn.addEventListener("click", (event) => {
        event.stopPropagation();
        console.log(`Добавить в корзину товар с id: ${sneaker.id}`);
        // Добавить в корзину
      });

      catalog.appendChild(item);
    });
  }
}

let activeBtnView = document.querySelector("#grid-btn-view");

const gridBtnView = document.querySelector("#grid-btn-view");
gridBtnView.addEventListener("click", async () => {
  if (gridBtnView.id !== activeBtnView.id) {
    activeBtnView.classList.remove("active");
    gridBtnView.classList.add("active");
    activeBtnView = gridBtnView;
  }
});

const listBtnView = document.querySelector("#list-btn-view");
listBtnView.addEventListener("click", async () => {
  if (listBtnView.id !== activeBtnView.id) {
    activeBtnView.classList.remove("active");
    listBtnView.classList.add("active");
    activeBtnView = listBtnView;
  }
});
