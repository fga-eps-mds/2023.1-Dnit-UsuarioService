using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class UsuarioDNIT
    {
        public int ID { get; set; }
        public string email { get;  set; }
        public string senha { get;  set; }
        public string nome { get; set; }
        public string UF { get; set; }

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