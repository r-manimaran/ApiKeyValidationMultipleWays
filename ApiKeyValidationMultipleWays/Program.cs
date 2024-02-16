using ApiKeyValidationMultipleWays.CustomAttribute;
using ApiKeyValidationMultipleWays.CustomMiddleware;
using ApiKeyValidationMultipleWays.EndpointFilter;
using ApiKeyValidationMultipleWays.Policy;
using ApiKeyValidationMultipleWays.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
    {
        policy.AddAuthenticationSchemes(new[] { JwtBearerDefaults.AuthenticationScheme });
        policy.AddRequirements(new ApiKeyRequirement());
    });
});
builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();


// Add services to the container.

builder.Services.AddControllers();

//Register the API Key Validation Service
builder.Services.AddTransient<IApiKeyValidationService, ApiKeyValidationService>();

builder.Services.AddScoped<ApiKeyAuthFilter>();

builder.Services.AddHttpContextAccessor(); //Used in policy based Validation


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Register the Custom Middleware to validate the API key in Header
//app.UseMiddleware<ApiKeyMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("api/Greet", () =>
{
    return Results.Ok();
}).AddEndpointFilter<ApiKeyEndpointFilter>();


app.Run();
