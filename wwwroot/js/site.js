const eyeIcon = document.getElementById("eyeIcon");
const eyeSlashIcon = document.getElementById("eyeSlashIcon");
const passwordInput = document.getElementById("passwordInput");

const defaultTheme = document.getElementById("defaultTheme");
const blueTheme = document.getElementById("blueTheme");
const orangeTheme = document.getElementById("orangeTheme");
const purpleTheme = document.getElementById("purpleTheme");


eyeSlashIcon.addEventListener("click", () => {
    eyeSlashIcon.classList.add("visually-hidden");
    eyeIcon.classList.remove("visually-hidden");
    passwordInput.setAttribute("type", "text");
});

eyeIcon.addEventListener("click", () => {
    eyeSlashIcon.classList.remove("visually-hidden");
    eyeIcon.classList.add("visually-hidden");
    passwordInput.setAttribute("type", "password");
});

function changeToLight() {
    const app = document.getElementById("appp");
    app.dataset.bsTheme = "light";

    localStorage.setItem("theme", app.dataset.bsTheme);
}

function changeToDark() {
    const app = document.getElementById("appp");
    app.dataset.bsTheme = "dark";
    
    localStorage.setItem("theme", app.dataset.bsTheme);
    document.querySelectorAll()
}

function next() {
    document.getElementById('carousel').scrollBy({
        top: 0,
        left: 600,
        behavior: "smooth"
    });
}

function prev() {
    document.getElementById('carousel').scrollBy({
        top: 0,
        left: -600,
        behavior: "smooth"
    });
}
