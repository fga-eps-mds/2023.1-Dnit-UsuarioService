using dominio;
using repositorio.Interfaces;
using service.Interfaces;
using System;
using System.Collections.Generic;

namespace service
{
    public class ItemService : IItemService
    {
        private readonly IItemRepositorio itemRepositorio;
        public ItemService(IItemRepositorio itemRepositorio)
        {
            this.itemRepositorio = itemRepositorio;
        }
        public Item Obter(int id)
        {
            Item item = itemRepositorio.Obter(id);

            return item;
        }

        public IEnumerable<Item> ObterLista()
        {
            IEnumerable<Item> listaItens = itemRepositorio.ObterLista();

            return listaItens;
        }

        public void Atualizar(Item item)
        {
            itemRepositorio.Atualizar(item);
        }
    }
}
