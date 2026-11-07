using ProgramasFacpya.Api.Common;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddFluentValidationConfig();
builder.Services.AddOpenApi();

// Registrar DbContext (conexion a BD)
builder.Services.AddSingleton<ProgramasFacpya.Api.Data.DbContext>();

// Registrar Services
builder.Services.AddScoped<ProgramasFacpya.Api.Features.Alumnos.AlumnoService>();
builder.Services.AddScoped<ProgramasFacpya.Api.Features.Maestros.MaestroService>();
builder.Services.AddScoped<ProgramasFacpya.Api.Features.Carreras.CarreraService>();

// CORS - para que el FRONT pueda conectarse
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();