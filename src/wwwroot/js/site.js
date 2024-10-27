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