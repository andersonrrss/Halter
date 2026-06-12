using Microsoft.EntityFrameworkCore;

using Halter.API.Extensions;

using Halter.Application.Interfaces;
using Halter.Infrastructure.Data;
using Halter.Infrastructure.Services;
using Halter.Application.Settings;
using Halter.API;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Dev");
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

if(app.Environment.IsDevelopment())
    app.UseCors("Dev"); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
