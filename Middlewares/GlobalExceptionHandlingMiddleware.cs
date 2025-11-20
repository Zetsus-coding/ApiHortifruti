using ApiHortifruti.Exceptions;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
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
        else if (exceptionType == typeof(DbUpdateException))
        {
            if (exception.InnerException is MySqlException mySqlException) // Verifica se a exceção é do MySQL
            {
                switch (mySqlException.Number)
                {
                    case 1451: // Restrição de chave estrangeira (referenciada em outro lugar)
                        mensagem = "Não é possivel excluir o registro pois o mesmo está sendo utilizado em outro lugar. Por favor, verifique outros registros relacionados a este.";
                        status = HttpStatusCode.Conflict; // 409 Conflito
                        stackTrace = exception.StackTrace;
                        break;
                    case 1452: // Restrição de chave estrangeira (chave informada não existe)
                        mensagem = "Não é possível adicionar este registro pois a referência informada não existe.";
                        status = HttpStatusCode.Conflict;
                        break;

                    case 1062: // Tentativa de inserção de registro duplicado (Unique Key)
                        mensagem = "Já existe um registro com esses dados (duplicado).";
                        status = HttpStatusCode.Conflict;
                        break;
                    default: // Outras exceções do MySQL não tratadas
                        mensagem = "Erro ao realizar a operação no banco de dados.Exceção ainda não tratada. ERRO:" + exception.Message;
                        status = HttpStatusCode.InternalServerError;
                        stackTrace = exception.StackTrace;
                        break;
                }
            }
            else
            {
                mensagem = "Erro ao salvar dados no banco. ERROR_MSG: " + exception.Message;
                status = HttpStatusCode.InternalServerError;
                stackTrace = exception.StackTrace;
            }
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