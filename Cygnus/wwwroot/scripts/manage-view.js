$(document).ready(function () {
    console.log("jQuery is ready!");
});

function deleteProduct(productId) {
    console.log("Deleting product with ID:", productId);
    $.ajax({
        type: "POST",
        url: "/Manage/Delete",
        data: { id: productId },
        success: function () {
            location.reload(); // Reload the page after deleting the product
        }
    });
}
