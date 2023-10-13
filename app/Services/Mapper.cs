using AutoMapper;
using api.Senhas;
using api.Usuarios;

namespace app.Services.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UsuarioDTO, UsuarioDnit>()
                .ForMember(usuarioDnit => usuarioDnit.Id, opt => opt.Ignore());

            CreateMap<RedefinicaoSenhaDTO, RedefinicaoSenhaModel>();

            CreateMap<UsuarioDTO, UsuarioTerceiro>()
                .ForMember(usuarioTerceiro => usuarioTerceiro.Id, opt => opt.Ignore());
        }
    }
}