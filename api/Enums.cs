using System.ComponentModel;
using System.Text.Json.Serialization;

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

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Permissao
    {
        [Description("Cadastrar Escola")]
        EscolaCadastrar = 1000,
        [Description("Editar Escola")]
        EscolaEditar = 1001,
        [Description("Remover Escola")]
        EscolaRemover = 1002,
        [Description("Visualizar Escola")]
        EscolaVisualizar = 1003,

        //[Description("Cadastrar Empresa")]
        //EmpresaCadastrar = 2000,
        //[Description("Editar Empresa")]
        //EmpresaEditar = 2001,
        //[Description("Remover Empresa")]
        //EmpresaRemover = 2002,

        [Description("Cadastrar Perfil de Usuário")]
        PerfilCadastrar = 3000,
        [Description("Editar Perfil de Usuário")]
        PerfilEditar = 3001,
        [Description("Remover Perfil de Usuário")]
        PerfilRemover = 3002,
        [Description("Visualizar perfis")]
        PerfilVisualizar = 3003,

        [Description("Calcular UPS de sinistros")]
        UpsCalcularSinistro = 5000,
        [Description("Calcular UPS de escolas")]
        UpsCalcularEscola = 5001,
        [Description("Visualizar UPS")]
        UpsVisualizar = 5002,

        [Description("Cadastrar rodovia")]
        RodoviaCadastrar = 6000,

        [Description("Cadastrar sinistro")]
        SinistroCadastrar = 7000,

        [Description("Visualizar Usuário")]
        UsuarioVisualizar = 8003,
        [Description("Editar Perfil Usuário")]
        UsuarioPerfilEditar = 8004,
    }

    public enum ErrorCodes
    {
        Unknown,
        [Description("Usuário não possui permissão para realizar ação")]
        NaoPermitido,
        [Description("Usuário não encontrado")]
        UsuarioNaoEncontrado,
        [Description("Código UF inválido")]
        CodigoUfInvalido,
        [Description("Permissao não encontrada")]
        PermissaoNaoEncontrada,
        [Description("Email já cadastrado")]
        EmailUtilizado,
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoPerfil
    {
        Basico = 1,
        Administrador,
        Customizavel
    }
}
