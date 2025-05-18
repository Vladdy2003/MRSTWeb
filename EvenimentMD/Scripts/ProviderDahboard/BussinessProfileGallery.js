// Global variables to store files
let selectedImages = [];
let selectedVideos = [];

// Maximum allowed files
const MAX_IMAGES = 10;
const MAX_VIDEOS = 3;

// File size limits in bytes
const MAX_IMAGE_SIZE = 5 * 1024 * 1024; // 5MB
const MAX_VIDEO_SIZE = 100 * 1024 * 1024; // 100MB

// Handle file selection for images
function handleFileSelect(input, type) {
    if (input.files && input.files.length > 0) {
        const files = Array.from(input.files);

        if (type === 'images') {
            // Check if exceeding max allowed images
            const existingCount = window.existingImagesCount || 0;
            const totalImages = existingCount + selectedImages.length + files.length;

            if (totalImages > MAX_IMAGES) {
                // Remove the alert and just update the count display
                document.getElementById('imageCount').textContent = selectedImages.length;
                return;
            }

            // Filter valid images
            const validImages = files.filter(file => {
                const isImage = file.type.startsWith('image/');
                const isValidSize = file.size <= MAX_IMAGE_SIZE;

                return isImage && isValidSize;
            });

            // Add to selected images
            selectedImages = [...selectedImages, ...validImages];

            // Update the count display
            document.getElementById('imageCount').textContent = selectedImages.length;

            // Create previews
            displayImagePreviews(validImages);

        } else if (type === 'videos') {
            // Check if exceeding max allowed videos
            const existingCount = window.existingVideosCount || 0;
            const totalVideos = existingCount + selectedVideos.length + files.length;

            if (totalVideos > MAX_VIDEOS) {
                // Remove the alert and just update the count display
                document.getElementById('videoCount').textContent = selectedVideos.length;
                return;
            }

            // Filter valid videos
            const validVideos = files.filter(file => {
                const isVideo = file.type.startsWith('video/') ||
                    file.name.toLowerCase().endsWith('.mp4') ||
                    file.name.toLowerCase().endsWith('.avi') ||
                    file.name.toLowerCase().endsWith('.mov') ||
                    file.name.toLowerCase().endsWith('.wmv') ||
                    file.name.toLowerCase().endsWith('.webm');
                const isValidSize = file.size <= MAX_VIDEO_SIZE;

                return isVideo && isValidSize;
            });

            // Add to selected videos
            selectedVideos = [...selectedVideos, ...validVideos];

            // Update the count display
            document.getElementById('videoCount').textContent = selectedVideos.length;

            // Create previews
            displayVideoPreviews(validVideos);
        }

        // Enable or disable the save button based on file selection
        toggleSaveButton();
    }
}

// Handle video selection
function handleVideoSelect(input, type) {
    handleFileSelect(input, type);
}

// Display image previews
function displayImagePreviews(images) {
    const container = document.getElementById('galleryGrid');

    images.forEach((image, index) => {
        const colDiv = document.createElement('div');
        colDiv.className = 'col-12 col-sm-6 col-md-4 col-lg-3';
        colDiv.dataset.index = selectedImages.indexOf(image);

        const galleryItem = document.createElement('div');
        galleryItem.className = 'gallery-item position-relative';

        const img = document.createElement('img');
        img.className = 'img-fluid rounded';
        img.style.height = '200px';
        img.style.objectFit = 'cover';
        img.style.width = '100%';
        img.alt = 'Image preview';

        // Read the image
        const reader = new FileReader();
        reader.onload = function (e) {
            img.src = e.target.result;
        };
        reader.readAsDataURL(image);

        const removeBtn = document.createElement('button');
        removeBtn.className = 'btn btn-danger btn-sm position-absolute top-0 end-0 m-2';
        removeBtn.innerHTML = '<i class="bi bi-trash"></i>';
        removeBtn.onclick = function () {
            removeSelectedFile('images', colDiv.dataset.index);
            colDiv.remove();
        };

        const fileInfo = document.createElement('div');
        fileInfo.className = 'position-absolute bottom-0 start-0 m-2';
        fileInfo.innerHTML = `<small class="badge bg-dark">${formatFileSize(image.size)}</small>`;

        galleryItem.appendChild(img);
        galleryItem.appendChild(removeBtn);
        galleryItem.appendChild(fileInfo);
        colDiv.appendChild(galleryItem);
        container.appendChild(colDiv);
    });
}

