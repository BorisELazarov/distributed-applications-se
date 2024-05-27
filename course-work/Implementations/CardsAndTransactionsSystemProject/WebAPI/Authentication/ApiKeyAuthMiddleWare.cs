namespace WebAPI.Authentication
{
    public class ApiKeyAuthMiddleWare
    {
        private readonly RequestDelegate _next;

        public ApiKeyAuthMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName,
                out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API key missing");
                return;
            }

            var _apiKey = AuthConstants.ApiKeyHeaderValue;
            if (!_apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid API key");
                return;
            }

            await _next(context);
        }
    }
}
