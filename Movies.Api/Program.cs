using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Movies.Application.AppRegistry;
using Movies.Application.Database;
using Movies.Mvc.Api.Auth;
using Movies.Mvc.Api.Health;
using Movies.Mvc.Api.Mapping;
using Movies.Mvc.Api.Swagger;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// adding JWT Authentication
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        ValidateIssuer = true,
        ValidateAudience = true
    };
});

// adding Authorization
builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy(AuthConstants.JwtOrApiKeyPolicyName, authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.AddRequirements(new AdminAuthRequirement(config["ApiKey"]!));
    });

    authorizationOptions.AddPolicy(AuthConstants.AdminUserPolicyName, authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireClaim(AuthConstants.AdminUserClaimName, "true");
    });

    authorizationOptions.AddPolicy(AuthConstants.TrustedMemberPolicyName, authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireAssertion(context =>
            context.User.HasClaim(claim => claim is { Type: AuthConstants.AdminUserClaimName, Value: "true" }) ||
            context.User.HasClaim(claim => claim is { Type: AuthConstants.TrustedMemberClaimName, Value: "true" }));
    });
});


// Add services to the container.

// Enabling OData
builder.Services
    .AddControllers()
    .AddOData(option => option
        .Select()
        .Filter()
        .OrderBy());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom application services
builder.Services.AddApplication();
// Register database with connection string
builder.Services.AddDatabase(config["Database:ConnectionString"]!);
// Register custom fluent validation middleware
builder.Services.AddSingleton<ValidationMappingMiddleware>();
// api versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
});

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Adding custome database health check
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);

builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

// using health check
app.MapHealthChecks("_health");

// using JWT Authentication
app.UseAuthentication();
// using Authorization
app.UseAuthorization();

// using custom fluent validation middleware
app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

// Initialising movies and genres db schema in postgressql
var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
