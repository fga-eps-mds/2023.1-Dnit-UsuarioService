using app.Controllers;
using app.Entidades;
using app.Repositorios;
using app.Repositorios.Interfaces;
using app.Services;
using app.Services.Interfaces;
using app.Services.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test.Fixtures
{
    public class Base : TestBedFixture
    {
        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
            // Para evitar a colisão durante a testagem paralela, o nome deve ser diferente para cada classe de teste
            var databaseName = "DbInMemory" + Random.Shared.Next().ToString();
            services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(databaseName));

            // Repositorios
            services.AddScoped<IUnidadeFederativaRepositorio, UnidadeFederativaRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddScoped<IPerfilRepositorio, PerfilRepositorio>();

            // Services
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddScoped<IPermissaoService, PermissaoService>();

            // Controllers
            services.AddScoped<DominioController>();
            services.AddScoped<UsuarioController>();
        }

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = false };
        }
    }
}