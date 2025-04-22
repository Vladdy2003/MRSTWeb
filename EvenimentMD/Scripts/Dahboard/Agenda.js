document.addEventListener('DOMContentLoaded', function () {
    // Update current time indicator
    function updateCurrentTimeIndicator() {
        const now = new Date();
        const currentHour = now.getHours();
        const currentMinute = now.getMinutes();

        if (currentHour >= 9 && currentHour <= 19) {
            const minutesSinceNine = (currentHour - 9) * 60 + currentMinute;
            const topPosition = (minutesSinceNine / 60) * 60; // 60px per hour
            const indicator = document.querySelector('.current-time-indicator');
            if (indicator) {
                indicator.style.top = topPosition + 'px';
            }
        }
    }

    updateCurrentTimeIndicator();
    setInterval(updateCurrentTimeIndicator, 60000); // Update every minute

    // Add event listeners for events
    document.querySelectorAll('.event').forEach(event => {
        event.addEventListener('click', function () {
            const modal = new bootstrap.Modal(document.getElementById('eventModal'));
            modal.show();
        });
    });

    // Add booking button
    document.getElementById('addBooking').addEventListener('click', function () {
        const modal = new bootstrap.Modal(document.getElementById('addBookingModal'));
        modal.show();
    });

    // Week navigation
    let currentWeekStart = new Date();
    currentWeekStart.setDate(currentWeekStart.getDate() - currentWeekStart.getDay() + 1); // Monday

    function updateWeekDisplay() {
        const weekEnd = new Date(currentWeekStart);
        weekEnd.setDate(weekEnd.getDate() + 6);

        const options = { day: 'numeric', month: 'short' };
        const startStr = currentWeekStart.toLocaleDateString('ro-RO', options);
        const endStr = weekEnd.toLocaleDateString('ro-RO', options);

        document.getElementById('weekSelector').textContent =
            `Luni, ${startStr} - Duminică, ${endStr}`;
    }

    document.getElementById('prevWeek').addEventListener('click', function () {
        currentWeekStart.setDate(currentWeekStart.getDate() - 7);
        updateWeekDisplay();
    });

    document.getElementById('nextWeek').addEventListener('click', function () {
        currentWeekStart.setDate(currentWeekStart.getDate() + 7);
        updateWeekDisplay();
    });
});