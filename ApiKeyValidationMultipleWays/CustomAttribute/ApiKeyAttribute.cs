using Microsoft.AspNetCore.Mvc;

namespace ApiKeyValidationMultipleWays.CustomAttribute
{
    public class ApiKeyAttribute:ServiceFilterAttribute
    {
        public ApiKeyAttribute():base(typeof(ApiKeyAuthFilter))
        {
                
        }
    }
}
