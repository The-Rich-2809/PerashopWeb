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
        public IActionResult Index(string tipo) 
        {
            ViewBag.Tipo = tipo;    
            List<Producto> listaProductos = _ContextoDB.Producto.ToList();
            return View(listaProductos);
        }

        [HttpGet]
        public IActionResult DetallesProductos(int ID) 
        {
            return View();
        }

        [HttpPost]
        public IActionResult DetallesProductos(Producto producto)
        {
            return View();
        }
    }
}
