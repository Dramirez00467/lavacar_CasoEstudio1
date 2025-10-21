using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lavacarBLL.Dtos
{
    public class VehiculoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Placa es obligatoria.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Marca es obligatoria.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Modelo es obligatorio.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "Año es obligatorio.")]
        public int? Anio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente.")]
        public int? IdCliente { get; set; }
    }
}