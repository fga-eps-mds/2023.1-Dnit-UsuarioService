namespace api.Usuarios
{
    public class UsuarioDnit : UsuarioModel
    {
        public UF UfLotacao { get; set; }
    }

    public class UsuarioDnitNovo : UsuarioDTONovo
    {
        public UF UfLotacao { get; set; }
    }
}