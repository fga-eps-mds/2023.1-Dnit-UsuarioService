using System.Collections.Generic;
using System.ComponentModel;
using api.Perfis;
using app.Entidades;
using api;
using System.Data.Common;
using api.Usuarios;

namespace test.Stub
{
    public class EmpresaStub
    {
        
        public static Empresa RetornarEmpresa(string cnpj ="123456789", string RazaoSocial = "RazãoSocial" ){            
            return new Empresa{
                Cnpj = cnpj,
                RazaoSocial = RazaoSocial,
                // Usuarios = new List<Usuario>{
                //     new Usuario
                //     {
                //         Email = "usuarioteste@gmail.com1",
                //         Senha = "senha12341",
                //         Nome = "Usuario1",
                //         UfLotacao = UF.AC
                //     }
                // },
                EmpresaUFs = new List<EmpresaUF>{}
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

        // public static Usuario RetornarUsuario()
        // {
        //     return new Usuario
        //     {
        //         Email = "usuarioteste@gmail.com1",
        //         Senha = "senha1234",
        //         Nome = "Usuario",
        //         UfLotacao = UF.AC
        //     };
        // }
    }  
}
