const searchButton = document.querySelector('#searchButton');
const searchInput = document.querySelector('#searchInput');

searchButton.addEventListener('click', () => {
    const searchText = searchInput.value;
    if (searchText.length > 0) {
        Search(searchText); // assuming that Search method is in a script tag or an imported module
    }
});

let slideIndex = 0;
const slider = document.querySelector('.slider');
const productWidth = document.querySelector('.cygnus-product').clientWidth;
const sliderWidth = productWidth * 3; // show 3 products at a time

slider.style.width = sliderWidth + 'px';

function slide(n) {
    slideIndex += n;
    if (slideIndex < 0) {
        slideIndex = 0;
    } else if (slideIndex > slider.children.length - 1) {
        slideIndex = slider.children.length - 1;
    }
    slider.style.transform = `translateX(-${slideIndex * productWidth}px)`;
}

function redirectToProductPage(id) {
    window.location.href = "/Product?id=" + id;
}
