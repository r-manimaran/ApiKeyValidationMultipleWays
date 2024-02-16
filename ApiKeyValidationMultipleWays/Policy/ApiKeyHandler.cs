using ApiKeyValidationMultipleWays.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiKeyValidationMultipleWays.Policy;

public class ApiKeyHandler: AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApiKeyValidationService _apiKeyValidationService;

    public ApiKeyHandler(IHttpContextAccessor httpContextAccessor,
                        IApiKeyValidationService apiKeyValidationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _apiKeyValidationService = apiKeyValidationService;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        var reqApiKey = _httpContextAccessor?.HttpContext?.Request.Headers[Constants.ApiKeyHeaderName];

        if (string.IsNullOrEmpty(reqApiKey))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (!_apiKeyValidationService.IsValidApiKey(reqApiKey!))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}
