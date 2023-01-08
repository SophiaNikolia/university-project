const eyeIcon = document.getElementById("eyeIcon");
const eyeSlashIcon = document.getElementById("eyeSlashIcon");
const passwordInput = document.getElementById("passwordInput");

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
