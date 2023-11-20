using AutoMapper;
using api.Senhas;
using api.Usuarios;
using app.Entidades;
using api;
using EnumsNET;
using api.Perfis;
using api.Permissoes;
using api.Municipios;
using api.Empresa;

namespace app.Services.Mapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Municipio, MunicipioModel>();

            CreateMap<Usuario, UsuarioModel>();
            
            CreateMap<UsuarioDTO, UsuarioTerceiro>()
                .ForMember(u => u.CNPJ, opt => opt.Ignore());

            CreateMap<UsuarioDTO, UsuarioDnit>();

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
            
            CreateMap<Empresa, EmpresaModel>()
                .ForMember(em => em.UFs, opt => opt.MapFrom(e => e.EmpresaUFs.ConvertAll(d => d.Uf)));

            CreateMap<EmpresaDTO, Empresa>()
                .ForMember(e => e.Cnpj, opt => opt.MapFrom(em => em.Cnpj))
                .ForMember(e => e.RazaoSocial, opt => opt.MapFrom(em => em.RazaoSocial))
                .ForMember(e => e.EmpresaUFs, opt => opt.MapFrom((dto, em) => dto.UFs.ConvertAll(uf => new EmpresaUF {Empresa=em, Uf=uf})))
                .ForMember(e => e.Usuarios, opt => opt.Ignore());

        }
    }
}