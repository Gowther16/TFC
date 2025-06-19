// Navigation functionality
window.addEventListener('scroll', function () {
    const firstNavItem = document.querySelector('.nav-category-item');
    if (window.scrollY === 0 && firstNavItem && !firstNavItem.classList.contains('active')) {
        firstNavItem.classList.add('active');
    } else if (window.scrollY !== 0 && firstNavItem && firstNavItem.classList.contains('active')) {
        firstNavItem.classList.remove('active');
    }

    const sections = document.querySelectorAll('section[id^="category-"], section[id="combo"]');
    const navItems = document.querySelectorAll('.nav-category-item');
    let current = '';

    sections.forEach(section => {
        const sectionTop = section.offsetTop - 120; // Account for fixed navbar height
        const sectionHeight = section.offsetHeight;

        if (window.scrollY >= sectionTop && window.scrollY < sectionTop + sectionHeight) {
            current = section.getAttribute('id');
        }
    });

    navItems.forEach(item => {
        item.classList.remove('active');
        const href = item.getAttribute('href');
        if (href === '#' + current) {
            item.classList.add('active');
        }
    });
});

// Smooth scroll for navigation items
document.querySelectorAll('.nav-category-item').forEach(item => {
    item.addEventListener('click', function (e) {
        e.preventDefault();

        const targetId = this.getAttribute('href').substring(1);
        const targetSection = document.getElementById(targetId);

        if (targetSection) {
            // Calculate offset for fixed navbar
            const navbarHeight = 120; // Adjust based on your navbar height
            const targetPosition = targetSection.offsetTop - navbarHeight;

            window.scrollTo({
                top: targetPosition,
                behavior: 'smooth'
            });

            // Update active state
            document.querySelectorAll('.nav-category-item').forEach(nav => nav.classList.remove('active'));
            this.classList.add('active');
        }
    });
});
