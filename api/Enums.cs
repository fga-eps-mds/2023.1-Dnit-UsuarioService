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

        [Description("Cadastrar Perfil de Usuário")]
        PerfilCadastrar = 3000,
        [Description("Editar Perfil de Usuário")]
        PerfilEditar = 3001,
        [Description("Remover Perfil de Usuário")]
        PerfilRemover = 3002,
        [Description("Visualizar perfis")]
        PerfilVisualizar = 3003,

        [Description("Cadastrar rodovia")]
        RodoviaCadastrar = 6000,

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
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoPerfil
    {
        Basico = 1,
        Administrador,
        Customizavel
    }
}
