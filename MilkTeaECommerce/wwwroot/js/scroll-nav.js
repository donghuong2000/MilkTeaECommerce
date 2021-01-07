const navBar = document.getElementById("nav");

window.addEventListener("scroll", function () {
    const scrollHeight = window.pageYOffset;
    const navHeight = navBar.getBoundingClientRect().height;
    if (scrollHeight > 1) {
        navBar.classList.add("fixed-nav");
    } else {
        navBar.classList.remove("fixed-nav");
    }
});
