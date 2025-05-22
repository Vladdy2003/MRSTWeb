tinymce.init({
    selector: '#serviceDescription',
    menubar: false,
    plugins: 'lists link image preview',
    toolbar: 'bold italic underline | bullist numlist | undo redo | preview',
    branding: false,
    height: 300
});

let editingServiceCard = null;
let exchangeRates = {};

// Fetch exchange rates on page load
fetchExchangeRates();

async function fetchExchangeRates() {
    try {
        // Using ExchangeRate-API (you need to get your own API key)
        const response = await fetch('https://api.exchangerate-api.com/v4/latest/USD');
        const data = await response.json();

        // Store rates
        exchangeRates = {
            'USD': 1,
            'EUR': data.rates.EUR,
            'MDL': data.rates.MDL
        };
    } catch (error) {
        console.error('Error fetching exchange rates:', error);
        // Fallback rates in case API fails
        exchangeRates = {
            'USD': 1,
            'EUR': 0.92,
            'MDL': 18.10
        };
    }
}

function convertCurrency(amount, fromCurrency, toCurrency) {
    if (fromCurrency === toCurrency) return amount;

    // Convert to USD first, then to target currency
    const amountInUSD = amount / exchangeRates[fromCurrency];
    return (amountInUSD * exchangeRates[toCurrency]).toFixed(2);
}

function showServiceForm() {
    document.getElementById('serviceForm').classList.remove('d-none');

    // Ascunde empty state dacă există
    const emptyState = document.getElementById('emptyState');
    if (emptyState) {
        emptyState.style.display = 'none';
    }

    document.getElementById('serviceName').focus();
}

function hideServiceForm() {
    document.getElementById('serviceForm').classList.add('d-none');
    clearForm();

    // Resetează formularul la modul de adăugare
    document.getElementById('serviceForm').action = '/Provider/AddService';
    document.getElementById('submitBtn').textContent = 'Salvează';
    document.getElementById('isEditing').value = 'false';
    document.getElementById('serviceId').value = '0';

    // Verifică dacă trebuie să afișeze empty state
    const serviceList = document.getElementById('serviceList');
    const services = serviceList.querySelectorAll('[data-service-id]');
    if (services.length === 0) {
        const emptyState = document.getElementById('emptyState');
        if (emptyState) {
            emptyState.style.display = 'block';
        }
    }
}

function clearForm() {
    document.getElementById('serviceName').value = '';
    document.getElementById('servicePrice').value = '';
    document.getElementById('serviceCurrency').value = 'MDL';

    // Clear TinyMCE content
    if (tinymce.get('serviceDescription')) {
        tinymce.get('serviceDescription').setContent('');
    }
}

function editService(serviceId, serviceName, servicePrice, currency, description) {
    // Populează formularul cu datele existente
    document.getElementById('serviceId').value = serviceId;
    document.getElementById('serviceName').value = serviceName;
    document.getElementById('servicePrice').value = servicePrice;
    document.getElementById('serviceCurrency').value = currency;

    // Set content for TinyMCE
    if (tinymce.get('serviceDescription')) {
        tinymce.get('serviceDescription').setContent(description || '');
    }

    // Marchează că suntem în modul de editare
    document.getElementById('isEditing').value = 'true';

    // Schimbă acțiunea formularului pentru editare
    document.getElementById('serviceForm').action = '/Provider/UpdateService?serviceId=' + serviceId;

    // Schimbă textul butonului
    document.getElementById('submitBtn').textContent = 'Actualizează';

    // Afișează formularul
    showServiceForm();
}

