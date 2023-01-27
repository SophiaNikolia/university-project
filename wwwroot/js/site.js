const eyeIcon = document.getElementById("eyeIcon");
const eyeSlashIcon = document.getElementById("eyeSlashIcon");
const passwordInput = document.getElementById("passwordInput");

const changeThemeBtn = document.getElementById("changeTheme");

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


// changeThemeBtn.addEventListener("click", () => {

//     const app = document.getElementById("appp");

//     // app.dataset.bsTheme = "dark";

//     console.log("test");

//     (app.dataset.bsTheme == "light") ? dataset.bsTheme = "dark" : dataset.bsTheme = "light";

// });

function change() {
    const app = document.getElementById("appp");
    
    (app.dataset.bsTheme == "light") ? app.dataset.bsTheme = "dark" : app.dataset.bsTheme = "light";
    
    sessionStorage.setItem("theme", app.dataset.bsTheme);

}


function loadTheme() {
    const app = document.getElementById("appp");

    const storedTheme = sessionStorage.getItem("theme")

    app.dataset.bsTheme = storedTheme;
}
