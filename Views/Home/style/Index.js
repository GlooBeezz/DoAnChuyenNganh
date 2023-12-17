document.addEventListener("DOMContentLoaded", function () {
    var slides = document.querySelectorAll(".Fly-cmt");
    var currentSlide = 0;
       
     
    function showSlide(index) {
        slides.forEach(function (slide, i) {
            slide.style.display = i === index ? "block" : "none";
        });
    }

    function nextSlide() {
        currentSlide = (currentSlide + 1) % slides.length;
        showSlide(currentSlide);
    }

    setInterval(nextSlide, 1000); // 1000 milliseconds = 1 second
});

//document.addEventListener("DOMContentLoaded",fu)



document.addEventListener("DOMContentLoaded", function () {
    const images = document.querySelectorAll('.slider-image');
    let currentIndex = 0;

    // Hiển thị ảnh đầu tiên
    images[currentIndex].style.display = 'block';

    // Tự động chuyển ảnh sau mỗi 3 giây
    setInterval(function () {
        images[currentIndex].style.display = 'none';

        currentIndex++;
        if (currentIndex >= images.length) {
            currentIndex = 0;
        }

        images[currentIndex].style.display = 'block';

    }, 3000);
});