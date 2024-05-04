using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;
using System.Drawing;

namespace PeroShopWeb.Controllers
{
    public class ProductosController : Controller
    {
        public readonly PerashopDB _ContextoDB;

        public ProductosController(PerashopDB perashopDB)
        {
            _ContextoDB = perashopDB;
        }

        [HttpGet]
        public IActionResult DetallesProductos(int valor) 
        {
            ViewBag.ID = valor;
            var producto = _ContextoDB.Producto.First(p => p.ID == valor);
            return View(producto);
        }

        [HttpPost]
        public IActionResult DetallesProductos(Producto producto)
        {
            return View();
        }
        [HttpGet]
        public IActionResult ListaProductos(string valor)
        {
            ViewBag.Tipo = valor;
            List<Producto> listaProductos = _ContextoDB.Producto.ToList();
            return View(listaProductos); ;
        }
    }
}
