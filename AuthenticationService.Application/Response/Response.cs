using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthenticationService.Application.Response
{
    public class Response<T> 
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
        public int Code { get; set; }

        /// <summary>
        /// Constructor for success Response.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        public Response(T data,int code)
        {
            Code = code;
            Errors = null;
            Data = data;
            Success = true;
        }
        /// <summary>
        /// Constructor for unsuccess Response
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        public Response(List<string> errors,int code,T data)
        {
            Code = code;
            Errors = errors;
            Success = false;
            Data = data;
        }
    }
}
