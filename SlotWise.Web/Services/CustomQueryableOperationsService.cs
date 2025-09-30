using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SlotWise.Web.Core;
using SlotWise.Web.Core.Pagination;
using SlotWise.Web.Data;
using SlotWise.Web.Data.Abstraction;
using System.Security.Cryptography;

namespace SlotWise.Web.Services
{
    public class CustomQueryableOperationsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomQueryableOperationsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PaginationResponse<TDTO>>> GetPaginationAsync<TEntity, TDTO>(PaginationRequest request, IQueryable<TEntity> query = null)
       where TEntity : class
       where TDTO : class
        {
            try
            {
                if (query is null)
                {
                    query = _context.Set<TEntity>();
                }

                PagedList<TEntity> list = await PagedList<TEntity>.ToPagedListAsync(query, request);

                PaginationResponse<TDTO> response = new PaginationResponse<TDTO>
                {
                    List = _mapper.Map<PagedList<TDTO>>(list),
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return Response<PaginationResponse<TDTO>>.Success(response);
            }
            catch (Exception ex)
            {
                return Response<PaginationResponse<TDTO>>.Failure(ex);
            }
        }

        public async Task<Response<List<TDTO>>> GetCompleteListAsync<TEntity, TDTO>(IQueryable<TEntity> query = null)
       where TEntity : class, IId
        {
            try
            {
                if (query is null)
                {
                    query = _context.Set<TEntity>();
                }

                List<TEntity> list = await query.ToListAsync();
                List<TDTO> dtoList = _mapper.Map<List<TDTO>>(list);

                return Response<List<TDTO>>.Success(dtoList);
            }
            catch (Exception ex)
            {
                return Response<List<TDTO>>.Failure(ex);
            }
        }
    }
}
