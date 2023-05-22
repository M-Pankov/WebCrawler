using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebCrawler.Presentation.WebApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = exception switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest,
            UriFormatException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

        var exceptionResult = JsonSerializer.Serialize(new { StatusCode = statusCode, Error = exception.Message });

        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(exceptionResult);
    }
}
