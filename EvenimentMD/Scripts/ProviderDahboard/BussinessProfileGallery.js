function handleFileSelect(input) {
    const files = input.files;
    const galleryGrid = document.getElementById('galleryGrid');

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function (e) {
            // Create gallery item
            const col = document.createElement('div');
            col.className = 'col-12 col-sm-6 col-md-4 col-lg-3';

            const galleryItem = document.createElement('div');
            galleryItem.className = 'gallery-item';

            // Create image element
            const img = document.createElement('img');
            img.src = e.target.result;
            img.alt = 'Gallery image';

            // Create delete button
            const deleteBtn = document.createElement('button');
            deleteBtn.className = 'delete-btn';
            deleteBtn.innerHTML = '<i class="bi bi-x-lg"></i>';
            deleteBtn.onclick = function () {
                col.remove();
            };

            // Assemble elements
            galleryItem.appendChild(img);
            galleryItem.appendChild(deleteBtn);
            col.appendChild(galleryItem);
            galleryGrid.appendChild(col);
        };

        reader.readAsDataURL(file);
    }

    // Reset input value to allow re-uploading same files
    input.value = '';
}

function handleVideoSelect(input) {
    const files = input.files;
    const videoGrid = document.getElementById('videoGrid');

    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const reader = new FileReader();

        reader.onload = function (e) {
            // Create video item
            const col = document.createElement('div');
            col.className = 'col-12 col-sm-6 col-md-4 col-lg-3';

            const videoItem = document.createElement('div');
            videoItem.className = 'video-item';

            // Create video element
            const video = document.createElement('video');
            video.src = e.target.result;
            video.controls = true;
            video.preload = 'metadata';

            // Create play overlay
            const playOverlay = document.createElement('div');
            playOverlay.className = 'play-overlay';
            playOverlay.innerHTML = '<i class="bi bi-play-fill"></i>';

            // Create delete button
            const deleteBtn = document.createElement('button');
            deleteBtn.className = 'delete-btn';
            deleteBtn.innerHTML = '<i class="bi bi-x-lg"></i>';
            deleteBtn.onclick = function () {
                col.remove();
            };

            // Assemble elements
            videoItem.appendChild(video);
            videoItem.appendChild(playOverlay);
            videoItem.appendChild(deleteBtn);
            col.appendChild(videoItem);
            videoGrid.appendChild(col);

            // Hide play overlay when video plays
            video.addEventListener('play', function () {
                playOverlay.style.display = 'none';
            });

            // Show play overlay when video pauses
            video.addEventListener('pause', function () {
                playOverlay.style.display = 'flex';
            });
        };

        reader.readAsDataURL(file);
    }

    // Reset input value to allow re-uploading same files
    input.value = '';
}

// Optional: Add drag & drop functionality
const dropZone = document.querySelector('.card-body');

dropZone.addEventListener('dragover', (e) => {
    e.preventDefault();
    dropZone.style.backgroundColor = '#f8f9fa';
});

dropZone.addEventListener('dragleave', (e) => {
    e.preventDefault();
    dropZone.style.backgroundColor = '';
});

dropZone.addEventListener('drop', (e) => {
    e.preventDefault();
    dropZone.style.backgroundColor = '';

    const files = e.dataTransfer.files;
    const imageFiles = [];
    const videoFiles = [];

    // Separate image and video files
    for (let i = 0; i < files.length; i++) {
        if (files[i].type.startsWith('image/')) {
            imageFiles.push(files[i]);
        } else if (files[i].type.startsWith('video/')) {
            videoFiles.push(files[i]);
        }
    }

    // Handle image files
    if (imageFiles.length > 0) {
        handleFileSelect({ files: imageFiles });
    }

    // Handle video files
    if (videoFiles.length > 0) {
        handleVideoSelect({ files: videoFiles });
    }
});