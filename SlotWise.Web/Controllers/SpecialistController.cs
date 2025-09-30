using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

namespace SlotWise.Web.Controllers
{
    public class SpecialistController : Controller
    {
        private readonly ISpecialistService _specialistService;
        private readonly INotyfService _notyfService;

        public SpecialistController(ISpecialistService specialistService, INotyfService notyfService)
        {
            _specialistService = specialistService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            try
            {
                var response = await _specialistService.GetPaginatedListAsync(request);

                if (!response.IsSuccess)
                {
                    // Mostrar el mensaje de error en la vista
                    ViewBag.ErrorMessage = response.Message;
                    return View(new PaginationResponse<SpecialistDTO>());
                }

                return View(response.Result);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción no manejada
                ViewBag.ErrorMessage = ex.Message;
                return View(new PaginationResponse<SpecialistDTO>());
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SpecialistDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<SpecialistDTO> response = await _specialistService.CreateAsync(dto);

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
            Response<SpecialistDTO> response = await _specialistService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] SpecialistDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<SpecialistDTO> response = await _specialistService.EditAsync(dto);

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
            Response<object> response = await _specialistService.DeleteAsync(id);

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
        public async Task<IActionResult> Toggle([FromForm] ToggleSpecialistStatusDTO dto)
        {
            Response<object> response = await _specialistService.ToggleAsync(dto);

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
        /*
         * Este fragmento sirve para hacer Debug a lo que trae GetPaginatedListAsync se debe ir al edponid /Specialist/TestData para visualizar el error 
         * 
        public async Task<IActionResult> TestData()
        {
            try
            {
                // Probar GetListAsync
                var serviceResult = await _specialistService.GetListAsync();
                ViewBag.ServiceSuccess = serviceResult.IsSuccess;
                ViewBag.ServiceMessage = serviceResult.Message;
                ViewBag.ServiceData = serviceResult.Result;
                ViewBag.ServiceDataCount = serviceResult.Result?.Count ?? 0;

                // Probar paginación
                var paginationResult = await _specialistService.GetPaginatedListAsync(new PaginationRequest());
                ViewBag.PaginationSuccess = paginationResult.IsSuccess;
                ViewBag.PaginationMessage = paginationResult.Message;
                ViewBag.PaginationData = paginationResult.Result;
                ViewBag.PaginationDataCount = paginationResult.Result?.List?.Count ?? 0;

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }*/
    }
}
