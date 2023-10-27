using api.Perfis;

namespace api.Usuarios
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
    }

    public class UsuarioModelNovo
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public Guid? PerfilId { get; set; }
        public PerfilModel? Perfil { get; set; }
        public UF UfLotacao { get; set; }
        public Guid? Municipio { get; set; }
    }
}
