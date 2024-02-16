
using ApiKeyValidationMultipleWays.Services;
using System.Net;

namespace ApiKeyValidationMultipleWays.EndpointFilter
{
    public class ApiKeyEndpointFilter : IEndpointFilter
    {
        private readonly IApiKeyValidationService _apiKeyValidationService;

        public ApiKeyEndpointFilter(IApiKeyValidationService apiKeyValidationService)
        {
            _apiKeyValidationService = apiKeyValidationService;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var reqApiKey = context.HttpContext.Request.Headers[Constants.ApiKeyHeaderName];

            if (string.IsNullOrEmpty(reqApiKey))
            {
                return Results.BadRequest();
            }

            if (!_apiKeyValidationService.IsValidApiKey(reqApiKey!))
            {
                return Results.Unauthorized();
            }

            return await next(context);
        }
    }
}
