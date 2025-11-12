using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;
using SlotWise.Web.Core;
namespace SlotWise.Web.Services.Abstractions
{
    public interface IRolesService
    {
        Task<Response<PaginationResponse<RolesDTO>>> GetPaginatedListAsync(PaginationRequest request);
        Task<Response<List<PermissionForRolesDTO>>> GetPermissionsAsync();
        Task<Response<RolesDTO>> CreateAsync(RolesDTO dto);
        Task<Response<RolesDTO>> EditAsync(RolesDTO dto);
        Task<Response<object>> DeleteAsync(Guid id);
        Task<Response<RolesDTO>> GetOneAsync(Guid id);
    }
}

