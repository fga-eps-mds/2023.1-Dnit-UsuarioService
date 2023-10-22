using AutoMapper;
using api.Senhas;
using api.Usuarios;
using app.Entidades;
using api;
using EnumsNET;


namespace app.Services.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dto => dto.CNPJ, opt => opt.MapFrom(u => u.Empresas.FirstOrDefault().Cnpj));
                
            CreateMap<Usuario, UsuarioModel>()
                .ForMember(dto => dto.Cnpj, opt => opt.MapFrom(u => u.Empresas.FirstOrDefault().Cnpj));

            CreateMap<UF, UfModel>()
                .ForMember(model => model.Id, opt => opt.MapFrom(uf => (int)uf))
                .ForMember(model => model.Sigla, opt => opt.MapFrom(uf => uf.ToString()))
                .ForMember(model => model.Nome, opt => opt.MapFrom(uf => uf.AsString(EnumFormat.Description)));


            CreateMap<UsuarioDTO, UsuarioDnit>()
                .ForMember(usuarioDnit => usuarioDnit.Id, opt => opt.Ignore());

            CreateMap<RedefinicaoSenhaDTO, RedefinicaoSenhaModel>();

            CreateMap<UsuarioDTO, UsuarioTerceiro>()
                .ForMember(usuarioTerceiro => usuarioTerceiro.Id, opt => opt.Ignore());
        }
    }
}