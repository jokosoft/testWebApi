$(document).ready(function () {
    var grid = $("#gvRestaurantes").bootgrid({
        ajax: true,
        rowselect: true,
        ajaxSettings: {
            method: "GET",
            cache: false
        },
        post: function () {

            return {
                id: "71F4F3C9-B3BF-4616-9C57-79100B313A7C"
            };
        },
        url: "/api/Restaurantes",
        searchSettings: {
            delay: 1000,
            characters: 3
        },
        labels: {
            all: "ALL",
            infos: "Este es el listado de restaurantes",
            loading: "Obteniendo datos ...",
            noResults: "Ups!! No hay resultados",
            refresh: "Recargar los datos",
            search: "buscar..."
        },
        formatters: {
            "commands": function (column, row) {
                return "<a class='btn btn-warning btn-xs btnEditarRestaurante' data-id='"
                    + row.Id_Restaurante + "'>Editar</a> &nbsp; "
                    + "<a class='btn btn-danger btn-xs btnEliminarRestaurante'"
                    + " data-id='" + row.Id_Restaurante + "'>Eliminar</a>"
            }
        }
    }).on("loaded.rs.jquery.bootgrid", function (e) {
        // la tabla está cargada
        asignarEventosBotones();
    });


    // Muestra la venta modal y Carga en el div del contenido de la ventana el formulario de alta del nuevo registro
    $("#btnNuevoRestaurante").click(function (event) {

        // estable el titulo de la ventana
        $('#idTituloVentanaRestuarantes').html('Nuevo Restaurante');

        // carga el contenido de la ventana
        $("#idContenidoVentanaRestaurantes")
            .load("/Restaurantes/Create", () => {
                activarValidaciones('idFormNuevoRestaurante');
            });
    });

    function asignarEventosBotones() {
        // Muestra la venta modal y Carga en el div del contenido de la ventana modal el formulario de edicion, con los datos del registro seleccionado
        $(".btnEditarRestaurante").click(function (event) {

            // estable el titulo de la ventana
            $('#idTituloVentanaRestuarantes').html('Modificar Restaurante');

            // carga el contenido de la ventana
            $('#idContenidoVentanaRestaurantes').load("/Restaurantes/Edit/" + $(this).data("id"), () => {
                activarValidaciones('idFormEditarRestaurante');
            });

            // muestra la ventana modal
            $('#pnlRestaurante').modal('toggle');
        });

        $(".btnEliminarRestaurante").click(function (event) {

            // estable el titulo de la ventana
            $('#idTituloVentanaRestuarantes').html('Eliminar Restaurante');

            // carga el contenido de la ventana
            $('#idContenidoVentanaRestaurantes').load("/Restaurantes/Delete/" + $(this).data("id"), () => {
                activarValidaciones('idFormEditarRestaurante');
            });

            // muestra la ventana modal
            $('#pnlRestaurante').modal('toggle');
        });
    }

});


