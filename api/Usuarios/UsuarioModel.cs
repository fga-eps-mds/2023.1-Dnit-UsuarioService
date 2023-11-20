using api.Empresa;
using api.Municipios;
using api.Perfis;

namespace api.Usuarios
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public EmpresaModel? empresa{ get; set; }
        public Guid? PerfilId { get; set; }
        public PerfilModel? Perfil { get; set; }
        public UF UfLotacao { get; set; }

        public int? MunicipioId { get; set; }

        public MunicipioModel? Municipio { get; set; }
    }
}
