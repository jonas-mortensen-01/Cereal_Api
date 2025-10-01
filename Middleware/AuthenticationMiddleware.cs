public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEY_HEADER_NAME = "X-API-KEY";
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration.GetValue<string>("ApiSettings:ApiKey") ?? "";
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_apiKey == "" || !context.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        if (!_apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        await _next(context); // Key is valid, continue
    }
}
