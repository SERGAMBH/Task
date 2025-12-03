using Microsoft.EntityFrameworkCore;
using webapi.Models;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация для production
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        // Конфигурация Kestrel для production
        serverOptions.AddServerHeader = false;
        serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
    });
}

// Add services to the container.

builder.Services.AddSingleton<webapi.Interfaces.ITodoRepository, webapi.Services.TodoRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionUsers")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    context.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

// Для production за фронтендом (nginx)
if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();