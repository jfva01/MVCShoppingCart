function limpiarModal() {
    $("#txtId").val(0);
    $("#txtDescripcion").val("");
    $("#cboActivo").prop("selectedIndex", 0).val(); //Muestra el valor del primer elemento del select.
}

function openModal(json) {
    limpiarModal();

    $("#mensajeError").hide();

    if (json != null) {
        //Si la variable json tiene datos, los asigna a cada input.
        $("#txtId").val(json.idCategoria);
        $("#txtDescripcion").val(json.Descripcion);
        $("#cboActivo").val(json.Activa == true ? 1 : 0); //Boolean: Si el dato recibido es true le asigna el valor 1, de lo contrario le asigna 0.
    }

    $("#frmModal").modal("show");
}

$("#tblDatosCategoria tbody").on("click", '.btn-editar', function () {
    //Obtener la fila seleccionada
    selectedRow = $(this).closest("tr");
    //Obtener datos de la fila seleccionada
    let data = dataTable.row(selectedRow).data();
    //Abrir modal
    openModal(data);
})

$("#tblDatosCategoria tbody").on("click", '.btn-eliminar', function () {
    //Obtener la fila seleccionada
    selectedCategory = $(this).closest("tr");
    //Obtener datos de la fila seleccionada
    let data = dataTable.row(selectedCategory).data();
    swal({
        title: "Eliminar",
        text: "Seguro desea eliminar este registro?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-dark",
        cancelButtonClass: "btn-danger",
        confirmButtonText: "Aceptar",
        cancelButtonText: "Cancelar",
        closeOnConfirm: true
    },
        function () {
            jQuery.ajax({
                url: '@Url.Action("EliminarCategoria", "Mantenimiento")',
                type: "POST",
                data: JSON.stringify({ idCategoria: data.idCategoria }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.resultado) {
                        dataTable.row(selectedCategory).remove().draw();
                        swal("Registro eliminado correctamente", data.mensaje, "Confirmación")
                    } else {
                        swal("No se pudo eliminar el registro", data.mensaje, "Error")
                    }
                },
                error: function (error) {
                    console.log(error)
                }
            });
        });
})

function Guardar() {
    let categoria = {
        idCategoria: $("#txtId").val(),
        Descripcion: $("#txtDescripcion").val(),
        Activa: $("#cboActivo").val() == 1 ? true : false
    }

    jQuery.ajax({
        url: '@Url.Action("GuardarCategoria", "Mantenimiento")',
        type: "POST",
        data: JSON.stringify({ obj: categoria }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $(".modal-body").LoadingOverlay("hide");

            if (categoria.idCategoria == 0) {
                // Crear categoria
                if (data.resultado != 0) {
                    categoria.idCategoria = data.resultado;
                    dataTable.row.add(categoria).draw(false);
                    $("#frmModal").modal("hide");
                } else {
                    $("#mensajeError").text(data.mensaje);
                    $("#mensajeError").show();
                }
            } else {
                // Editar Categoria
                if (data.resultado) {
                    dataTable.row(selectedRow).data(categoria).draw(false);
                    selectedRow = null;
                    $("#frmModal").modal("hide");
                } else {
                    $("#mensajeError").text(data.mensaje);
                    $("#mensajeError").show();
                }
            }
        },
        error: function (error) {
            $(".modal-body").LoadingOverlay("hide");
            $("#mensajeError").text("Error ajax");
            $("#mensajeError").show();
        },
        beforeSend: function () {
            $(".modal-body").LoadingOverlay("show", {
                imageResizeFactor: 2,
                text: "Cargando...",
                size: 14
            })
        }
    });


}