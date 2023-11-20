using app.Controller;
using app.Controllers;
using app.Entidades;
using app.Repositorios;
using app.Repositorios.Interfaces;
using app.Services;
using app.Services.Interfaces;
using app.Services.Mapper;
using auth;
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
            services.AddScoped<IEmpresaRepositorio, EmpresaRepositorio>();

            // Services
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddScoped<IPermissaoService, PermissaoService>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IEmpresaService, EmpresaService>();

            // Controllers
            services.AddScoped<DominioController>();
            services.AddScoped<UsuarioController>();
            services.AddScoped<PerfilController>();
            services.AddScoped<EmpresaController>();

            services.AddAuth(configuration);
        }

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.Test.json", IsOptional = false };
        }
    }
}