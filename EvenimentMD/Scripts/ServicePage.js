document.addEventListener('DOMContentLoaded', function () {
    // Variabile pentru galerie
    const galleryOverlay = document.getElementById('galleryOverlay');
    const openGalleryBtnDesktop = document.getElementById('openGalleryBtnDesktop');
    const openGalleryBtnMobile = document.getElementById('openGalleryBtnMobile');
    const closeGalleryBtn = document.getElementById('closeGalleryBtn');
    const mainGalleryImage = document.getElementById('mainGalleryImage');
    const prevImageBtn = document.getElementById('prevImageBtn');
    const nextImageBtn = document.getElementById('nextImageBtn');
    const thumbnails = document.querySelectorAll('.thumbnail');

    // Colecție de imagini și index curent
    const galleryImages = [];
    let currentImageIndex = 0;

    // Adăugăm toate imaginile în array
    thumbnails.forEach(thumbnail => {
        const imgSrc = thumbnail.querySelector('img').src;
        galleryImages.push(imgSrc);
    });

    // Inițializăm imaginea principală dacă există imagini
    if (galleryImages.length > 0) {
        mainGalleryImage.src = galleryImages[0];
    }

    // Funcție pentru deschiderea galeriei
    function openGallery() {
        galleryOverlay.style.display = 'block';
        document.body.style.overflow = 'hidden'; // Previne scroll-ul în pagină
    }

    // Funcție pentru închiderea galeriei
    function closeGallery() {
        galleryOverlay.style.display = 'none';
        document.body.style.overflow = ''; // Restaurează scroll-ul în pagină
    }

    // Funcție pentru navigarea la imaginea anterioară
    function goToPrevImage() {
        currentImageIndex = (currentImageIndex - 1 + galleryImages.length) % galleryImages.length;
        updateMainImage();
    }

    // Funcție pentru navigarea la imaginea următoare
    function goToNextImage() {
        currentImageIndex = (currentImageIndex + 1) % galleryImages.length;
        updateMainImage();
    }

    // Funcție pentru actualizarea imaginii principale și thumbnail-ului activ
    function updateMainImage() {
        mainGalleryImage.src = galleryImages[currentImageIndex];

        // Actualizăm thumbnail-ul activ
        thumbnails.forEach(thumbnail => {
            const index = parseInt(thumbnail.getAttribute('data-index'));
            if (index === currentImageIndex) {
                thumbnail.classList.add('active');
            } else {
                thumbnail.classList.remove('active');
            }
        });

        // Facem scroll la thumbnail-ul activ
        const activeThumb = document.querySelector('.thumbnail.active');
        if (activeThumb) {
            activeThumb.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'center' });
        }
    }

    // Event listeners pentru butoanele din galerie
    if (openGalleryBtnDesktop) {
        openGalleryBtnDesktop.addEventListener('click', function (e) {
            e.preventDefault();
            openGallery();
        });
    }

    if (openGalleryBtnMobile) {
        openGalleryBtnMobile.addEventListener('click', function (e) {
            e.preventDefault();
            openGallery();
        });
    }

    closeGalleryBtn.addEventListener('click', closeGallery);

    prevImageBtn.addEventListener('click', goToPrevImage);
    nextImageBtn.addEventListener('click', goToNextImage);

    // Event listeners pentru thumbnail-uri
    thumbnails.forEach(thumbnail => {
        thumbnail.addEventListener('click', function () {
            currentImageIndex = parseInt(this.getAttribute('data-index'));
            updateMainImage();
        });
    });

    // Event listener pentru taste (săgeți stânga/dreapta și ESC)
    document.addEventListener('keydown', function (e) {
        if (galleryOverlay.style.display === 'block') {
            if (e.key === 'ArrowLeft') {
                goToPrevImage();
            } else if (e.key === 'ArrowRight') {
                goToNextImage();
            } else if (e.key === 'Escape') {
                closeGallery();
            }
        }
    });

    // Închide galeria când se face click în afara containerului
    galleryOverlay.addEventListener('click', function (e) {
        if (e.target === galleryOverlay) {
            closeGallery();
        }
    });
});