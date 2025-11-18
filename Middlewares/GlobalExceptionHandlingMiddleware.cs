using ApiHortifruti.Exceptions;
using System.Data;
using System.Net;
using System.Text.Json;

namespace ApiHortifruti.Middlewares;

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
        else if (exceptionType == typeof(InvalidOperationException))
        {
            mensagem = exception.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(KeyNotFoundException))
        {
            mensagem = exception.Message;
            status = HttpStatusCode.NotFound;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(ArgumentException))
        {
            mensagem = exception.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = exception.StackTrace;
        }
        else if (exceptionType == typeof(ArgumentNullException))
        {
            mensagem = exception.Message;
            status = HttpStatusCode.BadRequest;
            stackTrace = exception.StackTrace;
        }
        else
        {
            // Tratar a exceção genérica como Erro Interno do Servidor (500)
            mensagem = exception.Message; // Mensagem mais segura para o cliente
            status = HttpStatusCode.InternalServerError; // 500 para erros não tratados
            stackTrace = exception.StackTrace; // Incluir para log/debug, mas não no ambiente de produção
        }

        var result = JsonSerializer.Serialize(new { status, mensagem, stackTrace });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        return context.Response.WriteAsync(result);
    }
}