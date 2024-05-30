$(document).ready(function () {
    $(".delete-link").click(function (e) {
        e.preventDefault(); // Evitar el comportamiento predeterminado del enlace

        var url = $(this).attr("href");
        var fotoId = $(this).data("foto-id");

        if (confirm("¿Estás seguro de que quieres eliminar esta foto?")) {
            window.location.href = url;
        }
    });
});