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
    public class ClientesServicio : IClientesServicio
    {
        private readonly IClientesRepositorio _clientesRepositorio;
        private readonly IMapper _mapper;

        public ClientesServicio(IClientesRepositorio clientesRepositorio, IMapper mapper)
        {
            _clientesRepositorio = clientesRepositorio;
            _mapper = mapper;
        }

        public async Task<CustomResponse<ClienteDto>> ObtenerClientePorIdAsync(int id)
        {
            var respuesta = new CustomResponse<ClienteDto>();
            var cliente = await _clientesRepositorio.ObtenerClientePorIdAsync(id);

            var validacion = validar(cliente);
            if (validacion.EsError)
                return validacion;

            respuesta.Data = _mapper.Map<ClienteDto>(cliente);
            return respuesta;
        }

        public async Task<CustomResponse<List<ClienteDto>>> ObtenerClientesAsync()
        {
            var respuesta = new CustomResponse<List<ClienteDto>>();
            var clientes = await _clientesRepositorio.ObtenerClientesAsync();
            var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
            respuesta.Data = clientesDto;
            return respuesta;
        }

        public async Task<CustomResponse<ClienteDto>> AgregarClienteAsync(ClienteDto clienteDto)
        {
            var respuesta = new CustomResponse<ClienteDto>();

            // Validación de edad
            if (clienteDto.Edad < 18)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pueden registrar clientes menores de edad";
                return respuesta;
            }

            // Validación de identificación única
            var lista = await _clientesRepositorio.ObtenerClientesAsync();
            if (lista.Any(c => c.Identificacion == clienteDto.Identificacion))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Ya existe un cliente con esa identificación";
                return respuesta;
            }

            if (!await _clientesRepositorio.AgregarClienteAsync(_mapper.Map<Cliente>(clienteDto)))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo agregar el cliente";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<ClienteDto>> ActualizarClienteAsync(ClienteDto clienteDto)
        {
            var respuesta = new CustomResponse<ClienteDto>();
            var cliente = _mapper.Map<Cliente>(clienteDto);

            if (cliente.Edad < 18)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pueden registrar clientes menores de edad";
                return respuesta;
            }

            var lista = await _clientesRepositorio.ObtenerClientesAsync();
            if (lista.Any(c => c.Identificacion == cliente.Identificacion && c.Id != cliente.Id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Identificación ya registrada en otro cliente";
                return respuesta;
            }

            if (!await _clientesRepositorio.ActualizarClienteAsync(cliente))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo actualizar el cliente";
                return respuesta;
            }

            return respuesta;
        }

        public async Task<CustomResponse<ClienteDto>> EliminarClienteAsync(int id)
        {
            var respuesta = new CustomResponse<ClienteDto>();
            if (!await _clientesRepositorio.EliminarClienteAsync(id))
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "No se pudo eliminar el cliente";
                return respuesta;
            }
            return respuesta;
        }

        private CustomResponse<ClienteDto> validar(Cliente cliente)
        {
            var respuesta = new CustomResponse<ClienteDto>();

            if (cliente == null)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cliente no encontrado";
                return respuesta;
            }

            if (cliente.Edad < 18)
            {
                respuesta.EsError = true;
                respuesta.Mensaje = "Cliente menor de edad";
                return respuesta;
            }

            return respuesta;
        }
    }
}