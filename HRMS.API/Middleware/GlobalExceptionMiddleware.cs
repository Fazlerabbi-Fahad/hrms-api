using HRMS.Application.DTOs.Common;

namespace HRMS.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for {Method} {Path}.",
                                       context.Request.Method,
                                       context.Request.Path);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var { statusCode,message } = exception switch
            {
                InvalidOperationException => (409, exception.Message),
                UnauthorizedAccessException => (401, exception.Message),
                KeyNotFoundException => (404, exception.Message),
                ArgumentException => (400, exception.Message),
                _ => (500, "An unexpected error occurred!")
            };

            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.Failure(null, message, statusCode);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}