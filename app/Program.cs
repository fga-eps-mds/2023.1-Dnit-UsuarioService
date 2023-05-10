using app.DI;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DNIT",
        Description = "Backend DNIT"
    });
});

builder.Services.AddConfigServices(builder.Configuration);

builder.Services.AddConfigRepositorios();

builder.Services.AddContexto(builder.Configuration);

var app = builder.Build();

app.UseCors("All");

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
