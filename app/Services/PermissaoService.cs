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

        public List<CategoriaPermissaoModel> CategorizarPermissoes(List<Permissao> permissaos)
        {
            var categorias = ObterCategorias();

            return categorias.ConvertAll(c => new CategoriaPermissaoModel
            {
                Categoria = c,
                Permissoes = ObterPermissoesPorCategoria(c, permissaos)
            });
        }

        public List<string> ObterCategorias()
        {
            var permissoesOrdenadas = Enum.GetNames<Permissao>().OrderBy(str => str);

            var categorias = new HashSet<string>();

            foreach (var p in permissoesOrdenadas)
            {
                categorias.Add(Regex.Match(p, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100)).ToString());
            }

            return categorias.ToList();
        }

        public List<PermissaoModel> ObterPermissoesPorCategoria(string categoria, List<Permissao> permissoes)
        {
            return permissoes
                .Where(p => categoria == Regex.Match(p.ToString(), pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100)).ToString())
                .Select(p => new PermissaoModel
                {
                    Codigo = p,
                    Descricao = p.AsString(EnumFormat.Description)!
                }).ToList();
        }
    }
}