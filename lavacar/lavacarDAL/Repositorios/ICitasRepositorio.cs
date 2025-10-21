using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarDAL.Entidades;

namespace lavacarDAL.Repositorios
{
    public interface ICitasRepositorio
    {
        Task<List<Cita>> ObtenerCitasAsync();
        Task<Cita> ObtenerCitaPorIdAsync(int id);
        Task<bool> AgregarCitaAsync(Cita cita);
        Task<bool> ActualizarCitaAsync(Cita cita);
        Task<bool> EliminarCitaAsync(int id);
    }
}