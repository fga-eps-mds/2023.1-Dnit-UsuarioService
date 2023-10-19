using System.ComponentModel;

namespace api
{
    public enum UF
    {
        [Description("Acre")]
        AC = 1,
        [Description("Alagoas")]
        AL,
        [Description("Amapá")]
        AP,
        [Description("Amazonas")]
        AM,
        [Description("Bahia")]
        BA,
        [Description("Ceará")]
        CE,
        [Description("Espírito Santo")]
        ES,
        [Description("Goiás")]
        GO,
        [Description("Maranhão")]
        MA,
        [Description("Mato Grosso")]
        MT,
        [Description("Mato Grosso do Sul")]
        MS,
        [Description("Minas Gerais")]
        MG,
        [Description("Pará")]
        PA,
        [Description("Paraíba")]
        PB,
        [Description("Paraná")]
        PR,
        [Description("Pernambuco")]
        PE,
        [Description("Piauí")]
        PI,
        [Description("Rio de Janeiro")]
        RJ,
        [Description("Rio Grande do Norte")]
        RN,
        [Description("Rio Grande do Sul")]
        RS,
        [Description("Rondônia")]
        RO,
        [Description("Roraima")]
        RR,
        [Description("Santa Catarina")]
        SC,
        [Description("São Paulo")]
        SP,
        [Description("Sergipe")]
        SE,
        [Description("Tocantins")]
        TO,
        [Description("Distrito Federal")]
        DF
    }

    public enum Permissao
    {
        [Description("Sem permissões")]
        None = 0,
        [Description("Cadastrar Empresa")]
        EmpresaCadastrar = 1,
        [Description("Editar Empresa")]
        EmpresaEditar = 2,
        [Description("Remover Empresa")]
        RemoverEmpresa = 3,

        [Description("Cadastrar Escola")]
        CadastrarEscola = 4,
        [Description("Editar Escola")]
        EditarEscola = 5,
        [Description("Remover Escola")]
        RemoverEscola = 6,

        [Description("Cadastrar Perfil")]
        CadastrarPerfil = 7,
        [Description("Editar Perfil")]
        EditarPerfil = 8,
        [Description("Remover Perfil")]
        RemoverPerfil = 9,
    }
}