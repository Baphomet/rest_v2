using Microsoft.EntityFrameworkCore;
using ApiCursos.Context;
using ApiCursos.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connection = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddDbContext<Db>(options =>
options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

builder.Services.AddScoped<AlunoService>();
builder.Services.AddScoped<CursoService>();
builder.Services.AddScoped<UsuarioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
