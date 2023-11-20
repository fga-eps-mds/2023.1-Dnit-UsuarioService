namespace api.Usuarios
{
    public class PesquisaUsuarioFiltro
    {
        public int Pagina { get; set; } = 1;
        public int ItemsPorPagina { get; set; } = 50;
        public string? Nome { get; set; }
        public UF? UfLotacao { get; set; }
        public Guid? PerfilId { get; set; }
        public int? MunicipioId { get; set; }
        public string? Empresa { get; set; }
    }
}