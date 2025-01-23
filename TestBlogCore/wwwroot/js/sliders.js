var dataTable;

document.addEventListener("DOMContentLoaded", function () {
    loadDataTable(); // Llama a la función para cargar el DataTable cuando el DOM esté listo.
    console.log("DOM cargado");
});

function loadDataTable() {
        dataTable = $('#tlbSliders').DataTable({
            "ajax": {
                "url": "/admin/sliders/GetAll",
                "type": "GET",
                "datatype": "json"
            },
            "columns": [
                { "data": "id", "width": "5%" },
                { "data": "name", "width": "20%" },
                {
                    "data": "urlImage",
                    "render": function (image) {
                        if (image) {
                            return `<img src="${image}" width="120px" alt="Slider" class="img-thumbnail"/>`;
                        } else {
                            return `<span class="text-muted">Sin imagen</span>`;
                        }
                    },
                    "width": "20%"
                },
                {
                    "data": "state",
                    "render": function (state) {
                        return state
                            ? `<span class="badge bg-success">Activo</span>`
                            : `<span class="badge bg-danger">Inactivo</span>`;
                    },
                    "width": "10%"
                },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                        <div class="text-center">
                            <a href="/Admin/Sliders/Edit/${data}" class="btn btn-success text-white me-2" style="cursor:pointer">
                                <i class="fas fa-edit"></i> Editar
                            </a>
                            <button onclick="Delete('/Admin/Sliders/Delete/${data}')" class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i> Eliminar
                            </button>
                        </div>
                    `;
                    },
                    "width": "25%"
                }
            ],
            "responsive": true,
            "autoWidth": false
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
