// Aislar funciones
(() => {
    const Citas = {
        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {
            this.tabla = $('#tablaCitas').DataTable({
                ajax: {
                    url: '/Cita/ObtenerCitas',
                    type: 'GET',
                    dataSrc: 'data'
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'cliente', title: 'Cliente' },   // <-- nombre string
                    { data: 'vehiculo', title: 'Vehículo' }, // <-- nombre string
                    { data: 'fecha', title: 'Fecha' },
                    { data: 'estado', title: 'Estado' },
                    {
                        data: null,
                        title: 'Acción',
                        orderable: false,
                        render: function (data, type, row) {
                            return `
                                <button class="btn btn-sm btn-primary editar" data-id="${row.id}">
                                    Editar Estado
                                </button>
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
            $('#tablaCitas').on('click', '.editar', function () {
                const id = $(this).data('id');
                Citas.CargaDatosCita(id);
            });

            $('#btnEditarCambios').on('click', function () {
                Citas.EditarCita();
            });

            $('#btnGuardarCambios').on('click', function () {
                Citas.GuardarCita();
            });
        },

        GuardarCita: function () {
            const form = $('#formCrearCita');
            if (!form.valid()) return;

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalCrearCita').modal('hide');
                        Citas.tabla.ajax.reload();
                        form[0].reset();
                        Swal.fire({ title: 'Éxito', text: response.mensaje || 'Cita creada', icon: 'success' });
                    } else {
                        Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                    }
                }
            });
        },

        CargaDatosCita: function (id) {
            $.get(`/Cita/ObtenerCitaPorId/${id}`, function (response) {
                if (!response.esError) {
                    const c = response.data;
                    $('#CitaId').val(c.id);
                    $('#IdCliente').val(c.idCliente);
                    $('#IdVehiculo').val(c.idVehiculo);
                    $('#ClienteNombre').val(c.clienteNombre);
                    $('#VehiculoNombre').val(c.vehiculoNombre);
                    $('#Fecha').val(c.fecha?.substring(0, 10));
                    $('#Estado').val(c.estado);
                    $('#modalEditarCita').modal('show');
                } else {
                    Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                }
            });
        },

        EditarCita: function () {
            let form = $('#formEditarCita');
            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalEditarCita').modal('hide');
                        Citas.tabla.ajax.reload();
                        Swal.fire({ title: 'Éxito', text: response.mensaje || 'Estado actualizado', icon: 'success' });
                    } else {
                        Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                    }
                }
            });
        }
    }

    $(document).ready(() => Citas.init());
})();