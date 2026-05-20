using Microsoft.EntityFrameworkCore;

using GymApp.API.Extensions;

using GymApp.Application.Interfaces;
using GymApp.Infrastructure.Data;
using GymApp.Infrastructure.Services;
using GymApp.Application.Settings;
using GymApp.API;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddServices();
builder.Services.AddRepositories();

// Jwt
builder.Services.AddSingleton<IJwtService, JwtService>();

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.AddOptions<JwtSettings>()
    .BindConfiguration("Jwt")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddJwtAuthentication(jwtSection.Get<JwtSettings>()!);
builder.Services.AddAuthorization();

// Banco de dados
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if(string.IsNullOrEmpty(ConnectionString))
    throw new Exception("Connection String 'DefaultConnection' não foi encontrada");

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(ConnectionString));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwagger();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// faz as migrações automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