// Display video previews
function displayVideoPreviews(videos) {
    const container = document.getElementById('videoGrid');

    videos.forEach((video, index) => {
        const colDiv = document.createElement('div');
        colDiv.className = 'col-12 col-sm-6 col-md-4 col-lg-3';
        colDiv.dataset.index = selectedVideos.indexOf(video);

        const videoItem = document.createElement('div');
        videoItem.className = 'video-item position-relative';

        const videoElement = document.createElement('video');
        videoElement.className = 'w-100 rounded';
        videoElement.style.height = '200px';
        videoElement.style.objectFit = 'cover';
        videoElement.controls = true;

        // Read the video
        const reader = new FileReader();
        reader.onload = function (e) {
            videoElement.src = e.target.result;
        };
        reader.readAsDataURL(video);

        const removeBtn = document.createElement('button');
        removeBtn.className = 'btn btn-danger btn-sm position-absolute top-0 end-0 m-2';
        removeBtn.innerHTML = '<i class="bi bi-trash"></i>';
        removeBtn.onclick = function () {
            removeSelectedFile('videos', colDiv.dataset.index);
            colDiv.remove();
        };

        const fileInfo = document.createElement('div');
        fileInfo.className = 'position-absolute bottom-0 start-0 m-2';
        fileInfo.innerHTML = `<small class="badge bg-dark">${formatFileSize(video.size)}</small>`;

        videoItem.appendChild(videoElement);
        videoItem.appendChild(removeBtn);
        videoItem.appendChild(fileInfo);
        colDiv.appendChild(videoItem);
        container.appendChild(colDiv);
    });
}

// Format file size for display
function formatFileSize(bytes) {
    if (bytes < 1024) {
        return bytes + ' B';
    } else if (bytes < 1024 * 1024) {
        return (bytes / 1024).toFixed(2) + ' KB';
    } else {
        return (bytes / (1024 * 1024)).toFixed(2) + ' MB';
    }
}

// Remove selected file
function removeSelectedFile(type, index) {
    if (type === 'images') {
        selectedImages.splice(index, 1);
        document.getElementById('imageCount').textContent = selectedImages.length;
    } else if (type === 'videos') {
        selectedVideos.splice(index, 1);
        document.getElementById('videoCount').textContent = selectedVideos.length;
    }

    // Update indices for remaining items
    updateFileIndices(type);

    // Enable or disable the save button based on file selection
    toggleSaveButton();
}

// Update indices for remaining items after removal
function updateFileIndices(type) {
    const container = document.getElementById(type === 'images' ? 'galleryGrid' : 'videoGrid');
    const items = container.querySelectorAll('.col-12');

    items.forEach((item, newIndex) => {
        item.dataset.index = newIndex;
    });
}

// Enable or disable the save button based on file selection
function toggleSaveButton() {
    const saveBtn = document.getElementById('saveMediaBtn');
    if (selectedImages.length > 0 || selectedVideos.length > 0) {
        saveBtn.disabled = false;
    } else {
        saveBtn.disabled = true;
    }
}

// Upload media files
function uploadMedia() {
    // Disable the save button while uploading
    const saveBtn = document.getElementById('saveMediaBtn');
    saveBtn.disabled = true;
    saveBtn.innerHTML = '<i class="spinner-border spinner-border-sm me-2"></i>Se încarcă...';

    // Create form data
    const formData = new FormData();

    // Add CSRF token
    const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
    formData.append('__RequestVerificationToken', antiForgeryToken);

    // Add images with unique names to ensure they're processed correctly
    selectedImages.forEach((image, index) => {
        formData.append(`images[${index}]`, image, `image_${index}_${Date.now()}_${image.name}`);
    });

    // Add videos with unique names to ensure they're processed correctly
    selectedVideos.forEach((video, index) => {
        formData.append(`videos[${index}]`, video, `video_${index}_${Date.now()}_${video.name}`);
    });

    // Log formData contents for debugging (optional)
    console.log("FormData entries:");
    for (let pair of formData.entries()) {
        console.log(pair[0], pair[1]);
    }

    // Send request
    fetch('/Provider/UploadMedia', {
        method: 'POST',
        body: formData,
        credentials: 'same-origin'
    })
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                // Show success message
                const successContainer = document.createElement('div');
                successContainer.className = 'alert alert-success alert-dismissible fade show mt-3';
                successContainer.innerHTML = `
                    ${data.message}
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                `;

                // Insert success message
                const cardBody = document.querySelector('.card-body');
                cardBody.insertBefore(successContainer, cardBody.firstChild);

                // Refresh page after a short delay
                setTimeout(() => {
                    location.reload();
                }, 1500);
            } else {
                // Re-enable the save button
                saveBtn.disabled = false;
                saveBtn.innerHTML = '<i class="bi bi-save me-2"></i>Salvează Media Nouă';

                // Display error message instead of alert
                const errorContainer = document.createElement('div');
                errorContainer.className = 'alert alert-danger alert-dismissible fade show mt-3';
                errorContainer.innerHTML = `
                    ${data.message}
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                `;

                // Insert error message before the save button
                const cardBody = saveBtn.closest('.card-body');
                cardBody.insertBefore(errorContainer, saveBtn.parentElement);
            }
        })
        .catch(error => {
            console.error("Upload error:", error);

            // Re-enable the save button
            saveBtn.disabled = false;
            saveBtn.innerHTML = '<i class="bi bi-save me-2"></i>Salvează Media Nouă';

            // Display error message instead of alert
            const errorContainer = document.createElement('div');
            errorContainer.className = 'alert alert-danger alert-dismissible fade show mt-3';
            errorContainer.innerHTML = `
                A apărut o eroare. Vă rugăm să încercați din nou. Detalii: ${error.message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            `;

            // Insert error message before the save button
            const cardBody = saveBtn.closest('.card-body');
            cardBody.insertBefore(errorContainer, saveBtn.parentElement);
        });
}

