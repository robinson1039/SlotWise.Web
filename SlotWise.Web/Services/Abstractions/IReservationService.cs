using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;

namespace SlotWise.Web.Services.Abstractions
{
    public interface IReservationService
    {
        // Crear una reserva
        Task<Response<ReservationDTO>> CreateAsync(ReservationDTO dto);
        // Editar reserva
        Task<Response<ReservationDTO>> EditAsync(ReservationDTO dto);
        // Eliminar reserva
        Task<Response<object>> DeleteAsync(Guid id);
        // Obtener una reserva por Id
        Task<Response<ReservationDTO>> GetOneAsync(Guid id);
        // Obtener todas las reservas
        Task<Response<List<ReservationDTO>>> GetListAsync();
        public Task<Response<PaginationResponse<ReservationDTO>>> GetPaginatedListAsync(PaginationRequest request);
        Task<Response<object>> ToggleAsync(ReservationDTO dto);
    }
}
