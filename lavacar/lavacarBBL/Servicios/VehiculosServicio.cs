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
    public class VehiculosServicio : IVehiculosServicio
    {
        // Inyección de dependencias
        private readonly IVehiculosRepositorio _vehiculosRepositorio;
        private readonly IClientesRepositorio _clientesRepositorio;
        private readonly IMapper _mapper;

        public VehiculosServicio(IVehiculosRepositorio vehiculosRepositorio, IClientesRepositorio clientesRepositorio, IMapper mapper)
        {
            _vehiculosRepositorio = vehiculosRepositorio;
            _clientesRepositorio = clientesRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<VehiculoDto>> ObtenerVehiculoPorIdAsync(int id)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            var vehiculo = await _vehiculosRepositorio.ObtenerVehiculoPorIdAsync(id);
            var validaciones = validar(vehiculo);
            if (validaciones.EsError)
                return validaciones;

            respuesta.Data = _mapper.Map<VehiculoDto>(vehiculo);
            return respuesta;
        }

        public async Task<CustomResponse<List<VehiculoDto>>> ObtenerVehiculosAsync()
        {
            var respuesta = new CustomResponse<List<VehiculoDto>>();
            var vehiculos = await _vehiculosRepositorio.ObtenerVehiculosAsync();
            var vehiculosDto = _mapper.Map<List<VehiculoDto>>(vehiculos);
            respuesta.Data = vehiculosDto;
            return respuesta;
        }

        public async Task<CustomResponse<VehiculoDto>> AgregarVehiculoAsync(VehiculoDto vehiculoDto)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            // Validación de cliente existente
            var cliente = await _clientesRepositorio.ObtenerClientePorIdAsync(vehiculoDto.IdCliente ?? 0);
            if (cliente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe asignar un cliente válido al vehículo";
                return respuesta;
            }

            // Validación de placa única
            var existentes = await _vehiculosRepositorio.ObtenerVehiculosAsync();
            if (existentes.Any(v => v.Placa == vehiculoDto.Placa))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ya existe un vehículo con esa placa";
                return respuesta;
            }

            // El repositorio me indica si pudo o no agregar el vehículo
            if (!await _vehiculosRepositorio.AgregarVehiculoAsync(_mapper.Map<Vehiculo>(vehiculoDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo agregar el vehículo";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<VehiculoDto>> ActualizarVehiculoAsync(VehiculoDto vehiculoDto)
        {
            var respuesta = new CustomResponse<VehiculoDto>();
            var vehiculo = _mapper.Map<Vehiculo>(vehiculoDto);

            // Validación de cliente existente
            var cliente = await _clientesRepositorio.ObtenerClientePorIdAsync(vehiculo.IdCliente);
            if (cliente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Debe asignar un cliente válido al vehículo";
                return respuesta;
            }

            // Validación de placa única
            var existentes = await _vehiculosRepositorio.ObtenerVehiculosAsync();
            if (existentes.Any(v => v.Placa == vehiculo.Placa && v.Id != vehiculo.Id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Placa ya registrada en otro vehículo";
                return respuesta;
            }

            if (!await _vehiculosRepositorio.ActualizarVehiculoAsync(vehiculo))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el vehículo";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<VehiculoDto>> EliminarVehiculoAsync(int id)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            if (!await _vehiculosRepositorio.EliminarVehiculoAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar el vehículo";
                return respuesta;
            }

            return respuesta;
        }

        private CustomResponse<VehiculoDto> validar(Vehiculo vehiculo)
        {
            var respuesta = new CustomResponse<VehiculoDto>();

            if (vehiculo == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Vehículo no encontrado";
                return respuesta;
            }

            if (string.IsNullOrWhiteSpace(vehiculo.Placa))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "La placa del vehículo es obligatoria";
                return respuesta;
            }

            if (vehiculo.IdCliente <= 0)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "El vehículo debe estar asociado a un cliente";
                return respuesta;
            }

            return respuesta;
        }
    }
}