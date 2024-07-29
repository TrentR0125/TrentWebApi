namespace TrentWebApi.Auth
{
    public class ApiKeyMiddleware
    {
        public const string API_KEY = "tempkey";
        public const string API_KEY_HEADER = "X-Trent-APIKey";

        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKey) || API_KEY != apiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Could not find API Key.");

                return;
            }

            await _next(context);
        }
    }
}
