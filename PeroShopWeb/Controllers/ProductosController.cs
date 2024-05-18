using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;
using System.Drawing;
using System.Text.Json;
using Newtonsoft.Json;

namespace PeroShopWeb.Controllers
{
    public class ProductosController : Controller
    {
        public readonly PerashopDB _ContextoDB;

        public ProductosController(PerashopDB perashopDB)
        {
            _ContextoDB = perashopDB;
        }

        public int IdProducto { get; set; }
        public int IdUser { get; set; }
        public void Cookies()
        {
            var miCookie = HttpContext.Request.Cookies["MiCookie"];

            if (miCookie != null)
            {
                List<Usuario> listaUsuarios = _ContextoDB.Usuario.ToList();
                foreach (var user in listaUsuarios)
                {
                    if (miCookie == user.Correo)
                    {
                        IdUser = user.ID;
                        ViewBag.Nombre = user.Nombre;
                        ViewBag.Nivel = user.TipoUsuario;
                        ViewBag.FotoPerfil = user.DireccionImagen;
                    }
                }
            }
        }

        [HttpGet]
        public IActionResult DetallesProductos(int valor)
        {
            ViewBag.ID = valor;
            IdProducto = valor;
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
        public IActionResult DetallesProductos(CarritoVenta carritoVenta)
        {
            Cookies();
            carritoVenta.idusuario = IdUser;
            carritoVenta.Cambio = 1;
            carritoVenta.Envio = "";

            var insertarventas = new CarritoVenta[]
            {
                carritoVenta,
            };

            foreach (var u in insertarventas)
                _ContextoDB.CarritoVenta.Add(u);

            _ContextoDB.SaveChanges();
            return RedirectToAction("Carrito");
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
        [HttpGet]
        public IActionResult Carrito()
        {
            Cookies();
            List<CarritoVenta> carrito = _ContextoDB.CarritoVenta.ToList();
            List<ProductoColorAlamacenamientoInter> listaproint = _ContextoDB.ProductoInter.ToList();
            ViewBag.IdUser = IdUser;

            var viewmodel = new ProductosViewModel
            {
                CarritoVentas = carrito,
                ProductosInter = listaproint
            };

            return View(viewmodel);
        }
    }
}
