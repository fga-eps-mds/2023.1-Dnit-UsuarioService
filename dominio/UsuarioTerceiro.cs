using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class UsuarioTerceiro : Usuario
    {
        public string CNPJempresa { get; set; }

        public UsuarioTerceiro(string nome, string email, string senha, string CNPJempresa)
        {
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.CNPJempresa = CNPJempresa;
        }
    }
}
