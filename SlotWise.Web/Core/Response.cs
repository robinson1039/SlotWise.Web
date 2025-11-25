using SlotWise.Web.Core.Pagination;
using SlotWise.Web.DTOs;

namespace SlotWise.Web.Core
{
    public class Response<T>  
    {
        private string v=string.Empty;

        public Response(Response<PaginationResponse<RolesDTO>> pagination)
        {
        }
        public Response(string v)
        {
            this.v = v;
        }

        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Result { get; set; }
        public PagedList<RolesDTO> List { get; internal set; }

        public Response() { }

        public static Response<T> Failure(Exception ex, string message = "Ha ocurrido un error al generar la solicitud")
        {
            var errors = new List<string>
    {
        ex.Message
    };

            if (ex.InnerException != null)
            {
                errors.Add("InnerException: " + ex.InnerException.Message);
                errors.Add("InnerStackTrace: " + ex.InnerException.StackTrace);
            }

            errors.Add("StackTrace: " + ex.StackTrace);

            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }


        public static Response<T> Failure(string message, List<string> errors = null)
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
                Errors = errors
            };
        }

        public static Response<T> Success(T result, string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Result = result,
            };
        }

        public static Response<T> Success(string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
            };
        }

        internal static Response<PaginationResponse<RolesDTO>> Success(Response<PaginationResponse<RolesDTO>> result)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Response<T>(PaginationResponse<RolesDTO> v)
        {
            throw new NotImplementedException();
        }
    }
}
