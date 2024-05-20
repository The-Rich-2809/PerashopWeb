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

            // Crear un conjunto para almacenar los IDs de almacenamiento relacionados con el color especificado
            var almacenamientosRelacionados = new HashSet<int>();

            foreach (var i in listproductoInter)
            {
                if (valor == i.idcolor)
                {
                    almacenamientosRelacionados.Add(i.idalmacenamiento);
                }
            }

            // Mantener solo los almacenamientos que no están en el conjunto almacenamientosRelacionados
            listaalmacenamiento = listaalmacenamiento.Where(a => !almacenamientosRelacionados.Contains(a.ID)).ToList();

            var items = listaalmacenamiento.Select(a => new ComboBoxItem
            {
                Value = a.ID,
                Text = a.Almacenamineto
            }).ToList();

            return items;
        }

        [HttpGet("DetalleCombos")]
        public ActionResult<DetalleCombosResponse> DetalleCombos(int valor, int idprod)
        {
            var listproductoInter = _contextDB.ProductoInter.Where(p => p.idproducto == idprod).ToList();
            var listaalmacenamiento = _contextDB.Almacenamientos.ToList();

            var almacenamientosRelacionados = new HashSet<int>();
            string imageUrl = null;

            foreach (var i in listproductoInter)
            {
                if (valor == i.idcolor)
                {
                    almacenamientosRelacionados.Add(i.idalmacenamiento);
                    imageUrl = i.RutaImagen; // Asigna la URL de la imagen del color seleccionado
                }
            }

            listaalmacenamiento = listaalmacenamiento.Where(a => almacenamientosRelacionados.Contains(a.ID)).ToList();

            var items = listaalmacenamiento.Select(a => new ComboBoxItem
            {
                Value = a.ID,
                Text = a.Almacenamineto
            }).ToList();

            var response = new DetalleCombosResponse
            {
                Items = items,
                ImageUrl = imageUrl
            };

            return response;
        }

    }

    public class DetalleCombosResponse
    {
        public List<ComboBoxItem> Items { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ComboBoxItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}
