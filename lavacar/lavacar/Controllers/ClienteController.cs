using lavacarBLL.Servicios;
using Microsoft.AspNetCore.Mvc;
using lavacarBLL.Dtos;


namespace lavacar.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly IClientesServicio _clientesServicio;

        public ClienteController(ILogger<ClienteController> logger, IClientesServicio clientesServicio)
        {
            _logger = logger;
            _clientesServicio = clientesServicio;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Listado de Clientes";
            return View();
        }

        public async Task<IActionResult> ObtenerClientes()
        {
            var respuesta = await _clientesServicio.ObtenerClientesAsync();
            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCliente(ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
                return Json(new CustomResponse<ClienteDto> { EsError = true, Mensaje = "Error de validación" });

            var response = await _clientesServicio.AgregarClienteAsync(clienteDto);
            return Json(response);
        }

        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var respuesta = await _clientesServicio.ObtenerClientePorIdAsync(id);
            return Json(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCliente(ClienteDto clienteDto)
        {
            if (!ModelState.IsValid)
                return Json(new CustomResponse<ClienteDto> { EsError = true, Mensaje = "Error de validación" });

            var respuesta = await _clientesServicio.ActualizarClienteAsync(clienteDto);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var respuesta = await _clientesServicio.EliminarClienteAsync(id);
            return Json(respuesta);
        }
    }
}