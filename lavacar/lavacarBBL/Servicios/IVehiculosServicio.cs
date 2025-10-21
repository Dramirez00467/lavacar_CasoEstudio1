using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lavacarBLL.Dtos;

namespace lavacarBLL.Servicios
{
    public interface IVehiculosServicio
    {
        Task<CustomResponse<VehiculoDto>> ObtenerVehiculoPorIdAsync(int id);
        Task<CustomResponse<List<VehiculoDto>>> ObtenerVehiculosAsync();
        Task<CustomResponse<VehiculoDto>> AgregarVehiculoAsync(VehiculoDto vehiculoDto);
        Task<CustomResponse<VehiculoDto>> ActualizarVehiculoAsync(VehiculoDto vehiculoDto);
        Task<CustomResponse<VehiculoDto>> EliminarVehiculoAsync(int id);
    }
}