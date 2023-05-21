using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;

using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.AppRegistry;
using Movies.Application.Database;

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom application services
builder.Services.AddApplication();
// Register database with connection string
builder.Services.AddDatabase(config["Database:ConnectionString"]!);
// Register custom fluent validation middleware
builder.Services.AddSingleton<ValidationMappingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
