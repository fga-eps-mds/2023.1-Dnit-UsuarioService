using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class UsuarioDNIT
    {
        int ID;
        string email;
        string senha;
        string nome;
        string UF;

        public UsuarioDNIT(string email, string senha, string nome, string UF)
        {
            this.email = email;
            this.senha = senha;
            this.nome = nome;
            this.UF = UF;
        }

        public UsuarioDNIT(string email, string senha)
        {
            this.email = email;
            this.senha = senha;
        }
    }
}