using app.DI;
using app.Entidades;
using app.Services.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddEndpointsApiExplorer();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7083);
});

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UsuarioService",
        Description = "Microserviço UsuarioService"
    });
});

builder.Services.AddConfiguracoes(builder.Configuration);

builder.Services.AddConfigServices(builder.Configuration);

builder.Services.AddConfigRepositorios();

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

DotNetEnv.Env.Load();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseSwagger();

app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
    dbContext.Popula();
}

app.Run();
