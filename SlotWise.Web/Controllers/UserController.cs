using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;

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
                var response = await _userService.GetPaginatedListAsync(request);
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

    }
}
