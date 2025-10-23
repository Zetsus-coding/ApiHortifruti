using ApiHortifruti.Exceptions;
using System.Data;
using System.Net;
using System.Text.Json;

namespace Hortifruti.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExeptionAsync(context, ex);
        }
        
    }

    private static Task HandleExeptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string? stackTrace = String.Empty;
        string mensagem;
        var exceptionType = exception.GetType();

        if (exceptionType == typeof(DBConcurrencyException))
        {
            mensagem = exception.Message;
            status = HttpStatusCode.InternalServerError;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(NotFoundExeption))
        {
            mensagem = exception.Message;
            status = HttpStatusCode.NotFound;
            stackTrace = exception.StackTrace;
        }

        //necessita personalizar a exceção BadRequest no front para cada funcionalidade que depende de outra...
        else
        {
            mensagem = exception.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = exception.StackTrace;
        }

        var result = JsonSerializer.Serialize(new { status, mensagem, stackTrace });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) status;
        return context.Response.WriteAsync(result);
    }
}