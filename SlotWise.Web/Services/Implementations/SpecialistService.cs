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
    public class SpecialistService : CustomQueryableOperationsService, ISpecialistService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public SpecialistService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response<SpecialistDTO>> CreateAsync(SpecialistDTO dto)
        {
            try
            {
                Specialist specialist = new Specialist
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Age = dto.Age ?? 0,  // Convierte null a 0 o usa un valor por defecto
                    Status = dto.Status,
                    Create_at = DateTime.UtcNow
                };

                await _context.Specialist.AddAsync(specialist);
                await _context.SaveChangesAsync();

                dto.Id = specialist.Id;
                dto.CreateAt = specialist.Create_at;

                return Response<SpecialistDTO>.Success(dto, "Especialista creado con éxito");
            }
            catch (Exception ex)
            {
                return Response<SpecialistDTO>.Failure(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            try
            {
                Specialist? specialist = await _context.Specialist.FirstOrDefaultAsync(s => s.Id == id);

                if (specialist is null)
                {
                    return Response<object>.Failure($"No existe sección con id: {id}");
                }

                _context.Specialist.Remove(specialist);
               await _context.SaveChangesAsync();

                return Response<object>.Success("Sección eliminada con éxito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }

        public async Task<Response<SpecialistDTO>> EditAsync(SpecialistDTO dto)
        {
            try
            {
                Specialist? specialist = await _context.Specialist.AsNoTracking()
                                                          .FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (specialist is null)
                {
                    return Response<SpecialistDTO>.Failure($"No existe sección con id: {dto.Id}");
                }

                specialist = _mapper.Map<Specialist>(dto);
                _context.Specialist.Update(specialist);
                await _context.SaveChangesAsync();

                return Response<SpecialistDTO>.Success(dto, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return Response<SpecialistDTO>.Failure(ex);
            }
        }

        public async Task<Response<List<SpecialistDTO>>> GetListAsync()
        {
            try
            {
                // DEBUG: Verificar si hay datos
                var count = await _context.Specialist.CountAsync();
                Console.WriteLine($"Total specialists in DB: {count}");

                List<Specialist> specialists = await _context.Specialist.ToListAsync();
                Console.WriteLine($"Specialists retrieved: {specialists.Count}");

                // DEBUG: Verificar el primer especialista
                if (specialists.Any())
                {
                    var first = specialists.First();
                    Console.WriteLine($"First specialist - Name: {first.Name}, Create_at: {first.Create_at}, Age: {first.Age}");
                }

                List<SpecialistDTO> list = _mapper.Map<List<SpecialistDTO>>(specialists);

                // DEBUG: Verificar mapeo
                if (list.Any())
                {
                    var firstDto = list.First();
                    Console.WriteLine($"First DTO - Name: {firstDto.Name}, CreateAt: {firstDto.CreateAt}, Age: {firstDto.Age}");
                }

                return Response<List<SpecialistDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                // DEBUG: Mostrar el error real
                Console.WriteLine($"ERROR in GetListAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Response<List<SpecialistDTO>>.Failure(ex);
            }
        }
        
        public async Task<Response<SpecialistDTO>> GetOneAsync(Guid id)
        {
            try
            {
                Specialist? section = await _context.Specialist.FirstOrDefaultAsync(s => s.Id == id);

                if (section is null)
                {
                    return Response<SpecialistDTO>.Failure($"No existe sección con id: {id}");
                }

                SpecialistDTO dto = _mapper.Map<SpecialistDTO>(section);

                return Response<SpecialistDTO>.Success(dto, "Sección obtenida con éxito");
            }
            catch (Exception ex)
            {
                return Response<SpecialistDTO>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<SpecialistDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Specialist> query = _context.Specialist.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                // CORRECCIÓN: Elimina la duplicación en la condición
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Specialist, SpecialistDTO>(request, query);
        }
        public async Task<Response<object>> ToggleAsync(ToggleSpecialistStatusDTO dto)
        {
            try
            {
                Specialist? specialist = await _context.Specialist
                                              .FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (specialist is null)
                {
                    return Response<object>.Failure($"No existe especialista con id: {dto.Id}");
                }

                // CAMBIO IMPORTANTE: Hacer toggle del estado
                specialist.Status = !specialist.Status;  // ← Esto cambia true→false o false→true
        
                // Quita el AsNoTracking() porque necesitas que EF rastreé los cambios
                await _context.SaveChangesAsync();

                return Response<object>.Success(null, "Estado actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }
    }
}
