$(document).ready(function () {
    let currentIndex = 0;
    const $carouselInner = $('#carrusel .carousel-inner');
    const $panels = $carouselInner.children('.panel');
    const totalPanels = $panels.length;

    function showNextPanel() {
        $panels.eq(currentIndex).fadeOut(1000, function () {
            currentIndex = (currentIndex + 1) % totalPanels;
            $panels.eq(currentIndex).fadeIn(1000);
        });
    }

    // Hide all panels except the first one
    $panels.hide().eq(currentIndex).show();

    // Set interval to show next panel
    setInterval(showNextPanel, 3000); // Change every 3 seconds
});
