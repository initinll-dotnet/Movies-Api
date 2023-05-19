using Movies.Api.Mapping;
using Movies.Application.AppRegistry;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

// using custom fluent validation middleware
app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

// Initialising movies and genres db schema in postgressql
var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
await dbInitializer.InitializeAsync();

app.Run();
