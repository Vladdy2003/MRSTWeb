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
    document.getElementById('emptyState').classList.add('d-none');
    document.getElementById('serviceName').focus();
}

function hideServiceForm() {
    document.getElementById('serviceForm').classList.add('d-none');
    clearForm();
    editingServiceCard = null;

    // Show empty state if no services
    const serviceList = document.getElementById('serviceList');
    if (serviceList.children.length === 0) {
        document.getElementById('emptyState').classList.remove('d-none');
    }
}

function clearForm() {
    document.getElementById('serviceName').value = '';
    document.getElementById('servicePrice').value = '';
    document.getElementById('serviceCurrency').value = 'MDL';
    tinymce.get('serviceDescription').setContent('');
}

function addService() {
    const serviceName = document.getElementById('serviceName').value;
    const servicePrice = document.getElementById('servicePrice').value;
    const serviceCurrency = document.getElementById('serviceCurrency').value;
    const serviceDescription = tinymce.get('serviceDescription').getContent();

    if (!serviceName || !servicePrice) {
        alert('Vă rugăm să completați câmpurile obligatorii');
        return;
    }

    if (editingServiceCard) {
        // Update existing service
        updateServiceCard(editingServiceCard, serviceName, servicePrice, serviceCurrency, serviceDescription);
    } else {
        // Create new service
        createServiceCard(serviceName, servicePrice, serviceCurrency, serviceDescription);
    }

    // Clear form and hide it
    clearForm();
    hideServiceForm();
}

function createServiceCard(name, price, currency, description) {
    const serviceList = document.getElementById('serviceList');
    const emptyState = document.getElementById('emptyState');

    // Hide empty state
    emptyState.classList.add('d-none');

    // Get conversions
    let conversions = '';
    if (currency === 'MDL') {
        conversions = `≈ ${convertCurrency(price, 'MDL', 'EUR')} EUR | ${convertCurrency(price, 'MDL', 'USD')} USD`;
    } else if (currency === 'EUR') {
        conversions = `≈ ${convertCurrency(price, 'EUR', 'MDL')} MDL | ${convertCurrency(price, 'EUR', 'USD')} USD`;
    } else if (currency === 'USD') {
        conversions = `≈ ${convertCurrency(price, 'USD', 'MDL')} MDL | ${convertCurrency(price, 'USD', 'EUR')} EUR`;
    }

    // Create service card
    const col = document.createElement('div');
    col.className = 'col-12 mb-3';

    const serviceCard = document.createElement('div');
    serviceCard.className = 'service-card p-3';

    const descriptionDiv = document.createElement('div');
    descriptionDiv.innerHTML = description || 'Fără descriere';

    // Check if description is longer than 4 lines
    const needsExpansion = checkIfDescriptionNeedsExpansion(descriptionDiv);

    serviceCard.innerHTML = `
        <div class="content-wrapper">
            <div class="description-section">
                <h5 class="mb-1">${name}</h5>
                <div class="service-description text-muted mb-2">${description || 'Fără descriere'}</div>
                ${needsExpansion ? '<div class="show-more-btn" onclick="toggleDescription(this)">Vezi mai mult...</div>' : ''}
            </div>
            <div class="price-section text-end">
                <p class="service-price mb-1">${price} ${currency}</p>
                <p class="service-price-conversions mb-2">${conversions}</p>
                <div class="service-actions">
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="editService(this)">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteService(this)">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        </div>
    `;

    col.appendChild(serviceCard);
    serviceList.appendChild(col);
}

function updateServiceCard(col, name, price, currency, description) {
    const serviceCard = col.querySelector('.service-card');

    // Get conversions
    let conversions = '';
    if (currency === 'MDL') {
        conversions = `≈ ${convertCurrency(price, 'MDL', 'EUR')} EUR | ${convertCurrency(price, 'MDL', 'USD')} USD`;
    } else if (currency === 'EUR') {
        conversions = `≈ ${convertCurrency(price, 'EUR', 'MDL')} MDL | ${convertCurrency(price, 'EUR', 'USD')} USD`;
    } else if (currency === 'USD') {
        conversions = `≈ ${convertCurrency(price, 'USD', 'MDL')} MDL | ${convertCurrency(price, 'USD', 'EUR')} EUR`;
    }

    const descriptionDiv = document.createElement('div');
    descriptionDiv.innerHTML = description || 'Fără descriere';

    // Check if description is longer than 4 lines
    const needsExpansion = checkIfDescriptionNeedsExpansion(descriptionDiv);

    serviceCard.innerHTML = `
        <div class="content-wrapper">
            <div class="description-section">
                <h5 class="mb-1">${name}</h5>
                <div class="service-description text-muted mb-2">${description || 'Fără descriere'}</div>
                ${needsExpansion ? '<div class="show-more-btn" onclick="toggleDescription(this)">Vezi mai mult...</div>' : ''}
            </div>
            <div class="price-section text-end">
                <p class="service-price mb-1">${price} ${currency}</p>
                <p class="service-price-conversions mb-2">${conversions}</p>
                <div class="service-actions">
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="editService(this)">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteService(this)">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        </div>
    `;
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

function editService(button) {
    const serviceCard = button.closest('.col-12');
    const nameElement = serviceCard.querySelector('h5');
    const priceElement = serviceCard.querySelector('.service-price');
    const descriptionElement = serviceCard.querySelector('.service-description');

    // Extract data
    const name = nameElement.textContent;
    const priceText = priceElement.textContent.split(' ');
    const price = parseInt(priceText[0]);
    const currency = priceText[1];
    const description = descriptionElement.innerHTML;

    // Fill the form
    document.getElementById('serviceName').value = name;
    document.getElementById('servicePrice').value = price;
    document.getElementById('serviceCurrency').value = currency;
    tinymce.get('serviceDescription').setContent(description === 'Fără descriere' ? '' : description);

    // Set editing state
    editingServiceCard = serviceCard;

    // Show form
    showServiceForm();
}

function deleteService(button) {
    if (confirm('Sigur doriți să ștergeți acest serviciu?')) {
        const serviceCard = button.closest('.col-12');
        serviceCard.remove();

        // Show empty state if no services left
        const serviceList = document.getElementById('serviceList');
        if (serviceList.children.length === 0) {
            document.getElementById('emptyState').classList.remove('d-none');
        }
    }
}