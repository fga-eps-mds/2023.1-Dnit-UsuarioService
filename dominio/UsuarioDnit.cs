namespace dominio
{
    public class UsuarioDnit
    {
        public int id { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public string nome { get; set; }
        public string UF { get; set; }

        public UsuarioDnit(string email, string senha, string nome, string UF)
        {
            this.email = email;
            this.senha = senha;
            this.nome = nome;
            this.UF = UF;
        }

        public UsuarioDnit(string email, string senha)
        {
            this.email = email;
            this.senha = senha;
        }

        public UsuarioDnit(int id, string nome, string email, string senha)
        {
            this.email = email;
            this.senha = senha;
            this.nome = nome;
            this.id = id;
        }

    }
}