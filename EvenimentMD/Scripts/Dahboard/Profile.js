document.addEventListener('DOMContentLoaded', function () {
    // Handle tab switching
    document.querySelectorAll('.profile-tab').forEach(tab => {
        tab.addEventListener('click', function () {
            // Remove active class from all tabs and panes
            document.querySelectorAll('.profile-tab').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.tab-pane').forEach(p => p.style.display = 'none');

            // Add active class to clicked tab
            this.classList.add('active');

            // Show corresponding pane
            const tabId = this.dataset.tab;
            document.getElementById(tabId).style.display = 'block';
        });
    });

    // Handle form submissions
    document.querySelectorAll('form').forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            // Show success toast
            const toast = new bootstrap.Toast(document.getElementById('successToast'));
            toast.show();
        });
    });

    // Handle avatar upload
    document.querySelector('input[type="file"]').addEventListener('change', function (e) {
        if (e.target.files && e.target.files[0]) {
            // Here you would typically upload the file to your server
            // For now, just show a success message
            const toast = new bootstrap.Toast(document.getElementById('successToast'));
            toast.show();
        }
    });

    // Handle dangerous actions
    document.querySelector('.btn-danger').addEventListener('click', function () {
        if (confirm('Sunteți sigur că doriți să ștergeți contul? Această acțiune este ireversibilă!')) {
            // Handle account deletion
            alert('Funcționalitate în dezvoltare');
        }
    });

    document.querySelector('.btn-outline-danger').addEventListener('click', function () {
        if (confirm('Sunteți sigur că doriți să deconectați toate dispozitivele?')) {
            // Handle logout from all devices
            const toast = new bootstrap.Toast(document.getElementById('successToast'));
            toast.show();
        }
    });
});