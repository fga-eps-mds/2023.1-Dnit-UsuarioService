using AutoMapper;

namespace dominio.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UsuarioDTO, UsuarioDnit>()
                .ForMember(usuarioDnit => usuarioDnit.Id, opt => opt.Ignore());

            CreateMap<RedefinicaoSenhaDTO, RedefinicaoSenha>();

            CreateMap<UsuarioDTO, UsuarioTerceiro>()
                .ForMember(usuarioTerceiro => usuarioTerceiro.Id, opt => opt.Ignore());
        }
    }
}
