using System;

namespace Amazon.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request , you have made",
                401 => "Authrized, Ypu are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark Side",
                _ => null
            };
        }
    }
}
