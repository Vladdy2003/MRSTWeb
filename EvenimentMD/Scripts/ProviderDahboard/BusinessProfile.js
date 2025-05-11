
// Inițializare TinyMCE
tinymce.init({
    selector: '#description',
    menubar: false,
    plugins: 'lists link image preview',
    toolbar: 'bold italic underline | bullist numlist | undo redo | preview',
    branding: false,
    height: 300
});

document.addEventListener('DOMContentLoaded', function () {
    // Inițializare feather icons
    if (typeof feather !== 'undefined') {
        feather.replace();
    }

    // Funcționalitatea de previzualizare logo
    const logoInput = document.getElementById('logo');
    const logoPreview = document.getElementById('companyLogoPreview');
});

function previewImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById('companyLogoPreview').src = e.target.result;
            document.getElementById('companyLogoPreview').classList.remove('d-none');
            document.querySelector('.image-placeholder').classList.add('d-none');
        };

        reader.readAsDataURL(input.files[0]);
    }
}