using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarDAL.Entidades;

namespace lavacarDAL.Repositorios
{
    public interface IVehiculosRepositorio
    {
        Task<List<Vehiculo>> ObtenerVehiculosAsync();
        Task<Vehiculo> ObtenerVehiculoPorIdAsync(int id);
        Task<bool> AgregarVehiculoAsync(Vehiculo vehiculo);
        Task<bool> ActualizarVehiculoAsync(Vehiculo vehiculo);
        Task<bool> EliminarVehiculoAsync(int id);
    }
}