using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;
using SlotWise.Web.Services.Abstractions;
using System.Data;

namespace SlotWise.Web.Services.Implementations
{
    public class RolesService : CustomQueryableOperationsService, IRolesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesService(DataContext context, IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<RolesDTO>> CreateAsync(RolesDTO dto)
        {
            using (IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                     PrivateRole role = _mapper.Map<PrivateRole>(dto);

                    await _context.PrivateRoles.AddAsync(role);
                    await _context.SaveChangesAsync();

                    // Permisos
                    List<Guid> permissionIds = new();

                    if (!string.IsNullOrEmpty(dto.PermissionIds))
                    {
                        permissionIds = JsonConvert.DeserializeObject<List<Guid>>(dto.PermissionIds);
                    }

                    foreach (Guid permissionId in permissionIds)
                    {
                        RolePermission rolePermission = new RolePermission
                        {
                            PrivateRoleId = role.Id,
                            PermissionId = permissionId
                        };

                        await _context.RolePermissions.AddAsync(rolePermission);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Response<RolesDTO>.Success(dto, "Rol creado con éxito");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Response<RolesDTO>.Failure(ex);
                }
            }
        }

        public async Task<Response<RolesDTO>> EditAsync(RolesDTO dto)
        {
            try
            {
                if (dto.Name == Env.SUPER_ADMIN_ROLE_NAME)
                {
                    return Response<RolesDTO>.Failure($"El rol '{Env.SUPER_ADMIN_ROLE_NAME}' no puede ser editado");
                }

                PrivateRole role = _mapper.Map<PrivateRole>(dto);
                _context.PrivateRoles.Update(role);

                // Borrar permisos anteriores
                var oldPermissions = await _context.RolePermissions
                    .Where(rp => rp.PrivateRoleId == dto.Id)
                    .ToListAsync();

                _context.RolePermissions.RemoveRange(oldPermissions);

                // Agregar nuevos
                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    List<int>? permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);

                    foreach (int pid in permissionIds)
                    {
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            PrivateRoleId = role.Id,
                            PermissionId = role.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return Response<RolesDTO>.Success(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return Response<RolesDTO>.Failure(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            if (await _context.Users.AnyAsync(u => u.PrivateRoleId == id))
            {
                return Response<object>.Failure("No se puede eliminar el rol porque hay usuarios asociados.");
            }

            var role = await _context.PrivateRoles.FindAsync(id);
            if (role == null)
                return Response<object>.Failure("Rol no encontrado.");

            _context.PrivateRoles.Remove(role);
            await _context.SaveChangesAsync();

            return Response<object>.Success("Rol eliminado con éxito");
        }

        public async Task<Response<RolesDTO>> GetOneAsync(Guid id)
        {
            PrivateRole? role = await _context.PrivateRoles.FindAsync(id);

            if (role == null)
                return Response<RolesDTO>.Failure("Rol no encontrado.");

            RolesDTO dto = _mapper.Map<RolesDTO>(role);


            dto.Permissions = await _context.Permissions
                .Select(p => new PermissionForRolesDTO
                {
                    Id = p.Id,
                    Description = p.Description,
                    Module = p.Module,
                    Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.PrivateRoleId == role.Id)
                })
                .ToListAsync();

            return Response<RolesDTO>.Success(dto, "Rol obtenido con éxito");
        }

        public async Task<Response<PaginationResponse<RolesDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<PrivateRole> query = _context.PrivateRoles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                // CORRECCIÓN: Elimina la duplicación en la condición
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<PrivateRole, RolesDTO>(request, query);
        }

        //        public async Task<Response<PaginationResponse<RolesDTO>>> GetPaginatedListAsync(PaginationRequest request)
        //{
        //        try
        //        {
        //            IQueryable<PrivateRole> query = _context.PrivateRoles.AsQueryable();

        //        if (!string.IsNullOrWhiteSpace(request.Filter))
        //        {
        //            query = query.Where(r => r.Name.ToLower().Contains(request.Filter.ToLower()));
        //        }

        //                Response<PaginationResponse<RolesDTO>> result = await GetPaginationAsync<PrivateRole, RolesDTO>(request, query);

        //        // 🔹 Evitar nulos
        //        if (result == null)
        //        {
        //                    result = new PaginationResponse<RolesDTO>
        //                    {
        //                        List = new PagedList<RolesDTO>(new List<RolesDTO>(), 0, request.RecordsPerPage, 0),
        //                        CurrentPage = 1,
        //                        TotalPages = 1,
        //                        RecordsPerPage = request.RecordsPerPage,
        //                        TotalCount = 0
        //                    };

        //                }

        //                // 🔹 Evitar nulo en la lista
        //                result.List ??= new PagedList<RolesDTO>(new List<RolesDTO>(), 0, request.RecordsPerPage, 0);


        //                return Response<PaginationResponse<RolesDTO>>.Success(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Response<PaginationResponse<RolesDTO>>.Failure(ex, $"Error en GetPaginatedListAsync: {ex.Message}");
        //    }
        //}



        public async Task<Response<List<PermissionForRolesDTO>>> GetPermissionsAsync()
        {
            var permissions = await _context.Permissions
                .Select(p => new PermissionForRolesDTO
                {
                    Id = p.Id,
                    Description = p.Description,
                    Module = p.Module,
                    Selected = false
                })
                .ToListAsync();

            return Response<List<PermissionForRolesDTO>>.Success(permissions);
        }
    }
}
