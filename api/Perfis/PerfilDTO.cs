namespace api.Perfis
{
    public class PerfilDTO
    {
        public string Nome { get; set; }
        public List<Permissao> Permissoes { get; set; }
    }
}