using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using lavacarBLL.Dtos;
using lavacarDAL.Entidades;
using lavacarDAL.Repositorios;

namespace lavacarBLL.Servicios
{
    public class CitasServicio : ICitasServicio
    {
        // Inyección de dependencias
        private readonly ICitasRepositorio _citasRepositorio;
        private readonly IClientesRepositorio _clientesRepositorio;
        private readonly IVehiculosRepositorio _vehiculosRepositorio;
        private readonly IMapper _mapper;

        public CitasServicio(ICitasRepositorio citasRepositorio, IClientesRepositorio clientesRepositorio, IVehiculosRepositorio vehiculosRepositorio, IMapper mapper)
        {
            _citasRepositorio = citasRepositorio;
            _clientesRepositorio = clientesRepositorio;
            _vehiculosRepositorio = vehiculosRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<CitaDto>> ObtenerCitaPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<CitaDto>();

            var cita = await _citasRepositorio.ObtenerCitaPorIdAsync(id);
            var validaciones = validar(cita);
            if (validaciones.EsError)
            {
                return validaciones;
            }

            respuesta.Data = _mapper.Map<CitaDto>(cita);
            return respuesta;
        }

        public async Task<CustomResponse<List<CitaDto>>> ObtenerCitasAsync()
        {
            var respuesta = new CustomResponse<List<CitaDto>>();
            var citas = await _citasRepositorio.ObtenerCitasAsync();
            var citasDto = _mapper.Map<List<CitaDto>>(citas);
            respuesta.Data = citasDto;
            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> AgregarCitaAsync(CitaDto citaDto)
        {
            var respuesta = new CustomResponse<CitaDto>();

            // Validación de cliente
            var cliente = await _clientesRepositorio.ObtenerClientePorIdAsync(citaDto.IdCliente ?? 0);
            if (cliente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe seleccionar un cliente válido";
                return respuesta;
            }

            // Validación de vehículo
            var vehiculo = await _vehiculosRepositorio.ObtenerVehiculoPorIdAsync(citaDto.IdVehiculo ?? 0);
            if (vehiculo == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe seleccionar un vehículo válido";
                return respuesta;
            }

            // Validar que el vehículo pertenezca al cliente
            if (vehiculo.IdCliente != cliente.Id)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "El vehículo no pertenece al cliente seleccionado";
                return respuesta;
            }

            // El repositorio me indica si pudo o no agregar la cita
            if (!await _citasRepositorio.AgregarCitaAsync(_mapper.Map<Cita>(citaDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo registrar la cita";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> ActualizarCitaAsync(CitaDto citaDto)
        {
            var respuesta = new CustomResponse<CitaDto>();
            var entidad = _mapper.Map<Cita>(citaDto);

            // Validación de cliente
            var cliente = await _clientesRepositorio.ObtenerClientePorIdAsync(entidad.IdCliente);
            if (cliente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe seleccionar un cliente válido";
                return respuesta;
            }

            // Validación de vehículo
            var vehiculo = await _vehiculosRepositorio.ObtenerVehiculoPorIdAsync(entidad.IdVehiculo);
            if (vehiculo == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe seleccionar un vehículo válido";
                return respuesta;
            }

            // Validar que el vehículo pertenezca al cliente
            if (vehiculo.IdCliente != cliente.Id)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "El vehículo no pertenece al cliente seleccionado";
                return respuesta;
            }

            if (!await _citasRepositorio.ActualizarCitaAsync(entidad))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar la cita";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<CitaDto>> EliminarCitaAsync(int id)
        {
            var respuesta = new CustomResponse<CitaDto>();

            if (!await _citasRepositorio.EliminarCitaAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar la cita";
                return respuesta;
            }

            return respuesta;
        }

        // Cambia el estado de una cita
        public async Task<CustomResponse<CitaDto>> CambiarEstadoAsync(int id, string nuevoEstado)
        {
            var respuesta = new CustomResponse<CitaDto>();

            var cita = await _citasRepositorio.ObtenerCitaPorIdAsync(id);
            var validaciones = validar(cita);
            if (validaciones.EsError)
                return validaciones;

            if (!Enum.TryParse<EstadoCita>(nuevoEstado, true, out var estadoEnum))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Estado inválido. Use: Ingresada, Cancelada o Concluida";
                return respuesta;
            }

            cita.Estado = estadoEnum;

            if (!await _citasRepositorio.ActualizarCitaAsync(cita))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo cambiar el estado de la cita";
                return respuesta;
            }

            return respuesta;
        }



        private CustomResponse<CitaDto> validar(Cita cita)
        {
            var respuesta = new CustomResponse<CitaDto>();

            if (cita == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cita no encontrada";
                return respuesta;
            }

            if (cita.IdCliente <= 0)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La cita debe tener un cliente válido";
                return respuesta;
            }

            if (cita.IdVehiculo <= 0)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La cita debe tener un vehículo válido";
                return respuesta;
            }

            if (cita.Fecha == default)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La fecha de la cita es obligatoria";
                return respuesta;
            }

            if (!Enum.IsDefined(typeof(EstadoCita), cita.Estado))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Estado de la cita inválido";
                return respuesta;
            }

            return respuesta;
        }
    }
}