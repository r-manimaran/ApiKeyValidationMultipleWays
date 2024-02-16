namespace ApiKeyValidationMultipleWays.Services
{
    public class ApiKeyValidationService : IApiKeyValidationService
    {
        private readonly IConfiguration _configuration;

        public ApiKeyValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public bool IsValidApiKey(string apiKeyFromRequest)
        {
            if (string.IsNullOrEmpty(apiKeyFromRequest))
            {
                return false;
            }

            var apiKey = _configuration.GetValue<string>(Constants.ApiKey);
            if(apiKey is null || apiKey!= apiKeyFromRequest)
            {
                return false;
            }

            return true;
        }
    }
}
