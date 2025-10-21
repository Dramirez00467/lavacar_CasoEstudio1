// Aislar funciones 
(() => {
    const Vehiculos = {
        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },
        inicializarTabla() {
            this.tabla = $('#tablaVehiculos').DataTable({
                ajax: {
                    url: '/Vehiculo/ObtenerVehiculos',
                    type: 'GET',
                    dataSrc: 'data'
                },
                columns: [
                    { data: 'id', title: 'ID' },
                    { data: 'placa', title: 'Placa' },
                    { data: 'marca', title: 'Marca' },
                    { data: 'modelo', title: 'Modelo' },
                    { data: 'anio', title: 'Año' },
                    { data: 'idCliente', title: 'Cliente ID' },
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

            $('#tablaVehiculos').on('click', '.editar', function () {
                const id = $(this).data('id');
                Vehiculos.CargaDatosVehiculo(id);
            });

            $('#tablaVehiculos').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Vehiculos.EliminarVehiculo(id);
            });

            // mismos IDs que Usuarios
            $('#btnGuardarCambios').on('click', function () {
                Vehiculos.GuardarVehiculo();
            });

            $('#btnEditarCambios').on('click', function () {
                Vehiculos.EditarVehiculo();
            });
        },
        GuardarVehiculo: function () {
            let form = $('#formCrearVehiculo');
            if (!form.valid()) {
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalCrearVehiculo').modal('hide');
                        Vehiculos.tabla.ajax.reload();
                        form[0].reset();
                        Swal.fire({ title: 'Éxito', text: response.mensaje, icon: 'success' });
                    } else {
                        Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                    }
                }
            });

        },

        CargaDatosVehiculo: function (id) {
            $.get(`/Vehiculo/ObtenerVehiculoPorId/${id}`, function (response) {
                if (!response.esError) {
                    const v = response.data;
                    $('#VehiculoId').val(v.id);
                    $('#Placa').val(v.placa);
                    $('#Marca').val(v.marca);
                    $('#Modelo').val(v.modelo);
                    $('#Anio').val(v.anio);
                    $('#IdCliente').val(v.idCliente);
                    $('#modalEditarVehiculo').modal('show');
                } else {
                    Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                }
            });

        },

        EditarVehiculo: function () {
            let form = $('#formEditarVehiculo');
            if (!form.valid()) return;

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (response) {
                    if (!response.esError) {
                        $('#modalEditarVehiculo').modal('hide');
                        Vehiculos.tabla.ajax.reload();
                        Swal.fire({ title: 'Éxito', text: response.mensaje, icon: 'success' });
                    } else {
                        Swal.fire({ title: 'Error', text: response.mensaje, icon: 'error' });
                    }
                }
            });
        },
        EliminarVehiculo: function (id) {
            Swal.fire({
                title: "Estas seguro?",
                text: "No podras revertir esta acción",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, borrar"
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/Vehiculo/EliminarVehiculo',
                        type: 'POST',
                        data: { id: id },
                        success: function (response) {
                            if (!response.esError) {
                                Vehiculos.tabla.ajax.reload();
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
    $(document).ready(() => Vehiculos.init());
})();