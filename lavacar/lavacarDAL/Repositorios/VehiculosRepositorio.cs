using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarDAL.Entidades;

namespace lavacarDAL.Repositorios
{
    public class VehiculosRepositorio : IVehiculosRepositorio
    {
        private List<Vehiculo> vehiculos = new List<Vehiculo>()
        {
            new Vehiculo { Id = 1, Placa="ABC123", Marca="Toyota", Modelo="Yaris", Anio=2020, IdCliente=1, FechaCreacion = DateTime.Now },
            new Vehiculo { Id = 2, Placa="BCD234", Marca="Honda", Modelo="Civic", Anio=2019, IdCliente=2, FechaCreacion = DateTime.Now },
            new Vehiculo { Id = 3, Placa="CDE345", Marca="Nissan", Modelo="Versa", Anio=2021, IdCliente=3, FechaCreacion = DateTime.Now }
        };

        public async Task<List<Vehiculo>> ObtenerVehiculosAsync()
        {
            return vehiculos;
        }

        public async Task<Vehiculo> ObtenerVehiculoPorIdAsync(int id)
        {
            var vehiculo = vehiculos.FirstOrDefault(v => v.Id == id);
            return vehiculo;
        }

        public async Task<bool> AgregarVehiculoAsync(Vehiculo vehiculo)
        {
            vehiculo.Id = vehiculos.Any() ? vehiculos.Max(v => v.Id) + 1 : 1;
            vehiculo.FechaCreacion = DateTime.Now;
            vehiculos.Add(vehiculo);
            return true;
        }

        public async Task<bool> ActualizarVehiculoAsync(Vehiculo vehiculo)
        {
            var existente = vehiculos.FirstOrDefault(v => v.Id == vehiculo.Id);
            if (existente == null) return false;

            existente.Placa = vehiculo.Placa;
            existente.Marca = vehiculo.Marca;
            existente.Modelo = vehiculo.Modelo;
            existente.Anio = vehiculo.Anio;
            existente.IdCliente = vehiculo.IdCliente;
            return true;
        }

        public async Task<bool> EliminarVehiculoAsync(int id)
        {
            var vehiculo = vehiculos.FirstOrDefault(v => v.Id == id);
            if (vehiculo != null)
            {
                vehiculos.Remove(vehiculo);
                return true;
            }
            return false;
        }
    }
}