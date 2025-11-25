using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;
using SlotWise.Web.Helpers.Abstractions;
using SlotWise.Web.Services.Abstractions;

namespace SlotWise.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IRolesHelper _rolesHelper;
        public UserController(IUserService userService, INotyfService notyfService, IMapper mapper, UserManager<User> userManager, ILogger<UserController> logger,
            DataContext context, IRolesHelper rolesHelper )
        {
            _userService = userService;
            _notyfService = notyfService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _rolesHelper= rolesHelper;
        }

        [Authorize(Policy = "createServices")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await _rolesHelper.GetComboRolesAsync();
            return View();
        }
        
        [Authorize(Policy = "createServices")]
        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _rolesHelper.GetComboRolesAsync();
                return View(dto);
            }

            Response<UserDTO> response = await _userService.CreateAsync(dto);
            if (!response.IsSuccess)
            {
                ViewBag.Roles = await _rolesHelper.GetComboRolesAsync();
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ViewMyPermissions()
        {
            User? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                _logger.LogWarning("Usuario no encontrado");
                _notyfService.Error("Usuario no encontrado");
                return RedirectToAction("Login");
            }

            // Obtener información completa de permisos
            object permissionsInfo = await GetUserPermissionsAsync(user.Id);

            // Mostrar en consola
            LogPermissionsToConsole(permissionsInfo);

            // También pasar a la vista
            return View(permissionsInfo);
        }

        private async Task<object> GetUserPermissionsAsync(Guid userId)
        {
            User? user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("Usuario no encontrado");

            // 1. Roles de Identity
            IList<string> identityRoles = await _userManager.GetRolesAsync(user);

            // 2. PrivateRole y Permisos personalizados
            var privateRoleWithPermissions = await _context.Users
                .Include(u => u.PrivateRole)
                    .ThenInclude(pr => pr.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .Where(u => u.Id == userId)
                .Select(u => new
                {
                    PrivateRole = u.PrivateRole,
                    Permissions = u.PrivateRole != null ?
                        u.PrivateRole.RolePermissions.Select(rp => rp.Permission).ToList() :
                        new List<Permission>()
                })
                .FirstOrDefaultAsync();

            // 3. Claims de Identity
            IList<System.Security.Claims.Claim> claims = await _userManager.GetClaimsAsync(user);

            return new
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                IdentityRoles = identityRoles,
                PrivateRole = privateRoleWithPermissions?.PrivateRole,
                CustomPermissions = privateRoleWithPermissions?.Permissions ?? new List<Permission>(),
                IdentityClaims = claims
            };
        }

        private void LogPermissionsToConsole(object permissionsInfo)
        {
            try
            {
                dynamic info = permissionsInfo;

                Console.WriteLine("\n" + new string('=', 50));
                Console.WriteLine("🎯 PERMISOS DEL USUARIO ACTUAL");
                Console.WriteLine(new string('=', 50));

                Console.WriteLine($"👤 USUARIO: {info.FullName}");
                Console.WriteLine($"📧 Email: {info.Email}");
                Console.WriteLine($"🆔 ID: {info.UserId}");
                Console.WriteLine();

                // Roles de Identity
                Console.WriteLine("🔐 ROLES DE IDENTITY:");
                if (info.IdentityRoles.Count > 0)
                {
                    foreach (string role in info.IdentityRoles)
                    {
                        Console.WriteLine($"   ✅ {role}");
                    }
                }
                else
                {
                    Console.WriteLine("   ❌ No tiene roles de Identity");
                }

                Console.WriteLine();

                // PrivateRole - SOLO Name (NO tiene Description)
                Console.WriteLine("🎯 ROL PERSONALIZADO:");
                if (info.PrivateRole != null)
                {
                    Console.WriteLine($"   📝 {info.PrivateRole.Name}");
                }
                else
                {
                    Console.WriteLine("   ❌ No tiene rol personalizado asignado");
                }

                Console.WriteLine();

                // Permisos personalizados - Permission SÍ tiene Description y Module
                Console.WriteLine("📋 PERMISOS PERSONALIZADOS:");
                if (info.CustomPermissions.Count > 0)
                {
                    foreach (var permission in info.CustomPermissions)
                    {
                        Console.WriteLine($"   ✅ {permission.Name}");
                        Console.WriteLine($"      📖 {permission.Description}");
                        Console.WriteLine($"      🗂️  Módulo: {permission.Module}");
                    }
                    Console.WriteLine($"   📊 Total: {info.CustomPermissions.Count} permisos");
                }
                else
                {
                    Console.WriteLine("   ❌ No tiene permisos personalizados");
                }

                Console.WriteLine();

                // Claims
                Console.WriteLine("🏷️ CLAIMS DE IDENTITY:");
                if (info.IdentityClaims.Count > 0)
                {
                    foreach (var claim in info.IdentityClaims)
                    {
                        Console.WriteLine($"   🔹 {claim.Type} = {claim.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("   ❌ No tiene claims");
                }

                Console.WriteLine(new string('=', 50));
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al mostrar permisos: {ex.Message}");
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult>Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Response<Microsoft.AspNetCore.Identity.SignInResult> result = await _userService.LoginAsync(dto);
                if (result.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            }
            return View(dto);
        }

        // Logout
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = await _userService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                ViewBag.Message = "Error al registrar usuario: " + response.Message;
                return View(dto);
            }

            ViewBag.Message = "Usuario registrado con éxito";
            return RedirectToAction("Login", "User"); // o a otra vista si deseas
        }

        [Authorize(Policy = "showUsers")]
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

        [Authorize(Policy = "updateUsers")]
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
        
        [Authorize(Policy = "updateUsers")]
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
