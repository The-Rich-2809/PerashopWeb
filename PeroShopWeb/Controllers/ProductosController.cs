using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PeroShopWeb.Models;
using System.Drawing;

namespace PeroShopWeb.Controllers
{
    public class ProductosController : Controller
    {
        public readonly PerashopDB _ContextoDB;

        public int IdProducto { get; set; }

        public ProductosController(PerashopDB perashopDB)
        {
            _ContextoDB = perashopDB;
        }

        [HttpGet]
        public IActionResult DetallesProductos(int valor) 
        {
            ViewBag.ID = valor;
            IdProducto = valor;
            ViewBag.idprod = IdProducto;
            var producto = _ContextoDB.Producto.First(p => p.ID == valor);
            ViewBag.NombreProducto = producto.Nombre;
            ViewBag.Categoria = producto.Categoria;

            List<Producto> listaProductos = _ContextoDB.Producto.ToList();
            List<ProductoColor> productoColors = _ContextoDB.Colores.ToList();
            List<ProductoAlmacenamiento> productoAlmacenamientos = _ContextoDB.Almacenamientos.ToList();
            List<ProductoColorAlamacenamientoInter> listaproint = _ContextoDB.ProductoInter.ToList();
            var viewmodel = new ProductosViewModel
            {
                Productos = listaProductos,
                ProductosInter = listaproint,
                ProductoColors = productoColors,
                productoAlmacenamientos = productoAlmacenamientos
            };

            var json = JsonConvert.SerializeObject(viewmodel);

            return View(viewmodel);
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
            List<ProductoColorAlamacenamientoInter> listaproint = _ContextoDB.ProductoInter.ToList();

            var viewmodel = new ProductosViewModel
            {
                Productos = listaProductos,
                ProductosInter = listaproint
            };

            return View(viewmodel); 
        }
    }
}
