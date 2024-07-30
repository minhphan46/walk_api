using System.Net;
using WalkProject.API.RestFul.DTOs.ApiResponse;

namespace WalkProject.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorResponse = new APIErrorResponse(
                       id: Guid.NewGuid(),
                       statusCode: HttpStatusCode.InternalServerError,
                       message: "Something went wrong! We are looking into resolving this.",
                       errors: new List<string>() { ex.Message }
                    );

                // Log This Exception
                logger.LogError(ex, $"{errorResponse} : {ex.Message}");

                // Return A Custom Exrror Response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
