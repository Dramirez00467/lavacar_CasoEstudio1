using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarDAL.Entidades;

namespace lavacarDAL.Repositorios
{
    public class CitasRepositorio : ICitasRepositorio
    {
        private List<Cita> citas = new List<Cita>()
        {
            new Cita { Id = 1, IdCliente=1, IdVehiculo=1, Fecha=DateTime.Today.AddDays(1), Estado=EstadoCita.Ingresada, FechaCreacion = DateTime.Now },
            new Cita { Id = 2, IdCliente=2, IdVehiculo=2, Fecha=DateTime.Today.AddDays(2), Estado=EstadoCita.Concluida, FechaCreacion = DateTime.Now },
            new Cita { Id = 3, IdCliente=3, IdVehiculo=3, Fecha=DateTime.Today.AddDays(3), Estado=EstadoCita.Cancelada, FechaCreacion = DateTime.Now }
        };

        public async Task<List<Cita>> ObtenerCitasAsync()
        {
            return citas;
        }

        public async Task<Cita> ObtenerCitaPorIdAsync(int id)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            return cita;
        }

        public async Task<bool> AgregarCitaAsync(Cita cita)
        {
            cita.Id = citas.Any() ? citas.Max(c => c.Id) + 1 : 1;
            cita.FechaCreacion = DateTime.Now;
            citas.Add(cita);
            return true;
        }

        public async Task<bool> ActualizarCitaAsync(Cita cita)
        {
            var existente = citas.FirstOrDefault(c => c.Id == cita.Id);
            if (existente == null) return false;

            existente.IdCliente = cita.IdCliente;
            existente.IdVehiculo = cita.IdVehiculo;
            existente.Fecha = cita.Fecha;
            existente.Estado = cita.Estado;
            return true;
        }

        public async Task<bool> EliminarCitaAsync(int id)
        {
            var cita = citas.FirstOrDefault(c => c.Id == id);
            if (cita != null)
            {
                citas.Remove(cita);
                return true;
            }
            return false;
        }
    }
}