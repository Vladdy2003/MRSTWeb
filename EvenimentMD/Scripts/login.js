document.getElementById('togglePassword').addEventListener('click', function (e) {
    const passwordInput = document.getElementById('password');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
    this.classList.toggle('fa-eye-slash');
});

document.getElementById('registerLink').addEventListener('click', function () {
    document.getElementById('loginForm').style.display = 'none';
    document.getElementById('registerForm').style.display = 'block';
});

document.getElementById('loginLink').addEventListener('click', function () {
    document.getElementById('registerForm').style.display = 'none';
    document.getElementById('loginForm').style.display = 'block';
});

document.addEventListener('DOMContentLoaded', function () {
    // Inițializează telefonul cu preferințele pentru România
    const phoneInput = document.querySelector("#phone");
    const iti = window.intlTelInput(phoneInput, {
        initialCountry: "md", // Moldova ca țară implicită
        preferredCountries: ["md", "ro", "ru"], // Țările preferate
        separateDialCode: true, // Afișează prefixul separat
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js"
    });

    // Validare înainte de submit
    document.getElementById('registerForm').addEventListener('submit', function (e) {
        if (!iti.isValidNumber()) {
            e.preventDefault();
            alert("Te rugăm să introduci un număr de telefon valid!");
            return false;
        }

        // Actualizează valoarea numărului de telefon să includă prefixul
        phoneInput.value = iti.getNumber();
    });
});