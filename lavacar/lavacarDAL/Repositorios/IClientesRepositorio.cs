using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarDAL.Entidades;

namespace lavacarDAL.Repositorios
{
    public interface IClientesRepositorio
    {
        Task<List<Cliente>> ObtenerClientesAsync();
        Task<Cliente> ObtenerClientePorIdAsync(int id);
        Task<bool> AgregarClienteAsync(Cliente cliente);
        Task<bool> ActualizarClienteAsync(Cliente cliente);
        Task<bool> EliminarClienteAsync(int id);
    }
}