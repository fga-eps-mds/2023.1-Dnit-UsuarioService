namespace api.Usuarios
{
    public class UsuarioTerceiro : UsuarioModel
    {
        public string CNPJ { get; set; }
    }
    
    public class UsuarioTerceiroNovo : UsuarioDTONovo
    {
        public string CNPJ { get; set; }
    }
}
