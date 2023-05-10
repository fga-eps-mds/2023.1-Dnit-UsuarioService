using dominio;
using System.Collections.Generic;

namespace service.Interfaces
{
    public interface IItemService
    {
        public Item Obter(int id);

        public IEnumerable<Item> ObterLista();
        public void Atualizar(Item item);
    }
}