function deleteService(serviceId) {
    if (confirm('Sigur doriți să ștergeți acest serviciu?')) {
        // Obține tokenul anti-forgery
        var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        // Trimite cererea AJAX pentru ștergere
        fetch('/Provider/DeleteService', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: '__RequestVerificationToken=' + encodeURIComponent(token) + '&serviceId=' + serviceId
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    // Elimină elementul din DOM
                    var serviceElement = document.querySelector('[data-service-id="' + serviceId + '"]');
                    if (serviceElement) {
                        serviceElement.remove();
                    }

                    // Verifică dacă mai sunt servicii
                    var remainingServices = document.querySelectorAll('[data-service-id]');
                    if (remainingServices.length === 0) {
                        // Afișează starea goală
                        var serviceList = document.getElementById('serviceList');
                        var emptyState = '<div id="emptyState" class="text-center py-5">' +
                            '<i class="bi bi-box-seam" style="font-size: 3rem; color: #6c757d;"></i>' +
                            '<p class="mt-3 text-muted">Nu există servicii adăugate momentan.</p>' +
                            '</div>';
                        serviceList.innerHTML = emptyState;
                    }

                    // Afișează mesaj de succes
                    showMessage('Serviciul a fost șters cu succes!', 'success');
                } else {
                    showMessage('Eroare la ștergerea serviciului: ' + data.message, 'error');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showMessage('A apărut o eroare la ștergerea serviciului.', 'error');
            });
    }
}

// Păstrează doar deleteServiceLocal pentru serviciile care nu sunt încă salvate
function deleteServiceLocal(button) {
    if (confirm('Sigur doriți să ștergeți acest serviciu?')) {
        const serviceCard = button.closest('.col-12');
        serviceCard.remove();

        // Show empty state if no services left
        const serviceList = document.getElementById('serviceList');
        const remainingServices = serviceList.querySelectorAll('[data-service-id]');
        if (remainingServices.length === 0) {
            const emptyState = '<div id="emptyState" class="text-center py-5">' +
                '<i class="bi bi-box-seam" style="font-size: 3rem; color: #6c757d;"></i>' +
                '<p class="mt-3 text-muted">Nu există servicii adăugate momentan.</p>' +
                '</div>';
            serviceList.innerHTML = emptyState;
        }
    }
}

function checkIfDescriptionNeedsExpansion(descriptionElement) {
    // Create a temporary element to measure the height
    const tempDiv = document.createElement('div');
    tempDiv.style.position = 'absolute';
    tempDiv.style.visibility = 'hidden';
    tempDiv.style.width = '500px'; // Approximate width
    tempDiv.innerHTML = descriptionElement.innerHTML;

    document.body.appendChild(tempDiv);

    const lineHeight = parseInt(window.getComputedStyle(tempDiv).lineHeight);
    const height = tempDiv.clientHeight;
    const lineCount = Math.ceil(height / lineHeight);

    document.body.removeChild(tempDiv);

    return lineCount > 4;
}

function toggleDescription(button) {
    const descriptionElement = button.previousElementSibling;
    const isExpanded = descriptionElement.classList.contains('expanded');

    if (isExpanded) {
        descriptionElement.classList.remove('expanded');
        button.textContent = 'Vezi mai mult...';
    } else {
        descriptionElement.classList.add('expanded');
        button.textContent = 'Vezi mai puțin';
    }
}

function escapeHtml(text) {
    if (!text) return '';
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, function (m) { return map[m]; });
}

function showMessage(message, type) {
    // Create alert element
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type === 'success' ? 'success' : 'danger'} alert-dismissible fade show`;
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;

    // Insert at the beginning of card body
    const cardBody = document.querySelector('.card-body');
    cardBody.insertBefore(alertDiv, cardBody.firstChild);

    // Auto-hide after 5 seconds
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.remove();
        }
    }, 5000);
}

// Initialize on document ready
document.addEventListener('DOMContentLoaded', function () {
    // Initialize form validation if needed
    const serviceForm = document.getElementById('serviceForm');
    if (serviceForm) {
        serviceForm.addEventListener('submit', function (e) {
            const serviceName = document.getElementById('serviceName').value.trim();
            const servicePrice = document.getElementById('servicePrice').value.trim();

            if (!serviceName || !servicePrice) {
                e.preventDefault();
                showMessage('Vă rugăm să completați câmpurile obligatorii', 'error');
                return false;
            }

            if (parseFloat(servicePrice) <= 0) {
                e.preventDefault();
                showMessage('Prețul trebuie să fie mai mare decât 0', 'error');
                return false;
            }
        });
    }

    // Auto-hide alerts after page load
    setTimeout(() => {
        const alerts = document.querySelectorAll('.alert');
        alerts.forEach(alert => {
            if (alert.classList.contains('alert-success')) {
                const bsAlert = new bootstrap.Alert(alert);
                bsAlert.close();
            }
        });
    }, 5000);
});