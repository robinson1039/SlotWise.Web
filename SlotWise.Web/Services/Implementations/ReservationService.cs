using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;

namespace SlotWise.Web.Services.Implementations
{
    public class ReservationService : CustomQueryableOperationsService, IReservationService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        //private object dto;
        public ReservationService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response<ReservationDTO>> CreateAsync(ReservationDTO dto)
        {
            try
            {
                Reservation reservation = new Reservation
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.UserId,
                    SpecialistId = dto.SpecialistId,
                    ServiceId = dto.ServiceId,
                    Status = dto.Status,
                    CreateAt = DateTime.UtcNow

                };
                await _context.Reservations.AddAsync(reservation);
                await _context.SaveChangesAsync();


                dto.Id = reservation.Id;
                return Response<ReservationDTO>.Success(dto, "Servicio creado con éxito.");
            }
            catch (Exception ex)
            {
                return new Response<ReservationDTO>($"Error al crear el servicio: {ex.Message}");
            }
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            try
            {
                Reservation? reservation = await _context.Reservations.FirstOrDefaultAsync(s => s.Id == id);

                if (reservation == null)
                {
                    return Response<object>.Failure($"no existe servicio con id: {id}");
                }
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                return Response<object>.Success("Servicio eliminado con exito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }

        public async Task<Response<ReservationDTO>> EditAsync(ReservationDTO dto)
        {
            try
            {
                Reservation? reservation = await _context.Reservations.FirstOrDefaultAsync(s => s.Id == dto.Id);
                if (reservation is null)
                {
                    return Response<ReservationDTO>.Failure($"No existe servicio con id:{dto.Id}");
                }
                dto.CreateAt = reservation.CreateAt;

                _mapper.Map(dto, reservation); // actualiza propiedades sin crear un nuevo objeto
                                           // await _context.SaveChangesAsync();


                // service = _mapper.Map<Service>(dto);
                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();

                return Response<ReservationDTO>.Success(dto, "reserva actualizada con éxito");
            }
            catch (Exception ex)
            {
                return Response<ReservationDTO>.Failure(ex);
            }
        }

        public async Task<Response<List<ReservationDTO>>> GetListAsync()
        {
            try
            {
                List<Reservation> reservations = await _context.Reservations.Include(s => s.Specialist)
                    .Include(s => s.User)
                    .Include(s => s.Service)
                    .ToListAsync();

                List<ReservationDTO> list = _mapper.Map<List<ReservationDTO>>(reservations);

                return Response<List<ReservationDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<ReservationDTO>>.Failure(ex);
            }
        }

        public async Task<Response<ReservationDTO>> GetOneAsync(Guid id)
        {
            try
            {
                Reservation? reservation = await _context.Reservations
                    .Include(r => r.User)        // Trae los datos del usuario asociado
                    .Include(r => r.Specialist)  // Trae los datos del especialista
                    .Include(r => r.Service)     // Trae los datos del servicio
                    .FirstOrDefaultAsync(r => r.Id == id); // Filtra por ID

                if (reservation is null)
                {
                    return Response<ReservationDTO>.Failure($"No existe servicio con id: {id}");
                }

                ReservationDTO dto = _mapper.Map<ReservationDTO>(reservation);

                return Response<ReservationDTO>.Success(dto, "Servicio obtenido con éxito");
            }
            catch (Exception ex)
            {
                return Response<ReservationDTO>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<ReservationDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Reservation> query = _context.Reservations.Include(s => s.Specialist)
                    .Include(s => s.User)
                    .Include(s => s.Service).AsQueryable();

            return await GetPaginationAsync<Reservation, ReservationDTO>(request, query);
        }

        public async Task<Response<object>> ToggleAsync(ReservationDTO dto)
        {
            try
            {
                Reservation? reservation = await _context.Reservations.FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (reservation is null)
                {
                    return Response<object>.Failure($"No existe servicio con id: {dto.Id}");
                }

                reservation.Status = !reservation.Status;
                _context.Reservations.Update(reservation);
                await _context.SaveChangesAsync();

                return Response<object>.Success(null, "Estado de servicio actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }
    }
}
