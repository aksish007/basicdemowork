using System.Net;
using System.Text.Json;

namespace TaskManagementSystem.API.Middlewares
{
    //public class ExceptionHandlingMiddleware : IMiddleware
    //{
    //    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    //    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    //    {
    //        try
    //        {
    //            await next(context);
    //        }
    //        catch (Exception ex)
    //        {
    //            await HandleExceptionAsync(context, ex);
    //        }
    //    }

    //    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    //    {
    //        var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
    //        var result = string.Empty;

    //        switch (exception)
    //        {
    //            case ApplicationException ex:
    //                // Custom application error
    //                statusCode = HttpStatusCode.BadRequest; // 400
    //                result = JsonSerializer.Serialize(new { error = ex.Message });
    //                break;
    //            case KeyNotFoundException ex:
    //                // Not found error
    //                statusCode = HttpStatusCode.NotFound; // 404
    //                result = JsonSerializer.Serialize(new { error = ex.Message });
    //                break;
    //            default:
    //                // Unhandled error
    //                _logger.LogError(exception, "Unhandled exception");
    //                result = JsonSerializer.Serialize(new { error = "An unexpected error occurred. Please try again later." });
    //                break;
    //        }

    //        context.Response.ContentType = "application/json";
    //        context.Response.StatusCode = (int)statusCode;
    //        return context.Response.WriteAsync(result);
    //    }
    //}

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError; // 500 if unexpected
            var result = string.Empty;

            switch (exception)
            {
                case ApplicationException ex:
                    // Custom application error
                    statusCode = HttpStatusCode.BadRequest; // 400
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;
                case KeyNotFoundException ex:
                    // Not found error
                    statusCode = HttpStatusCode.NotFound; // 404
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;
                default:
                    // Unhandled error
                    _logger.LogError(exception, "Unhandled exception");
                    result = JsonSerializer.Serialize(new { error = "An unexpected error occurred. Please try again later." });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }

}
