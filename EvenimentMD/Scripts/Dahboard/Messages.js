document.addEventListener('DOMContentLoaded', function () {
    // Handle conversation item clicks
    document.querySelectorAll('.conversation-item').forEach(item => {
        item.addEventListener('click', function () {
            document.querySelectorAll('.conversation-item').forEach(i => i.classList.remove('active'));
            this.classList.add('active');
            this.classList.remove('unread');

            // On mobile, show chat area
            if (window.innerWidth <= 768) {
                document.querySelector('.chat-area').classList.add('mobile-active');
            }
        });
    });

    // Handle back button on mobile
    document.querySelector('.back-button').addEventListener('click', function () {
        document.querySelector('.chat-area').classList.remove('mobile-active');
    });

    // Handle message input auto-resize
    const messageInput = document.querySelector('.message-input');
    messageInput.addEventListener('input', function () {
        this.style.height = 'auto';
        this.style.height = (this.scrollHeight) + 'px';
    });

    // Handle send button
    document.querySelector('.send-button').addEventListener('click', function () {
        const message = messageInput.value.trim();
        if (message) {
            // Here you would normally send the message to the server
            // For now, just simulate adding it to the chat
            const messageElement = document.createElement('div');
            messageElement.className = 'message sent';
            messageElement.innerHTML = `
                    <div class="message-content">
                        <p class="message-text">${message}</p>
                        <div class="message-time">${new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}</div>
                    </div>
                `;
            document.querySelector('.messages-area').appendChild(messageElement);
            messageInput.value = '';
            messageInput.style.height = 'auto';

            // Scroll to bottom
            document.querySelector('.messages-area').scrollTop = document.querySelector('.messages-area').scrollHeight;
        }
    });

    // Handle enter key in message input
    messageInput.addEventListener('keypress', function (e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            document.querySelector('.send-button').click();
        }
    });

    // Handle conversation tabs
    document.querySelectorAll('.conversation-tab').forEach(tab => {
        tab.addEventListener('click', function () {
            document.querySelectorAll('.conversation-tab').forEach(t => t.classList.remove('active'));
            this.classList.add('active');
            // Here you would filter conversations based on the selected tab
        });
    });

    // New message button
    document.getElementById('newMessageBtn').addEventListener('click', function () {
        const modal = new bootstrap.Modal(document.getElementById('newMessageModal'));
        modal.show();
    });
});