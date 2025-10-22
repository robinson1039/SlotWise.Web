using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Models;
using SlotWise.Web.Services.Abstractions;
using System.Diagnostics;

namespace SlotWise.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceService _serviceService;
        private readonly INotyfService _notyfService;

        public HomeController(IServiceService serviceService, INotyfService notyfService)
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
