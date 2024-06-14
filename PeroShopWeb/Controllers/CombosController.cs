using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeroShopWeb.Models;
using System.Collections.Generic;
using System.Linq;

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

            var almacenamientosRelacionados = new HashSet<int>();

            foreach (var i in listproductoInter)
            {
                if (valor == i.idcolor)
                {
                    almacenamientosRelacionados.Add(i.idalmacenamiento);
                }
            }

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
                    imageUrl = i.RutaImagen;
                }
            }

            listaalmacenamiento = listaalmacenamiento.Where(a => almacenamientosRelacionados.Contains(a.ID)).ToList();

            var items = listaalmacenamiento.Select(a => new ComboBoxItem
            {
                Value = a.ID,
                Text = a.Almacenamineto,
                MaxQuantity = listproductoInter.FirstOrDefault(p => p.idcolor == valor && p.idalmacenamiento == a.ID)?.Stock ?? 0,
                ProductInterId = listproductoInter.FirstOrDefault(p => p.idcolor == valor && p.idalmacenamiento == a.ID)?.ID ?? 0,
                Price = listproductoInter.FirstOrDefault(p => p.idcolor == valor && p.idalmacenamiento == a.ID)?.PrecioVenta ?? 0  // Añadir el precio
            }).ToList();

            var response = new DetalleCombosResponse
            {
                Items = items,
                ImageUrl = imageUrl
            };

            return response;
        }

        [HttpGet("GetMaxQuantity")]
        public IActionResult GetMaxQuantity(int productInterId)
        {
            var productInter = _contextDB.ProductoInter.FirstOrDefault(p => p.ID == productInterId);
            if (productInter == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(new { maxQuantity = productInter.Stock });
        }

        [HttpGet("CambioCantidad")]
        public IActionResult CambioCantidad(int idprodInter, int valor, int carritoId)
        {
            var carrito = _contextDB.CarritoVenta.FirstOrDefault(i => i.ID == carritoId && i.idproductointer == idprodInter);
            var inter = _contextDB.ProductoInter.FirstOrDefault(i => i.ID == idprodInter);

            carrito.Cantidad = valor;
            carrito.Total = carrito.Cantidad * carrito.Total;

            _contextDB.CarritoVenta.Update(carrito);
            _contextDB.SaveChanges();

            var nuevototal = carrito.Cantidad * inter.PrecioVenta;

            var response = new
            {
                total = nuevototal,
            };

            return Ok(response);
        }
    }

    public class ComboBoxItem
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public int MaxQuantity { get; set; }
        public int ProductInterId { get; set; }
        public decimal Price { get; set; }  // Añadir el precio
    }

    public class DetalleCombosResponse
    {
        public List<ComboBoxItem> Items { get; set; }
        public string ImageUrl { get; set; }
    }
}
