using System.Text.RegularExpressions;
using api;
using app.Repositorios.Interfaces;

namespace app.Repositorios
{
    public class PermissaoRepositorio : IPermissaoRepositorio
    {   
        private const string pattern = @"^([A-Z][a-z]+)";
        public PermissaoRepositorio()
        {

        }
        public List<string> ObterCategorias()
        {
            var permissoesOrdenadas = Enum.GetNames<Permissao>().OrderBy(str => str);
            
            HashSet<string> categorias = new();

            foreach(var p in permissoesOrdenadas)
            {
                categorias.Add(Regex.Match(p, pattern).ToString());
            }

            return categorias.ToList();
        }

        public List<Permissao> ObterPermissoesPortCategoria(string categoria)
        {
            var permissoes = Enum.GetValues<Permissao>().Where(p => categoria == Regex.Match(p.ToString(), pattern).ToString());
            return permissoes.ToList();
        }
    }
}