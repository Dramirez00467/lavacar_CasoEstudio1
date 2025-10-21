// Aislar funciones 
(() => {
    const Clientes = {
        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },
        inicializarTabla() {
            this.tabla = $('#tablaClientes').DataTable({
                ajax: {
                    url: '/Cliente/ObtenerClientes',
                    type: 'GET',
                    dataSrc: 'data'
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'nombre', title: 'Nombre' },
                    { data: 'apellido', title: 'Apellido' },
                    { data: 'identificacion', title: 'Identificación' },
                    { data: 'edad', title: 'Edad' },
                    { data: 'telefono', title: 'Teléfono' },
                    { data: 'correo', title: 'Correo' },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        render: function (data, type, row) {
                            return `
                                <button class="btn btn-sm btn-primary editar" data-id="${row.id}">Editar</button>
                                <button class="btn btn-sm btn-danger eliminar" data-id="${row.id}">Eliminar</button>
                            `;
                        }
                    }
                ],
                responsive: true,
                processing: true,
                pageLength: 10
            });
        },
        registrarEventos() {

            $('#tablaClientes').on('click', '.editar', function () {
                const id = $(this).data('id');
                Clientes.CargaDatosCliente(id);
            });

            $('#tablaClientes').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Clientes.EliminarCliente(id);
            });

            // mismos IDs que Usuarios
            $('#btnGuardarCambios').on('click', function () {
                Clientes.GuardarCliente();
            });

            $('#btnEditarCambios').on('click', function () {
                Clientes.EditarCliente();
            });
        },
        GuardarCliente: function () {
            let form = $('#formCrearCliente');

            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalCrearCliente').modal('hide');
                        Clientes.tabla.ajax.reload();
                        form[0].reset();
                        Swal.fire({ title: 'Éxito', text: response.mensaje, icon: 'success' });
                    } else {
                        Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                    }
                }
            });

        },

        CargaDatosCliente: function (id) {
            $.get(`/Cliente/ObtenerClientePorId/${id}`, function (response) {
                if (!response.esError) {
                    const c = response.data;
                    $('#ClienteId').val(c.id);
                    $('#Nombre').val(c.nombre);
                    $('#Apellido').val(c.apellido);
                    $('#Identificacion').val(c.identificacion);
                    $('#Edad').val(c.edad);
                    $('#Telefono').val(c.telefono);
                    $('#Correo').val(c.correo);
                    $('#modalEditarCliente').modal('show');
                } else {
                    Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                }
            });

        },

        EditarCliente: function () {
            let form = $('#formEditarCliente');
            if (!form.valid()) return;

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalEditarCliente').modal('hide');
                        Clientes.tabla.ajax.reload();
                        Swal.fire({ title: 'Éxito', text: response.mensaje, icon: 'success' });
                    } else {
                        Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                    }
                }
            });
        },
        EliminarCliente: function (id) {
            Swal.fire({
                title: "Estas seguro?",
                text: "No podras revertir esta acción",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, borrar"
            }).then((result) => {

                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Cliente/EliminarCliente',
                        type: 'POST',
                        data: { id: id },
                        success: function (response) {
                            if (!response.esError) {
                                Clientes.tabla.ajax.reload();
                                Swal.fire({ title: 'Éxito', text: response.mensaje, icon: 'success' });
                            } else {
                                Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                            }
                        }
                    });
                }
            });
        }

    }
    $(document).ready(() => Clientes.init());
})();