using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;

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
        public IActionResult Index(string valor) 
        {
            ViewBag.Tipo = valor;
            List<Producto> listaProductos = _ContextoDB.Producto.ToList();
            return View(listaProductos);
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
    }
}
