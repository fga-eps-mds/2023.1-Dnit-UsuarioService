namespace api.Empresa 
{
    public class EmpresaModel 
    {
        public string Cnpj { get; set; }
        public string RazaoSocial { get; set; }
        public List<UfModel> UFs { get; set; }
    }
}