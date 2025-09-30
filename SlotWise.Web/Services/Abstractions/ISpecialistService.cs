using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;

namespace SlotWise.Web.Services.Abstractions
{
    public interface ISpecialistService
    {
        // Crear un especialista
        Task<Response<SpecialistDTO>> CreateAsync(SpecialistDTO dto);

        // Editar especialista
        Task<Response<SpecialistDTO>> EditAsync(SpecialistDTO dto);

        // Eliminar especialista
        Task<Response<object>> DeleteAsync(Guid id);

        // Obtener un especialista por Id
        Task<Response<SpecialistDTO>> GetOneAsync(Guid id);

        // Obtener todos los especialistas
        Task<Response<List<SpecialistDTO>>> GetListAsync();

        public Task<Response<PaginationResponse<SpecialistDTO>>> GetPaginatedListAsync(PaginationRequest request);


        // Alternar estado (activar/desactivar)
        Task<Response<object>> ToggleAsync(ToggleSpecialistStatusDTO dto);
    }
}
