namespace dominio
{
    public class UsuarioDnit
    {
        public int ID;
        public string email;
        public string senha;
        public string nome;
        public string UF;

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
    }
}