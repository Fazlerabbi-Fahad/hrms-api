using System.Diagnostics;

namespace HRMS.Application.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Success(T data, string? message = "Success", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Data = data,
                Message = message,
                Errors= null
            };
        }

        public static ApiResponse<T> Failure(List<string> errors, string? message = "Failure", int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Data = default,
                Message = message,
                Errors = errors
            };
        }
    }
}
