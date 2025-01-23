var dataTable;

document.addEventListener("DOMContentLoaded", function () {
    loadDataTable(); // Llama a la función para cargar el DataTable cuando el DOM esté listo.
});

function loadDataTable() {
    dataTable = $('#tblCategories').DataTable({
        "ajax": {
            "url": "/admin/categories/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "50%" },
            { "data": "order", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Categories/Edit/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-edit"></i> Editar
                            </a>
                            <a onclick=Delete("/Admin/Categories/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i> Eliminar
                            </a>
                        </div>
                    `;
                }, "width": "25%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "¿Estás seguro de que deseas eliminar la categoría?",
        text: "¡Una vez eliminada, no podrás recuperar los datos!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (err) {
                    toastr.error("Hubo un error al procesar la solicitud.");
                }
            });
        }
    });
}

