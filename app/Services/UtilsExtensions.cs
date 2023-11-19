using api;
using EnumsNET;

namespace app.Services
{
    public static class UtilsExtensions
    {
        public static List<Permissao> ToList(this Permissao[] ps, bool comInternas = false)
        {
            return ps.AsEnumerable().ToList(comInternas);
        }

        public static List<Permissao> ToList(this IEnumerable<Permissao> ps, bool comInternas = false)
        {
            if (!comInternas)
            {
                return new List<Permissao>(ps.Where(p => !p.AsString(EnumFormat.Description)!.StartsWith('_')));
            }
            return new List<Permissao>(ps);
        }
    }
}
