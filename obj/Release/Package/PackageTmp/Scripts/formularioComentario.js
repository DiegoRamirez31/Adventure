$(document).ready(function () {
    $("#mostrarFormulario").click(function () {
        $("#formularioContainer").show();
        $("#formularioContainer").addClass('modal-open'); // Agregar clase para estilo de modal
        $("#mostrarFormulario").hide();
    });

    $("#btnCancelar").click(function () {
        $("#formulario")[0].reset(); // Limpia los campos del formulario
        $("#formularioContainer").removeClass('modal-open'); // Quitar clase de estilo de modal
        $("#formularioContainer").hide();
        $("#mostrarFormulario").show();
    });
});