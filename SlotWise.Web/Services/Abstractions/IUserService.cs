using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;

namespace SlotWise.Web.Services.Abstractions
{
    public interface IUserService
    {
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
    }
}
