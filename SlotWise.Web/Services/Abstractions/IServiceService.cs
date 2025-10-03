using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;


namespace SlotWise.Web.Services.Abstractions
{
    public interface IServiceService
    {
        //Crear el servicio.
        Task<Response<ServiceDTO>>CreateAsync(ServiceDTO dto);
        //Editar
        Task<Response<ServiceDTO>>EditAsync(ServiceDTO dto);
        //Borrar físicamente
        Task<Response<object>>DeleteAsync(Guid id);
        //traer todos los servicios
        Task<Response<List<ServiceDTO>>>GetListAsync();
        //traer por Id
        Task<Response<ServiceDTO>>GetOneAsync(Guid id);
        Task<Response<PaginationResponse<ServiceDTO>>> GetPaginatedListAsync(PaginationRequest request);
        //Estado del servicio:
        Task<Response<object>> ToggleAsync(ToggleServiceStatusDTO dto);
    }
}
