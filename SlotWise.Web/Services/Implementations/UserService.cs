using AutoMapper;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;

namespace SlotWise.Web.Services.Implementations
{
    public class UserService : CustomQueryableOperationsService, IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<UserDTO>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = dto.UserName,
                    PasswordHash = dto.PasswordHash,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    CC = dto.CC,
                    Age = dto.Age,
                    Birthdate = dto.Birthdate,
                    CreateAt = DateTime.UtcNow
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                dto.Id = user.Id;
                dto.CreateAt = user.CreateAt;
                return Response<UserDTO>.Success(dto, "Usuario creado con éxito");
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }
        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);

                if (user is null)
                {
                    return Response<object>.Failure($"No existe sección con id: {id}");
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Response<object>.Success("Sección eliminada con éxito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }

        public async Task<Response<UserDTO>> EditAsync(UserDTO dto)
        {
            try
            {
                User? user = await _context.Users.AsNoTracking()
                                                          .FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (user is null)
                {
                    return Response<UserDTO>.Failure($"No existe sección con id: {dto.Id}");
                }
                dto.CreateAt = user.CreateAt;
                user = _mapper.Map<User>(dto);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return Response<UserDTO>.Success(dto, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public async Task<Response<List<UserDTO>>> GetListAsync()
        {
            try
            {
                // DEBUG: Verificar si hay datos
                var count = await _context.Users.CountAsync();
                Console.WriteLine($"Total usuarios in DB: {count}");

                List<User> users = await _context.Users.ToListAsync();
                Console.WriteLine($"Users retrieved: {users.Count}");

                // DEBUG: Verificar el primer especialista
                if (users.Any())
                {
                    var first = users.First();
                    Console.WriteLine($"First user - Name: {first.FirstName}, Create_at: {first.CreateAt}, Age: {first.Age}");
                }

                List<UserDTO> list = _mapper.Map<List<UserDTO>>(users);

                // DEBUG: Verificar mapeo
                if (list.Any())
                {
                    var firstDto = list.First();
                    Console.WriteLine($"First DTO - Name: {firstDto.FirstName}, CreateAt: {firstDto.CreateAt}, Age: {firstDto.Age}");
                }

                return Response<List<UserDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                // DEBUG: Mostrar el error real
                Console.WriteLine($"ERROR in GetListAsync: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return Response<List<UserDTO>>.Failure(ex);
            }
        }

        public async Task<Response<UserDTO>> GetOneAsync(Guid id)
        {
            try
            {
                User? section = await _context.Users.FirstOrDefaultAsync(s => s.Id == id);

                if (section is null)
                {
                    return Response<UserDTO>.Failure($"No existe sección con id: {id}");
                }

                UserDTO dto = _mapper.Map<UserDTO>(section);

                return Response<UserDTO>.Success(dto, "Sección obtenida con éxito");
            }
            catch (Exception ex)
            {
                return Response<UserDTO>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<UserDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<User> query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                // CORRECCIÓN: Elimina la duplicación en la condición
                query = query.Where(s => s.FirstName.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<User, UserDTO>(request, query);
        }
    }
}
