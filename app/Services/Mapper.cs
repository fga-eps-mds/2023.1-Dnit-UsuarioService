using AutoMapper;
using api.Senhas;
using api.Usuarios;
using app.Entidades;
using api;
using EnumsNET;
using api.Perfis;
using api.Permissoes;

namespace app.Services.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Usuario, UsuarioModel>()
                .ForMember(u => u.Cnpj, opt => opt.Ignore());
            
            CreateMap<UsuarioDTO, UsuarioTerceiro>()
                .ForMember(u => u.CNPJ, opt => opt.Ignore());

            CreateMap<UsuarioDTO, UsuarioDnit>()
                .ForMember(u => u.UfLotacao, opt => opt.Ignore());

            CreateMap<UF, UfModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(uf => (int)uf))
                .ForMember(model => model.Sigla, opt => opt.MapFrom(uf => uf.ToString()))
                .ForMember(model => model.Nome, opt => opt.MapFrom(uf => uf.AsString(EnumFormat.Description)));

            CreateMap<RedefinicaoSenhaDTO, RedefinicaoSenhaModel>();

            CreateMap<PerfilDTO, Perfil>()
                .ForMember(p => p.Id, opt => opt.Ignore())
                .ForMember(p => p.Permissoes, opt => opt.Ignore())
                .ForMember(p => p.PerfilPermissoes, opt => opt.Ignore())
                .ForMember(p => p.Usuarios, opt => opt.Ignore())
                .ForMember(p => p.Tipo, opt => opt.Ignore())
                .ForMember(p => p.PermissoesSessao, opt => opt.Ignore());
            
            CreateMap<Perfil, PerfilModel>()
                .ForMember(model => model.Permissoes, opt => opt.MapFrom
                    (
                        perf => perf.Permissoes.Select(p => new PermissaoModel
                            {
                                Codigo = p,
                                Descricao = p.AsString(EnumFormat.Description)!
                            }).ToList()
                    )
                )
                .ForMember(model => model.QuantidadeUsuarios, opt => opt.MapFrom(p => p.Usuarios.Count()))
                .ForMember(model => model.CategoriasPermissao, opt => opt.Ignore());
        }
    }
}