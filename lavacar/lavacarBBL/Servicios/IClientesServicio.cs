using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarBLL.Dtos;

namespace lavacarBLL.Servicios
{
    public interface IClientesServicio
    {
        Task<CustomResponse<ClienteDto>> ObtenerClientePorIdAsync(int id);
        Task<CustomResponse<List<ClienteDto>>> ObtenerClientesAsync();
        Task<CustomResponse<ClienteDto>> AgregarClienteAsync(ClienteDto clienteDto);
        Task<CustomResponse<ClienteDto>> ActualizarClienteAsync(ClienteDto clienteDto);
        Task<CustomResponse<ClienteDto>> EliminarClienteAsync(int id);
    }
}