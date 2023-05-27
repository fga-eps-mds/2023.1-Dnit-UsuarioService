using AutoMapper;

namespace dominio.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UsuarioDTO, UsuarioDnit>();
            CreateMap<UsuarioDTO, UsuarioTerceiro>();
        }
    }
}
