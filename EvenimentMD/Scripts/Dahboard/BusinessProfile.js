tinymce.init({
    selector: '#companyDescription',
    menubar: false,
    plugins: 'lists link image preview',
    toolbar: 'bold italic underline | bullist numlist | undo redo | preview',
    branding: false,
    height: 300
});

document.addEventListener('DOMContentLoaded', function () {
    // Initialize feather icons
    feather.replace();

    // Logo preview functionality
    const logoInput = document.getElementById('companyLogo');
    const logoPreview = document.getElementById('companyLogoPreview');

    logoInput.addEventListener('change', function () {
        if (this.files && this.files[0]) {
            const reader = new FileReader();
            reader.onload = function (e) {
                logoPreview.src = e.target.result;
            };
            reader.readAsDataURL(this.files[0]);
        }
    });

    // Form submission
    const form = document.getElementById('businessProfileForm');
    form.addEventListener('submit', function (e) {
        e.preventDefault();
        // Here you would typically send the form data to the server
        // For now, just show a success message
        alert('Profilul a fost salvat cu succes!');
    });
});
function previewImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById('companyLogoPreview').src = e.target.result;
            document.getElementById('companyLogoPreview').classList.remove('d-none');
            document.querySelector('.image-placeholder').classList.add('d-none');
        }

        reader.readAsDataURL(input.files[0]);
    }
}