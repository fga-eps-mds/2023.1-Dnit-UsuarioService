using AutoMapper;
using api.Senhas;
using api.Usuarios;
using app.Entidades;
using api;
using EnumsNET;
using api.Perfis;


namespace app.Services.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dto => dto.CNPJ, opt => opt.MapFrom(u => u.Empresas.FirstOrDefault().Cnpj));
                
            CreateMap<Usuario, UsuarioModel>();

            CreateMap<UF, UfModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(uf => (int)uf))
                .ForMember(model => model.Sigla, opt => opt.MapFrom(uf => uf.ToString()))
                .ForMember(model => model.Nome, opt => opt.MapFrom(uf => uf.AsString(EnumFormat.Description)));


            CreateMap<UsuarioDTO, UsuarioDnit>()
                .ForMember(usuarioDnit => usuarioDnit.Id, opt => opt.Ignore());

            CreateMap<RedefinicaoSenhaDTO, RedefinicaoSenhaModel>();

            CreateMap<UsuarioDTO, UsuarioTerceiro>()
                .ForMember(usuarioTerceiro => usuarioTerceiro.Id, opt => opt.Ignore());

            CreateMap<PerfilDTO, Perfil>()
                .ForMember(p => p.Permissoes, opt => opt.Ignore())
                .ForMember(p => p.PerfilPermissoes, opt => opt.Ignore())
                .ForMember(p => p.Usuarios, opt => opt.Ignore());

            CreateMap<Perfil, PerfilModel>()
                .ForMember(model => model.Permissoes, opt => opt.MapFrom(p => p.Permissoes));
        }
    }
}