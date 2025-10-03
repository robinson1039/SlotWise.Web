using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

namespace SlotWise.Web.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly INotyfService _notyfService;

        public ServiceController(IServiceService serviceService, INotyfService notyfService)
        {
            _serviceService = serviceService;
            _notyfService = notyfService;

        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            try
            {
                Response<PaginationResponse<ServiceDTO>> response = await _serviceService.GetPaginatedListAsync(request);

                if (!response.IsSuccess)
                {
                    
                    ViewBag.ErrorMessage = response.Message;
                    return View(new PaginationResponse<ServiceDTO>());
                }

                return View(response.Result);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción no manejada
                ViewBag.ErrorMessage = ex.Message;
                return View(new PaginationResponse<ServiceDTO>());
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<ServiceDTO> response = await _serviceService.CreateAsync(dto);

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
            Response<ServiceDTO> response = await _serviceService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ServiceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<ServiceDTO> response = await _serviceService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _serviceService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Toggle([FromForm] ToggleServiceStatusDTO dto)
        {
            Response<object> response = await _serviceService.ToggleAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
