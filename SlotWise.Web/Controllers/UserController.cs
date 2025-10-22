using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

namespace SlotWise.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotyfService _notyfService;
        public UserController(IUserService userService, INotyfService notyfService)
        {
            _userService = userService;
            _notyfService = notyfService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            try
            {
                Response<PaginationResponse<UserDTO>> response = await _userService.GetPaginatedListAsync(request);
                if (!response.IsSuccess)
                {
                    // Mostrar el mensaje de error en la vista
                    ViewBag.ErrorMessage = response.Message;
                    return View(new PaginationResponse<UserDTO>());
                }
                return View(response.Result);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new PaginationResponse<UserDTO>());
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }
            Response<UserDTO> response = await _userService.CreateAsync(dto);
            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }
            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<UserDTO> response = await _userService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Obtener los nombres de los campos con error y su mensaje
                var errores = ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(ms => new
                    {
                        Campo = ms.Key,
                        Mensajes = ms.Value.Errors.Select(e => e.ErrorMessage)
                    })
                    .ToList();

                // Crear un mensaje descriptivo
                string detalleErrores = string.Join("; ", errores.Select(e =>
                    $"{e.Campo}: {string.Join(", ", e.Mensajes)}"));

                _notyfService.Error($"Debe ajustar los errores de validación → {detalleErrores}");

                return View(dto);
            }

            Response<UserDTO> response = await _userService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
