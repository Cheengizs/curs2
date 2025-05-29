const images = [
  "/photos/nike/men/nike_initiator_white_black_main.jpg",
  "/photos/nike/men/nike_initiator_white_black.jpg",
  "/photos/nike/men/nike_initiator_white_black_top.jpg",
];

let currentSlide = 0;
const imgElement = document.getElementById("product-image");
const dots = document.querySelectorAll(".dot");

function updateSlider() {
  // плавно скрываем картинку
  imgElement.style.opacity = 0;

  setTimeout(() => {
    imgElement.src = images[currentSlide]; // меняем изображение
    dots.forEach((dot, i) =>
      dot.classList.toggle("active", i === currentSlide)
    );
    imgElement.style.opacity = 1; // плавно показываем
  }, 500); // длительность совпадает с CSS transition
}

function nextSlide() {
  currentSlide = (currentSlide + 1) % images.length;
  updateSlider();
}

function prevSlide() {
  currentSlide = (currentSlide - 1 + images.length) % images.length;
  updateSlider();
}

// навесить клик по точкам для переключения
dots.forEach((dot, idx) => {
  dot.addEventListener("click", () => {
    currentSlide = idx;
    updateSlider();
  });
});

// инициализация слайдера
updateSlider();
