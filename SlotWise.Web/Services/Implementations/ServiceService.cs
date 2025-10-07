using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;
using AutoMapper;


namespace SlotWise.Web.Services.Implementations
{
    public class ServiceService : CustomQueryableOperationsService, IServiceService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        //private object dto;

        public ServiceService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // Crear un servicio
        public async Task<Response<ServiceDTO>> CreateAsync(ServiceDTO dto)
        {
            try
            {
                Service service = new Service
                {
                    Id = Guid.NewGuid(),
                    NameService = dto.NameService,
                    Price = dto.Price,
                    Description = dto.Description,
                    Status = dto.Status,
                    SpecialistId = dto.SpecialistId
                };
                await _context.Services.AddAsync(service);
                await _context.SaveChangesAsync();


                dto.Id = service.Id;
                return Response<ServiceDTO>.Success(dto, "Servicio creado con éxito.");
            }
            catch (Exception ex)
            {
                return new Response<ServiceDTO>($"Error al crear el servicio: {ex.Message}");
            }
        }

        // 🔹 Borar servicio (borrar fisicamente)
        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            try
            {
                Service? service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);

                if (service == null)
                {
                    return Response<object>.Failure($"no existe servicio con id: {id}");
                }
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                return Response<object>.Success("Servicio eliminado con exito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }
        // Edit 
        public async Task<Response<ServiceDTO>> EditAsync(ServiceDTO dto)
        {
            try
            {
                Service? service = await _context.Services.FirstOrDefaultAsync(s => s.Id == dto.Id);
                if (service is null)
                {
                    return Response<ServiceDTO>.Failure($"No existe servicio con id:{dto.Id}");
                }

                _mapper.Map(dto, service); // actualiza propiedades sin crear un nuevo objeto
               // await _context.SaveChangesAsync();


               // service = _mapper.Map<Service>(dto);
                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return Response<ServiceDTO>.Success(dto, "Servicio actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<ServiceDTO>.Failure(ex);
            }
        }

        public async Task<Response<List<ServiceDTO>>> GetListAsync()
        {
            try
            {
                List<Service> services = await _context.Services.Include(s => s.Specialist).ToListAsync();

                List<ServiceDTO> list = _mapper.Map<List<ServiceDTO>>(services);

                return Response<List<ServiceDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<ServiceDTO>>.Failure(ex);
            }
        }

        public async Task<Response<ServiceDTO>> GetOneAsync(Guid id)
        {
            try
            {
                Service? service = await _context.Services.Include(s => s.Specialist).FirstOrDefaultAsync(s => s.Id == id);

                if (service is null)
                {
                    return Response<ServiceDTO>.Failure($"No existe servicio con id: {id}");
                }

                ServiceDTO dto = _mapper.Map<ServiceDTO>(service);

                return Response<ServiceDTO>.Success(dto, "Servicio obtenido con éxito");
            }
            catch (Exception ex)
            {
                return Response<ServiceDTO>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<ServiceDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Service> query = _context.Services.Include(s => s.Specialist).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.NameService.ToLower().Contains(request.Filter.ToLower()));
            }
            query = query.OrderBy(s => s.NameService);//mostrar por oden de nombre de servicio
            //se puede cambiar para ordenar por id u otro atributo. 

            return await GetPaginationAsync<Service, ServiceDTO>(request, query);
        }

        public async Task<Response<object>> ToggleAsync(ToggleServiceStatusDTO dto)
        {
            try
            {
                Service? service = await _context.Services.FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (service is null)
                {
                    return Response<object>.Failure($"No existe servicio con id: {dto.Id}");
                }

                service.Status = !service.Status;
                _context.Services.Update(service);
                await _context.SaveChangesAsync();

                return Response<object>.Success(null,"Estado de servicio actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }
    }
}