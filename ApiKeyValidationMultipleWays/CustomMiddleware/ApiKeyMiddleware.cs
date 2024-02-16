using ApiKeyValidationMultipleWays.Services;
using System.Net;
using System.Runtime.CompilerServices;

namespace ApiKeyValidationMultipleWays.CustomMiddleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiKeyValidationService _apiKeyValidationService;
        public ApiKeyMiddleware(RequestDelegate next, IApiKeyValidationService apiKeyValidationService)
        {
            _next = next;
            _apiKeyValidationService = apiKeyValidationService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var reqApiKey = context.Request.Headers[Constants.ApiKeyHeaderName];

            if (string.IsNullOrEmpty(reqApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            if(!_apiKeyValidationService.IsValidApiKey(reqApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await _next(context);

        }

        
    }
}
