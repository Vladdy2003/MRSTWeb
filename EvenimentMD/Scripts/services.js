// Toggle function for filter lists
function toggleOptions(button) {
    const listItems = button.previousElementSibling.getElementsByTagName("li");
    const listItemsArray = Array.from(listItems);
    
    // Toggle for hidden elements
    listItemsArray.slice(5).forEach(item => item.classList.toggle("hidden"));
    
    // Check if there are hidden elements
    const hiddenItems = listItemsArray.filter(item => item.classList.contains("hidden"));
    
    // Change button text based on elements state
    if (hiddenItems.length > 0) {
        button.textContent = "Afișează mai mult";
    } else {
        button.textContent = "Ascunde";
    }
}

// Function for resetting filters
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

// Toggle filters panel on mobile
document.addEventListener('DOMContentLoaded', function() {
    const toggleBtn = document.getElementById('toggleFiltersBtn');
    const filtersPanel = document.getElementById('filtersPanel');
    
    if (toggleBtn && filtersPanel) {
        toggleBtn.addEventListener('click', function() {
            filtersPanel.classList.toggle('show-filters');
            if (filtersPanel.classList.contains('show-filters')) {
                toggleBtn.innerHTML = '<i class="fas fa-times"></i> Ascunde filtre';
            } else {
                toggleBtn.innerHTML = '<i class="fas fa-filter"></i> Afișează filtre';
            }
        });
    }
    
    // Hide filters panel on small screens by default
    function checkWidth() {
        if (window.innerWidth < 768) {
            filtersPanel.classList.remove('show-filters');
            toggleBtn.innerHTML = '<i class="fas fa-filter"></i> Afișează filtre';
        } else {
            filtersPanel.classList.add('show-filters');
        }
    }
    
    // Initial check
    checkWidth();
    
    // Check on resize
    window.addEventListener('resize', checkWidth);
});