using System.Net;
using System.Text.Json;

namespace EmployeeManagement.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}