namespace dominio
{
    public class UsuarioDnit
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
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