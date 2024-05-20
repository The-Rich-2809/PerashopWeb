using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;

namespace PeroShopWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CombosController : ControllerBase
    {
        private readonly PerashopDB _contextDB;

        public CombosController(PerashopDB perashopDB)
        {
            _contextDB = perashopDB;
        }

        [HttpGet("ValorCombos")]
        public ActionResult<List<ComboBoxItem>> ValorCombos(int valor, int idprod)
        {
            var listproductoInter = _contextDB.ProductoInter.Where(p => p.idproducto == idprod).ToList();
            var listaalmacenamiento = _contextDB.Almacenamientos.ToList();

            foreach (var i in listproductoInter)
            {
                if (valor == i.idcolor)
                {
                    listaalmacenamiento.RemoveAt(i.idalmacenamiento-1);
                }
            }


            var items = listaalmacenamiento.Select(a => new ComboBoxItem
            {
                Value = a.ID,
                Text = a.Almacenamineto
            }).ToList();

            return items;
        }
    }
    public class ComboBoxItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
