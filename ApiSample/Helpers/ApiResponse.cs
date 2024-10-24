namespace ApiSample.Helpers
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public ApiResponse() { }

        public ApiResponse(T data, bool success, string message, int statusCode)
        {
            Data = data;
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }

        public ApiResponse(bool success, string message, int statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }
    }

    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ResponseCode { get; set; }
        public ApiResponse() { }

        public ApiResponse(bool success, string message, int statusCode, string responseCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            ResponseCode = responseCode;
        }
        public ApiResponse(bool success, string message, int statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }

        public ApiResponse(bool success, int statusCode)
        {
            Success = success;
            StatusCode = statusCode;
        }
    }
}