// Delete existing media
function deleteExistingMedia(mediaId, mediaType) {
    // Show confirmation modal
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmBtn = document.getElementById('confirmDeleteBtn');

    // Set up confirmation button action
    confirmBtn.onclick = function () {
        // Close the modal
        modal.hide();

        // Send delete request
        const formData = new FormData();

        // Add CSRF token
        const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]').value;
        formData.append('__RequestVerificationToken', antiForgeryToken);
        formData.append('mediaId', mediaId);

        fetch('/Provider/DeleteMedia', {
            method: 'POST',
            body: formData,
            credentials: 'same-origin'
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    // Remove the media item from the DOM
                    const mediaItem = document.getElementById(`existing-${mediaType}-${mediaId}`);
                    if (mediaItem) {
                        mediaItem.remove();
                    }

                    // Update media count
                    if (mediaType === 'image') {
                        window.existingImagesCount--;
                    } else if (mediaType === 'video') {
                        window.existingVideosCount--;
                    }

                    // Add success notification instead of alert
                    const successContainer = document.createElement('div');
                    successContainer.className = 'alert alert-success alert-dismissible fade show mt-3';
                    successContainer.innerHTML = `
                    Media a fost ștearsă cu succes.
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                `;

                    // Insert success message
                    const cardBody = document.querySelector('.card-body');
                    cardBody.insertBefore(successContainer, cardBody.firstChild);

                    // Auto dismiss after 3 seconds
                    setTimeout(() => {
                        successContainer.classList.remove('show');
                        setTimeout(() => successContainer.remove(), 150);
                    }, 3000);
                } else {
                    // Display error message instead of alert
                    const errorContainer = document.createElement('div');
                    errorContainer.className = 'alert alert-danger alert-dismissible fade show mt-3';
                    errorContainer.innerHTML = `
                    ${data.message || 'A apărut o eroare la ștergerea media.'}
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                `;

                    // Insert error message
                    const cardBody = document.querySelector('.card-body');
                    cardBody.insertBefore(errorContainer, cardBody.firstChild);
                }
            })
            .catch(error => {
                // Display error message instead of alert
                const errorContainer = document.createElement('div');
                errorContainer.className = 'alert alert-danger alert-dismissible fade show mt-3';
                errorContainer.innerHTML = `
                A apărut o eroare la ștergerea media. Vă rugăm să încercați din nou.
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            `;

                // Insert error message
                const cardBody = document.querySelector('.card-body');
                cardBody.insertBefore(errorContainer, cardBody.firstChild);
            });
    };

    // Show the modal
    modal.show();
}

// Initialize on document ready
document.addEventListener('DOMContentLoaded', function () {
    // Disable save button initially
    const saveBtn = document.getElementById('saveMediaBtn');
    if (saveBtn) {
        saveBtn.disabled = true;
    }

    // Set up modals if needed
    const deleteConfirmModal = document.getElementById('deleteConfirmModal');
    if (deleteConfirmModal) {
        new bootstrap.Modal(deleteConfirmModal);
    }
});