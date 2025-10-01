using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER_NAME = "X-API-KEY";
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration.GetValue<string>("ApiSettings:ApiKey") ?? string.Empty;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip middleware for Swagger and Scalar docs
        if (context.Request.Path.StartsWithSegments("/docs") ||
            context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }

        // Check for API key in headers
        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        // Validate API key
        if (!_apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        // Key is valid, continue
        await _next(context);
    }
}
