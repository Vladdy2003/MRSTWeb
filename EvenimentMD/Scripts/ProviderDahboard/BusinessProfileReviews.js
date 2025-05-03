document.querySelectorAll('.filter-badge').forEach(badge => {
    badge.addEventListener('click', function () {
        document.querySelectorAll('.filter-badge').forEach(b => b.classList.remove('bg-primary'));
        document.querySelectorAll('.filter-badge').forEach(b => b.classList.add('bg-light', 'text-dark'));
        this.classList.remove('bg-light', 'text-dark');
        this.classList.add('bg-primary');
    });
});