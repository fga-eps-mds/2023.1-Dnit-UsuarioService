using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class UsuarioTerceiro : Usuario
    {
        public int CNPJempresa { get; set; }

        public UsuarioTerceiro(string nome, string email, string senha, int CNPJempresa)
        {
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.CNPJempresa = CNPJempresa;
        }
    }
}
