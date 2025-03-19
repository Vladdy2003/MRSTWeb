document.getElementById('togglePassword').addEventListener('click', function (e) {
    const passwordInput = document.getElementById('password');
    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
    this.classList.toggle('fa-eye-slash');
});

document.getElementById('registerLink').addEventListener('click', function () {
    const formTitle = document.getElementById('formTitle');
    formTitle.textContent = 'Înregistrare';
});