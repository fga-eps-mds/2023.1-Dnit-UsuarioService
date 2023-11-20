using System.Threading.Tasks;
using api;
using api.Empresa;
using app.Entidades;
using app.Repositorios.Interfaces;
using app.Services.Interfaces;
using AutoMapper;
using test.Fixtures;
using test.Stub;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace test
{
    public class EmpresaServiceTest: TestBed<Base>, IDisposable
    {
        private readonly IEmpresaService empresaService;
        private readonly IEmpresaRepositorio empresaRepositorio;

        private readonly IMapper mapper;
        private readonly AppDbContext dbContext;

        public EmpresaServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            empresaService = fixture.GetService<IEmpresaService>(testOutputHelper)!;
            empresaRepositorio = fixture.GetService<IEmpresaRepositorio>(testOutputHelper)!;
            mapper = fixture.GetService<IMapper>(testOutputHelper)!;
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
        }
        [Fact]
        public async Task CriarEmpresa_QuandoEmpresaPassado_DeveRetornarEmpresaRegistrado()
        {
            var empresaDTO = EmpresaStub.RetornarEmpresaDTO(cnpj : "123");

            var empresa = mapper.Map<Empresa>(empresaDTO);

            await empresaService.CadastrarEmpresa(empresa);

            var empresaDb = await empresaRepositorio.ObterEmpresaPorCnpjAsync(empresa.Cnpj);

            Assert.NotNull(empresaDb);
            Assert.Equal(empresaDTO.RazaoSocial, empresaDb.RazaoSocial);
        }

        [Fact]
        public async Task EditarEmpresa_QuandoEmpresaJaCadastrado_DeveRetornarEmpresaAtualizado()
        {
            var empresaDTO = EmpresaStub.RetornarEmpresaDTO();
            var empresaDTOEdicao = EmpresaStub.RetornarEmpresaDTO(RazaoSocial: "Novo Nome");
            

            var empresa = mapper.Map<Empresa>(empresaDTO);
            var empresaEdicao = mapper.Map<Empresa>(empresaDTOEdicao);

            await empresaService.CadastrarEmpresa(empresa);

            empresaEdicao.Cnpj = empresa.Cnpj;
            var empresaRetornoEditado = await empresaService.EditarEmpresa(empresa.Cnpj, empresaEdicao);

            var empresaDb = await empresaRepositorio.ObterEmpresaPorCnpjAsync(empresaRetornoEditado.Cnpj);

            Assert.NotNull(empresa);
            Assert.NotNull(empresaEdicao);
            Assert.NotNull(empresaDb);
            Assert.Equal(empresa.Cnpj, empresaDb.Cnpj);
            Assert.Equal(empresaEdicao.RazaoSocial, empresaDb.RazaoSocial);
            Assert.NotEqual(empresaDb.RazaoSocial, empresaDTO.RazaoSocial);
            
        }

        [Fact]
        public async Task ExcluirEmpresa_QuandoEmpresaExiste_DeveExcluirEmpresaDoDB()
        {
            var empresaDTO = EmpresaStub.RetornarEmpresaDTO(cnpj:"12");
            var empresa = mapper.Map<Empresa>(empresaDTO);

            await empresaService.CadastrarEmpresa(empresa);

            await empresaService.DeletarEmpresa(empresa.Cnpj);

            var perfilDb = await empresaRepositorio.ObterEmpresaPorCnpjAsync(empresa.Cnpj);

            Assert.Null(perfilDb);
        }

        [Fact]
        public async Task VisualizarEmpresa_QuandoEmpresaExiste_DeveRetornarEmpresaDoDB()
        {
            var empresaDTO = EmpresaStub.RetornarEmpresaDTO(cnpj:"123456");
            var empresa = mapper.Map<Empresa>(empresaDTO);

            await empresaService.CadastrarEmpresa(empresa);

            var empresaVisalizar = empresaService.VisualizarEmpresa(empresa.Cnpj);

            var perfilDb = await empresaRepositorio.ObterEmpresaPorCnpjAsync(empresaVisalizar.Cnpj);

            Assert.NotNull(perfilDb);
        }


        [Fact]
        public async Task ListarPerfis_QuandoExistir_DeveRetornarListaDePerfis()
        {
            var lista = EmpresaStub.RetornaListaDeEmpresas(5);

            lista.ForEach(p => empresaService.CadastrarEmpresa(p));

            var listaRetorno = await empresaService.ListarEmpresas(1, 5);

            Assert.Equal(5, listaRetorno.ItemsPorPagina);
        }
   
    }
}