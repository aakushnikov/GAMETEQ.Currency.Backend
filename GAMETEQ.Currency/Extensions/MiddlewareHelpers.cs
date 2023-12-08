using Microsoft.AspNetCore.Builder;

namespace GAMETEQ.Currency.Extensions;

public static class MiddlewareHelpers
{
    public static IApplicationBuilder UseCustomExceptionHandler<T>(this IApplicationBuilder builder) =>
        builder.UseMiddleware<T>();
}