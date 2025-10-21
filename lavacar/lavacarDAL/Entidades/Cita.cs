using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lavacarDAL.Entidades
{

    // Enumeración para los estados de la cita
    public enum EstadoCita
    {
        Ingresada,
        Cancelada,
        Concluida
    }

    public class Cita
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public int IdVehiculo { get; set; }

        public DateTime Fecha { get; set; }

        public EstadoCita Estado { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}