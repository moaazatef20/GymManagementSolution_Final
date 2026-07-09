(function () {
    function initWorkoutSlider() {
        const slider = document.getElementById('workoutSlider');
        if (!slider) return;

        const track = slider.querySelector('.workout-slider__track');
        const slides = slider.querySelectorAll('.workout-slider__slide');
        const indicatorsContainer = slider.querySelector('.indicators');
        const prevBtn = slider.querySelector('[data-direction="prev"]');
        const nextBtn = slider.querySelector('[data-direction="next"]');

        if (!track || slides.length === 0) return;

        let currentIndex = 0;
        let autoTimer = null;

        function getVisibleCount() {
            if (window.innerWidth >= 992) return 3;
            if (window.innerWidth >= 768) return 2;
            return 1;
        }

        function getMaxIndex() {
            return Math.max(0, slides.length - getVisibleCount());
        }

        function buildIndicators() {
            if (!indicatorsContainer) return;
            indicatorsContainer.innerHTML = '';
            const total = getMaxIndex() + 1;

            for (let i = 0; i < total; i++) {
                const dot = document.createElement('button');
                dot.type = 'button';
                dot.className = 'indicator' + (i === currentIndex ? ' active' : '');
                dot.setAttribute('aria-label', 'Go to slide ' + (i + 1));
                dot.addEventListener('click', function () {
                    goTo(i);
                    restartAutoPlay();
                });
                indicatorsContainer.appendChild(dot);
            }
        }

        function updateIndicators() {
            const dots = indicatorsContainer ? indicatorsContainer.querySelectorAll('.indicator') : [];
            dots.forEach(function (dot, i) {
                dot.classList.toggle('active', i === currentIndex);
            });
        }

        function goTo(index) {
            const maxIndex = getMaxIndex();
            currentIndex = Math.max(0, Math.min(index, maxIndex));
            const slideWidth = slides[0].getBoundingClientRect().width;
            track.style.transform = 'translateX(-' + (currentIndex * slideWidth) + 'px)';
            updateIndicators();
        }

        function next() {
            goTo(currentIndex >= getMaxIndex() ? 0 : currentIndex + 1);
        }

        function prev() {
            goTo(currentIndex <= 0 ? getMaxIndex() : currentIndex - 1);
        }

        function restartAutoPlay() {
            if (autoTimer) clearInterval(autoTimer);
            autoTimer = setInterval(next, 5000);
        }

        if (prevBtn) {
            prevBtn.addEventListener('click', function () {
                prev();
                restartAutoPlay();
            });
        }

        if (nextBtn) {
            nextBtn.addEventListener('click', function () {
                next();
                restartAutoPlay();
            });
        }

        let resizeTimer;
        window.addEventListener('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                if (currentIndex > getMaxIndex()) {
                    currentIndex = getMaxIndex();
                }
                buildIndicators();
                goTo(currentIndex);
            }, 150);
        });

        buildIndicators();
        goTo(0);
        restartAutoPlay();
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initWorkoutSlider);
    } else {
        initWorkoutSlider();
    }
})();
