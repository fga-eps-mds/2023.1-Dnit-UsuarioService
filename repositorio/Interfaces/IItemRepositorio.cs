using dominio;
using System.Collections.Generic;

namespace repositorio.Interfaces
{
    public interface IItemRepositorio
    {
        public Item Obter(int id);
        public IEnumerable<Item> ObterLista();
        public void Atualizar(Item item);
    }
}
