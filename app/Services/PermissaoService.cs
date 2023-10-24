using System.Text.RegularExpressions;
using api;
using api.Permissoes;
using app.Services.Interfaces;
using EnumsNET;

namespace app.Services
{
    public class PermissaoService : IPermissaoService
    {   
        private const string pattern = @"^([A-Z][a-z]+)";
        public PermissaoService()
        {

        }
        public List<string> ObterCategorias()
        {
            var permissoesOrdenadas = Enum.GetNames<Permissao>().OrderBy(str => str);
            
            HashSet<string> categorias = new();

            foreach(var p in permissoesOrdenadas)
            {
                categorias.Add(Regex.Match(p, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100)).ToString());
            }

            return categorias.ToList();
        }

        public List<PermissaoModel> ObterPermissoesPortCategoria(string categoria)
        {   
            var permissoes = Enum.GetValues<Permissao>().Where(p => categoria == Regex.Match(p.ToString(), pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100)).ToString());    

            return permissoes.Select(p => new PermissaoModel
                {
                    Codigo = p,
                    Descricao = p.AsString(EnumFormat.Description)!
                }).ToList();
        }
    }
}