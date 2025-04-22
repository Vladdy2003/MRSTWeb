document.addEventListener('DOMContentLoaded', function () {
    let deleteTarget = null;

    // Handle add member form submission
    document.getElementById('addMemberForm').addEventListener('submit', function (e) {
        e.preventDefault();
        const addModal = bootstrap.Modal.getInstance(document.getElementById('addMemberModal'));
        addModal.hide();
        showSuccessToast('Membru adăugat cu succes!');

        // Here you would normally send the data to the server
        // For now, just reset the form
        this.reset();
    });

    // Handle edit member form submission
    document.getElementById('editMemberForm').addEventListener('submit', function (e) {
        e.preventDefault();
        const editModal = bootstrap.Modal.getInstance(document.getElementById('editMemberModal'));
        editModal.hide();
        showSuccessToast('Modificările au fost salvate!');

        // Here you would normally send the data to the server
    });

    // Handle delete buttons
    document.querySelectorAll('.btn-outline-danger').forEach(button => {
        button.addEventListener('click', function () {
            deleteTarget = this.closest('.team-member');
            const deleteModal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
            deleteModal.show();
        });
    });

    // Confirm delete
    document.getElementById('confirmDelete').addEventListener('click', function () {
        if (deleteTarget) {
            deleteTarget.remove();
            const deleteModal = bootstrap.Modal.getInstance(document.getElementById('deleteConfirmModal'));
            deleteModal.hide();
            showSuccessToast('Membru șters cu succes!');

            // Check if team is empty
            if (document.querySelectorAll('.team-member').length === 0) {
                document.querySelector('.empty-state').style.display = 'block';
            }
        }
    });

    // Handle search
    document.querySelector('.search-input input').addEventListener('input', function (e) {
        const searchTerm = e.target.value.toLowerCase();
        document.querySelectorAll('.team-member').forEach(member => {
            const text = member.textContent.toLowerCase();
            member.style.display = text.includes(searchTerm) ? '' : 'none';
        });
    });

    // Phone number formatting
    document.querySelectorAll('input[type="tel"]').forEach(input => {
        input.addEventListener('input', function (e) {
            let value = e.target.value.replace(/\D/g, '');
            if (value.length > 0) {
                value = '+' + value;
            }
            e.target.value = value;
        });
    });

    // Show success toast helper function
    function showSuccessToast(message) {
        const toastElement = document.getElementById('successToast');
        const toastBody = toastElement.querySelector('.toast-body');
        toastBody.textContent = message;
        const toast = new bootstrap.Toast(toastElement);
        toast.show();
    }
});