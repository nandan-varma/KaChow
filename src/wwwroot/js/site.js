// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

function SubmitRating(productId, rating) {
    fetch(`/api/Rating/add?productId=${productId}&rating=${rating}`, {
        method: 'POST'
    })
        .then(response => {
            if (response.ok) {
                console.log("Rating submitted successfully.");
                // Optionally, you can refresh the product data or update the UI here
            } else {
                alert("One user can only rate a product once. If you cant see changes in the rating, refresh the page to see the updated rating.");
                console.log("Failed to submit rating.");
                console.log(productId, rating);
            }
        })
        .catch(error => {
            console.error("Error submitting rating:", error);
        });
}

function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

document.addEventListener('DOMContentLoaded', () => {
    const backToTopButton = document.getElementById('back-to-top');
    if (backToTopButton) {
        backToTopButton.addEventListener('click', scrollToTop);
        window.addEventListener('scroll', () => {
            if (window.scrollY > 200) {
                backToTopButton.classList.add('visible');
            } else {
                backToTopButton.classList.remove('visible');
            }
        });
    }

    const themeToggle = document.getElementById('theme-toggle');
    const body = document.body;

    themeToggle.addEventListener('click', function () {
        body.classList.toggle('dark-theme');
        localStorage.setItem('theme', body.classList.contains('dark-theme') ? 'dark' : 'light');
    });

    if (localStorage.getItem('theme') === 'dark') {
        body.classList.add('dark-theme');
    }
});