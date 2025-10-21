using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lavacarBLL.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int? IdCliente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo.")]
        public int? IdVehiculo { get; set; }

        [Required(ErrorMessage = "Debe indicar la fecha de la cita.")]
        public DateTime? Fecha { get; set; }

        [Required(ErrorMessage = "Debe indicar el estado de la cita.")]
        public string Estado { get; set; }  // "Ingresada", "Cancelada", "Concluida"
    }
}