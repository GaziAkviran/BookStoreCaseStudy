$('li.dropdown').hover(function() {
    $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn(500);
  }, function() {
    $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut(500);
  });


/* Book Card Sectiob */
const prevBtn = document.querySelector('.prev-btn');
const nextBtn = document.querySelector('.next-btn');
const bookSlider = document.querySelector('.book-slider');

let scrollAmount = 0;

nextBtn.addEventListener('click', () => {
  scrollAmount += 200;
  bookSlider.style.transform = `translateX(-${scrollAmount}px)`;
});

prevBtn.addEventListener('click', () => {
  scrollAmount -= 200;
  if(scrollAmount < 0) scrollAmount = 0;
  bookSlider.style.transform = `translateX(-${scrollAmount}px)`;
});

$(document).ready(function () {
    $('#cartDropdown').on('show.bs.dropdown', function () {
        $.get('/Cart/GetCartPopup', function (data) {
            $('#cartDropdownMenu').html(data);
        });
    });
});
