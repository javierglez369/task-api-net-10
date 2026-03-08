using Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Registrar el repositorio como Singleton:
// La misma instancia se comparte en todas las peticiones.
// Necesario para que el estado (lista de tareas) persista entre llamadas.
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
