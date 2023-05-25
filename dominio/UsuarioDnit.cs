namespace dominio
{
    public class UsuarioDnit : Usuario
    {
        public int UF { get; set; }

        public UsuarioDnit(string email, string senha, string nome, int uf)
        {
            this.Email = email;
            this.Senha = senha;
            this.Nome = nome;
            this.UF = uf;
        }

        public UsuarioDnit(string email, string senha)
        {
            this.Email = email;
            this.Senha = senha;
        }

        public UsuarioDnit(int id, string nome, string email, string senha)
        {
            this.Email = email;
            this.Senha = senha;
            this.Nome = nome;
            this.UF = id;
        }
    }
}