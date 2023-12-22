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

document.addEventListener("DOMContentLoaded",fu)
