using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;

namespace SlotWise.Web.Controllers
{
    public class ReservationController:Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly ISpecialistService _specialistService;
        private readonly IServiceService _serviceService;
        private readonly INotyfService _notyfService;

        public ReservationController(IReservationService  reservationService,IUserService userService,IServiceService serviceService, ISpecialistService specialistService, INotyfService notyfService)
        {
            _reservationService = reservationService;
            _userService = userService;
            _serviceService = serviceService;
            _specialistService = specialistService;
            _notyfService = notyfService;

        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            try
            {
                Response<PaginationResponse<ReservationDTO>> response = await _reservationService.GetPaginatedListAsync(request);

                if (!response.IsSuccess)
                {

                    ViewBag.ErrorMessage = response.Message;
                    return View(new PaginationResponse<ReservationDTO>());
                }


                return View(response.Result);
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción no manejada
                ViewBag.ErrorMessage = ex.Message;
                return View(new PaginationResponse<ReservationDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Obtenemos la lista de especialistas desde el servicio
            Response<List<SpecialistDTO>> specialistsResponse = await _specialistService.GetListAsync();
            Response<List<UserDTO>> usersResponse = await _userService.GetListAsync();
            Response<List<ServiceDTO>> servicesResponse = await _serviceService.GetListAsync();


            // Si todo va bien, llenamos el ViewBag para el <select>
            if (specialistsResponse.IsSuccess)
            {
                ViewBag.Specialists = new SelectList(
                    specialistsResponse.Result, // Lista de especialistas
                    "Id",                       // Valor que se guarda (SpecialistId)
                    "FirstName"                 // Lo que se muestra en pantalla (puedes usar "FullName" si existe)
                );
                ViewBag.Users = new SelectList(
                    usersResponse.Result, // Lista de especialistas
                    "Id",                       // Valor que se guarda (SpecialistId)
                    "FirstName"                 // Lo que se muestra en pantalla (puedes usar "FullName" si existe)
                );
                ViewBag.Services = new SelectList(
                    servicesResponse.Result, // Lista de especialistas
                    "Id",                       // Valor que se guarda (SpecialistId)
                    "NameService"                 // Lo que se muestra en pantalla (puedes usar "FullName" si existe)
                );
            }
            else
            {
                // Si algo falla, al menos enviamos una lista vacía para evitar errores
                ViewBag.Specialists = new SelectList(new List<SpecialistDTO>(), "Id", "FirstName");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ReservationDTO dto)
        {


            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<ReservationDTO> response = await _reservationService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<ReservationDTO> response = await _reservationService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }
            //  Obtener listas de selección para los <select>
            Response<List<UserDTO>> usersResponse = await _userService.GetListAsync();
            Response<List<SpecialistDTO>> specialistsResponse = await _specialistService.GetListAsync();
            Response<List<ServiceDTO>> servicesResponse = await _serviceService.GetListAsync();

            //  Llenar los ViewBag para las listas desplegables
            ViewBag.Users = usersResponse.Result?
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.FirstName} {u.LastName}"
                }).ToList();

            ViewBag.Specialists = specialistsResponse.Result?
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.FirstName} {s.LastName}"
                }).ToList();

            ViewBag.Services = servicesResponse.Result?
                .Select(sv => new SelectListItem
                {
                    Value = sv.Id.ToString(),
                    Text = sv.NameService
                }).ToList();
            return View(response.Result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ReservationDTO dto)
        {
            Response<List<UserDTO>> usersResponse = await _userService.GetListAsync();
            Response<List<SpecialistDTO>> specialistsResponse = await _specialistService.GetListAsync();
            Response<List<ServiceDTO>> servicesResponse = await _serviceService.GetListAsync();
            // 1️ Validar modelo
            if (!ModelState.IsValid)
            {
                _notyfService.Warning("Debe ajustar los errores de validación.");

                ViewBag.Users = usersResponse.Result?
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.FirstName} {u.LastName}"
                }).ToList();

                ViewBag.Specialists = specialistsResponse.Result?
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.FirstName} {s.LastName}"
                    }).ToList();

                ViewBag.Services = servicesResponse.Result?
                    .Select(sv => new SelectListItem
                    {
                        Value = sv.Id.ToString(),
                        Text = sv.NameService
                    }).ToList();

                return View(dto);
            }

            // 2️⃣ Llamar al servicio de edición
            Response<ReservationDTO> response = await _reservationService.EditAsync(dto);

            // 3️⃣ Validar resultado
            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);

                ViewBag.Users = usersResponse.Result?
               .Select(u => new SelectListItem
               {
                   Value = u.Id.ToString(),
                   Text = $"{u.FirstName} {u.LastName}"
               }).ToList();

                ViewBag.Specialists = specialistsResponse.Result?
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.FirstName} {s.LastName}"
                    }).ToList();

                ViewBag.Services = servicesResponse.Result?
                    .Select(sv => new SelectListItem
                    {
                        Value = sv.Id.ToString(),
                        Text = sv.NameService
                    }).ToList();

                return View(dto);

            }


            // 4️⃣ Si todo sale bien, redirigir al índice
            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _reservationService.DeleteAsync(id);

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
        public async Task<IActionResult> Toggle([FromForm] ReservationDTO dto)
        {
            Response<object> response = await _reservationService.ToggleAsync(dto);

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
