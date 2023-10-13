namespace api.Usuarios
{
    public class UsuarioDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public int? UF { get; set; }
        public string? CNPJ { get; set; }
    }
}
