using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarBLL.Dtos;

namespace lavacarBLL.Servicios
{
    public interface ICitasServicio
    {
        Task<CustomResponse<CitaDto>> ObtenerCitaPorIdAsync(int id);
        Task<CustomResponse<List<CitaDto>>> ObtenerCitasAsync();
        Task<CustomResponse<CitaDto>> AgregarCitaAsync(CitaDto citaDto);
        Task<CustomResponse<CitaDto>> ActualizarCitaAsync(CitaDto citaDto);
        Task<CustomResponse<CitaDto>> EliminarCitaAsync(int id);
        Task<CustomResponse<CitaDto>> CambiarEstadoAsync(int id, string nuevoEstado);
    }
}