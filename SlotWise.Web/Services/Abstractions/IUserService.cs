using Microsoft.AspNetCore.Identity;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;

namespace SlotWise.Web.Services.Abstractions
{
    public interface IUserService
    {
        // Agregar un usuario
        public Task<Response<IdentityResult>> AddUserAsync(User user, string password);
        // Generar token de confirmación de email
        public Task<Response<string>> GenerateConfirmationTokenAsync(User user);
        // Confirmar usuario con el token
        public Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token);
        // Verificar si el usuario actual está autorizado para un permiso y módulo específicos
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        // Verificar si el usuario actual está autenticado
        public bool CurrentUserIsAuthenticaded();
        // Obtener un usuario por email
        public Task<User> GetUserByEmailasync(string email);
        // Crear un usuario
        Task<Response<UserDTO>> CreateAsync(UserDTO dto);
        // Editar usuario
        Task<Response<UserDTO>> EditAsync(UserDTO dto);
        // Eliminar usuario
        Task<Response<object>> DeleteAsync(Guid id);
        // Obtener un usuario por Id
        Task<Response<UserDTO>> GetOneAsync(Guid id);
        // Obtener todos los usuarios
        Task<Response<List<UserDTO>>> GetListAsync();
        public Task<Response<PaginationResponse<UserDTO>>> GetPaginatedListAsync(PaginationRequest request);
        public Task<Response<SignInResult>> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
    }
}
