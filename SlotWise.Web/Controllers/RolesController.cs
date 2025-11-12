using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;
using SlotWise.Web.Services.Implementations;

namespace SlotWise.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notyfService;

        public RolesController(IRolesService rolesService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _rolesService.GetAllAsync();
            return View(response.Result);
        }




        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var permissionsResponse = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse.IsSuccess)
            {
                _notyfService.Error(permissionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            var dto = new RolesDTO
            {
                Permissions = permissionsResponse.Result
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RolesDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                var permissionsResponse = await _rolesService.GetPermissionsAsync();
                dto.Permissions = permissionsResponse.Result;
                return View(dto);
            }

            var createResponse = await _rolesService.CreateAsync(dto);
            if (createResponse.IsSuccess)
            {
                _notyfService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(createResponse.Message);
            var permissionsResponse2 = await _rolesService.GetPermissionsAsync();
            dto.Permissions = permissionsResponse2.Result;
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _rolesService.GetOneAsync(id);
            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RolesDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                var permissionsResponse = await _rolesService.GetPermissionsAsync();
                dto.Permissions = permissionsResponse.Result;
                return View(dto);
            }

            var updateResponse = await _rolesService.EditAsync(dto);
            if (updateResponse.IsSuccess)
            {
                _notyfService.Success(updateResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(updateResponse.Message);
            var permissionsResponse2 = await _rolesService.GetPermissionsAsync();
            dto.Permissions = permissionsResponse2.Result;
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _rolesService.DeleteAsync(id);

            if (!response.IsSuccess)
                _notyfService.Error(response.Message);
            else
                _notyfService.Success(response.Message);

            return RedirectToAction(nameof(Index));
        }
    }
}
