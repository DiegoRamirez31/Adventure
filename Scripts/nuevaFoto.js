$(document).ready(function () {
    $('#pathFoto').change(function (event) {
        var archivo = event.target.files[0];
        var lector = new FileReader();

        lector.onload = function (e) {
            $('#vistaPreviaFoto').attr('src', e.target.result).show();
        }

        lector.readAsDataURL(archivo);
    });

    $('#nuevaFoto #cancelar').click(function () {
        // Limpiar formulario y vista previa
        $('#nuevaFotoForm')[0].reset();
        $('#vistaPreviaFoto').hide();
    });
});