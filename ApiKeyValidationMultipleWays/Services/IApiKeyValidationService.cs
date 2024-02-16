namespace ApiKeyValidationMultipleWays.Services
{
    public interface IApiKeyValidationService
    {
        bool IsValidApiKey(string apiKey);
    }
}
