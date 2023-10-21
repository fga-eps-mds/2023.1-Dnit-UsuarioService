namespace api.Perfis
{
    public class PerfilModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public List<Permissao> Permissoes { get; set; }
    }
}
