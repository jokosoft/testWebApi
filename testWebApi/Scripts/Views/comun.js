function activarValidaciones(idDivContenedorFormulario) {
    var form = $('#' + idDivContenedorFormulario ).closest('form');
    form.removeData('validator');
    form.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);
}

function cierraModal(idModal) {
    $('#' + idModal).modal('toggle');
}