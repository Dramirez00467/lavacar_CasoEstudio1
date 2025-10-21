using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lavacarDAL.Entidades
{
    public class Vehiculo
    {
        public int Id { get; set; }

        public string Placa { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public int Anio { get; set; }

        // Relación con Cliente
        public int IdCliente { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}