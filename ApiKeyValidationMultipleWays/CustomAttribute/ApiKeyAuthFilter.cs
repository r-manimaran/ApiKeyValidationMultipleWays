using ApiKeyValidationMultipleWays.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiKeyValidationMultipleWays.CustomAttribute
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IApiKeyValidationService _apiKeyValidationService;
        public ApiKeyAuthFilter(IApiKeyValidationService apiKeyValidationService)
        {
            _apiKeyValidationService = apiKeyValidationService;
        }
        
        
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Get the API Key from Header
            var requestAPIKey = context.HttpContext.Request.Headers[Constants.ApiKeyHeaderName];

            if(string.IsNullOrEmpty(requestAPIKey) )
            {
                context.Result = new BadRequestResult();
                return;
            }

            if (!_apiKeyValidationService.IsValidApiKey(requestAPIKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

        }
    }
}
