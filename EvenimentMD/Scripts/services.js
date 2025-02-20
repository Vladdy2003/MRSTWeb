function toggleOptions(button) {
    const listItems = button.previousElementSibling.getElementsByTagName("li");
    const listItemsArray = Array.from(listItems);

    // Toggle pentru elementele ascunse
    listItemsArray.slice(5).forEach(item => item.classList.toggle("hidden"));

    // Verifică dacă există elemente ascunse
    const hiddenItems = listItemsArray.filter(item => item.classList.contains("hidden"));

    // Schimbă textul butonului în funcție de starea elementelor ascunse
    if (hiddenItems.length > 0) {
        button.textContent = "Afișează mai mult";
    } else {
        button.textContent = "Ascunde";
    }
}



function resetFilters() {
    // Reset checkboxes
    const checkboxes = document.querySelectorAll('input[type="checkbox"]');
    checkboxes.forEach(checkbox => {
        checkbox.checked = false;
    });

    // Reset date input
    const dateInput = document.querySelector('input[type="date"]');
    if (dateInput) {
        dateInput.value = "";
    }

    // Reset price inputs
    const priceInputs = document.querySelectorAll('input[type="number"]');
    priceInputs.forEach(input => {
        input.value = "";
    });
}
