using dominio;
using Microsoft.AspNetCore.Mvc;
using service;
using service.Interfaces;

namespace app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService service;

        public ItemController(IItemService service)
        {
            this.service = service;
        }

        [HttpGet("item")]
        public Item Obter([FromQuery] int id)
        {
            Item item = service.Obter(id);

            return item;
        }

        [HttpGet("listaItens")]
        public IEnumerable<Item> ObterLista()
        {
            IEnumerable<Item> listaItens = service.ObterLista();

            return listaItens;
        }

        [HttpPut("item")]
        public IActionResult Atualizar([FromBody] Item item)
        {
            try
            {
                service.Atualizar(item);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
