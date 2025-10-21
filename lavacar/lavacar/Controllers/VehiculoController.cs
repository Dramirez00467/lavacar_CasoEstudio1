using lavacarBLL.Dtos;
using lavacarBLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lavacar.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly ILogger<VehiculoController> _logger;
        private readonly IVehiculosServicio _vehiculosServicio;
        private readonly IClientesServicio _clientesServicio;

        public VehiculoController(
            ILogger<VehiculoController> logger,
            IVehiculosServicio vehiculosServicio,
            IClientesServicio clientesServicio)
        {
            _logger = logger;
            _vehiculosServicio = vehiculosServicio;
            _clientesServicio = clientesServicio;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Listado de Vehículos";

            // Dropdown de Clientes para los modales de Create/Edit
            var clientesResp = await _clientesServicio.ObtenerClientesAsync();
            ViewBag.Clientes = (clientesResp.Data ?? new List<ClienteDto>())
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} {c.Apellido}"
                })
                .ToList();

            return View();
        }

        public async Task<IActionResult> ObtenerVehiculos()
        {
            var respuesta = await _vehiculosServicio.ObtenerVehiculosAsync();
            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearVehiculo(VehiculoDto vehiculoDto)
        {
            if (!ModelState.IsValid)
                return Json(new CustomResponse<VehiculoDto> { EsError = true, Mensaje = "Error de validación" });

            var response = await _vehiculosServicio.AgregarVehiculoAsync(vehiculoDto);
            return Json(response);
        }

        public async Task<IActionResult> ObtenerVehiculoPorId(int id)
        {
            var respuesta = await _vehiculosServicio.ObtenerVehiculoPorIdAsync(id);
            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarVehiculo(VehiculoDto vehiculoDto)
        {
            if (!ModelState.IsValid)
                return Json(new CustomResponse<VehiculoDto> { EsError = true, Mensaje = "Error de validación" });

            var respuesta = await _vehiculosServicio.ActualizarVehiculoAsync(vehiculoDto);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarVehiculo(int id)
        {
            var respuesta = await _vehiculosServicio.EliminarVehiculoAsync(id);
            return Json(respuesta);
        }
    }
}