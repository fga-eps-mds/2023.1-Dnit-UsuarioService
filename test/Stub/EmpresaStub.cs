using System.Collections.Generic;
using app.Entidades;
using api;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Empresa;

namespace test.Stub
{
    public class EmpresaUsuarioStub: Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
    public static class EmpresaStub
    {
        static readonly UF[] ListaUfs = Enum.GetValues<UF>();

        static private UF UfAleatoria()
        {
            return ListaUfs[Random.Shared.Next() % ListaUfs.Length];
        }

        public static Empresa RetornarEmpresa(string cnpj ="123456789", string RazaoSocial = "RazãoSocial" )
        {           
            return new Empresa{
                Cnpj = cnpj,
                RazaoSocial = RazaoSocial,
                Usuarios = new List<Usuario>{},
                EmpresaUFs = new List<EmpresaUF>{}
            };
        }

        public static Usuario RetornarUsuarioValidoLogin()
        {
            return new Usuario
            {
                Id = 1234,
                Email = "usuarioteste@gmail.com",
                Senha = "$2a$11$p0Q3r8Q7pBBcfoW.EIdvvuosHDfgr6TBBOxQvpnG18fLLlHjC/J6O",
                Nome = "Usuario Dnit",
                UfLotacao = UfAleatoria(),
                
            };
        }

        public static IEnumerable<Empresa> Listar()
        {
            while (true)
            {
                yield return new Empresa()
                {
                    Cnpj = "teste " + Random.Shared.Next().ToString(),
                    RazaoSocial = $"testeRazao{Random.Shared.Next()}",
                    
                };
            }
        }

        public static EmpresaDTO RetornarEmpresaDTO(string cnpj = "123456789" , string RazaoSocial = "RazaoSocial Teste")
        {
            return new EmpresaDTO
            {
                Cnpj = cnpj,
                RazaoSocial = RazaoSocial,
                UFs = new List<UF>{UF.CE}
            };
        }

        public static List<Empresa> RetornaListaDeEmpresas(int n = 4)
        {
            var lista = new List<Empresa>();

            for(int i = 0; i < n; i++)
            {
                lista.Add(RetornarEmpresa("EmpresaTeste_" + i.ToString()));
            }

            return lista;
        }

        public static List<EmpresaDTO> RetornaListaEmpresaDTO(int n = 4)
        {
            var lista = new List<EmpresaDTO>();

            for (int i = 0; i < n; i++)
            {
                lista.Add(RetornarEmpresaDTO("Perfil" + i.ToString()));               
            }

            return lista;
        }
    }  
}
