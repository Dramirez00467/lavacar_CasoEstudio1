using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lavacarBLL.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellido es obligatorio.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Identificación es obligatoria.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Edad es obligatoria.")]
        public int? Edad { get; set; }

        [Required(ErrorMessage = "Teléfono es obligatorio.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Correo no tiene formato válido.")]
        public string Correo { get; set; }
    }
}