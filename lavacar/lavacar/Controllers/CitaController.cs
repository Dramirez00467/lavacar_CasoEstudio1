using lavacarBLL.Dtos;
using lavacarBLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lavacar.Controllers
{
    public class CitaController : Controller
    {
        private readonly ILogger<CitaController> _logger;
        private readonly ICitasServicio _citasServicio;
        private readonly IClientesServicio _clientesServicio;
        private readonly IVehiculosServicio _vehiculosServicio;

        public CitaController(
            ILogger<CitaController> logger,
            ICitasServicio citasServicio,
            IClientesServicio clientesServicio,
            IVehiculosServicio vehiculosServicio)
        {
            _logger = logger;
            _citasServicio = citasServicio;
            _clientesServicio = clientesServicio;
            _vehiculosServicio = vehiculosServicio;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Listado de Citas";

            // Dropdown de Clientes para Create/Edit
            var clientesResp = await _clientesServicio.ObtenerClientesAsync();
            ViewBag.Clientes = (clientesResp.Data ?? new List<ClienteDto>())
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Nombre} {c.Apellido}"
                })
                .ToList();

            // Dropdown de Vehículos
            var vehiculosResp = await _vehiculosServicio.ObtenerVehiculosAsync();
            ViewBag.Vehiculos = (vehiculosResp.Data ?? new List<VehiculoDto>())
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.Placa} - {v.Marca} {v.Modelo}"
                })
                .ToList();

            return View();
        }

        public async Task<IActionResult> ObtenerCitas()
        {
            var citasResp = await _citasServicio.ObtenerCitasAsync();
            var clientesResp = await _clientesServicio.ObtenerClientesAsync();
            var vehiculosResp = await _vehiculosServicio.ObtenerVehiculosAsync();


            // Mostrar el listado con nombres de clientes y vehículos
            var lista = citasResp.Data?.Select(c => new
            {
                c.Id,
                Cliente = clientesResp.Data?
                    .FirstOrDefault(x => x.Id == c.IdCliente)?.Nombre + " " +
                          clientesResp.Data?.FirstOrDefault(x => x.Id == c.IdCliente)?.Apellido,
                Vehiculo = vehiculosResp.Data?
                    .FirstOrDefault(v => v.Id == c.IdVehiculo)?.Placa + " - " +
                           vehiculosResp.Data?.FirstOrDefault(v => v.Id == c.IdVehiculo)?.Marca,
                Fecha = c.Fecha?.ToString("yyyy-MM-dd"),
                Estado = c.Estado
            }).ToList();

            return Json(new { data = lista });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCita(CitaDto citaDto)
        {
            if (!ModelState.IsValid)
                return Json(new CustomResponse<CitaDto> { EsError = true, Mensaje = "Error de validación" });

            var response = await _citasServicio.AgregarCitaAsync(citaDto);
            return Json(response);
        }


        // Obtener los detalles de una cita por su ID
        public async Task<IActionResult> ObtenerCitaPorId(int id)
        {
            var r = await _citasServicio.ObtenerCitaPorIdAsync(id);
            if (r.EsError || r.Data == null)
                return Json(new { esError = true, mensaje = r.Mensaje });

            // Obtener nombres de cliente y vehículo
            var cliente = await _clientesServicio.ObtenerClientePorIdAsync(r.Data.IdCliente ?? 0);
            var vehiculo = await _vehiculosServicio.ObtenerVehiculoPorIdAsync(r.Data.IdVehiculo ?? 0);

            // Formatear los nombres
            var clienteNombre = (cliente.Data != null) ? $"{cliente.Data.Nombre} {cliente.Data.Apellido}" : "";
            var vehiculoNombre = (vehiculo.Data != null) ? $"{vehiculo.Data.Placa} - {vehiculo.Data.Marca} {vehiculo.Data.Modelo}" : "";


            // Retornar los detalles de la cita junto con los nombres formateados
            return Json(new
            {
                esError = false,
                mensaje = r.Mensaje,
                data = new
                {
                    r.Data.Id,
                    r.Data.IdCliente,
                    r.Data.IdVehiculo,
                    r.Data.Fecha,
                    r.Data.Estado,
                    ClienteNombre = clienteNombre,
                    VehiculoNombre = vehiculoNombre
                }
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCita(CitaDto citaDto)
        {
            if (!ModelState.IsValid)
                return Json(new CustomResponse<CitaDto> { EsError = true, Mensaje = "Error de validación" });

            var respuesta = await _citasServicio.ActualizarCitaAsync(citaDto);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCita(int id)
        {
            var respuesta = await _citasServicio.EliminarCitaAsync(id);
            return Json(respuesta);
        }


        // Cambiar el estado de una cita
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstadoCita(int id, string nuevoEstado)
        {
            var respuesta = await _citasServicio.CambiarEstadoAsync(id, nuevoEstado);
            return Json(respuesta);
        }
    }
}