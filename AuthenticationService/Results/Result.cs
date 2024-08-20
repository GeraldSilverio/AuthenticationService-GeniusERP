namespace AuthenticationService.Api.Results
{
    public class Result<T> 
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }

        public Exception? Exception { get; set; }   
        public int Code { get; set; }

        /// <summary>
        /// Constructor for success Response.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        public Result(T data,int code)
        {
            Code = code;
            Data = data;
            Success = true;
        }
        /// <summary>
        /// Constructor for unsuccess Response
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        public Result(List<string> errors,int code)
        {
            Code = code;
            Errors = errors;
            Success = false;
        }

        public Result(Exception exception)
        {
            Code = 500;
            Exception = exception;
        }
    }
}
