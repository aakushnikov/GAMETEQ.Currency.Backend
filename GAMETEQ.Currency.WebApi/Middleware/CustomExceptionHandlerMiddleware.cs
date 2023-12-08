using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using FluentValidation;
using GAMETEQ.Currency.Exceptions;
using GAMETEQ.Currency.Model;

namespace GAMETEQ.Currency.WebApi.Middleware;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionsAsync(context, ex);
        }
    }

    private static async Task HandleExceptionsAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;
        switch (exception)
        {
            case ValidationException validationException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize((validationException.Errors));
                break;
            case EntityNotFoundException<CurrencyTick>:
                code = HttpStatusCode.NotFound;
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        if (result == string.Empty)
            result = JsonSerializer.Serialize(new { error = exception.Message });

        await context.Response.WriteAsync(result);
    }
}