    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PeroShopWeb.Models;
using System.Drawing;

namespace PeroShopWeb.Controllers
{
    public class ProductosController : Controller
    {
        public readonly PerashopDB _ContextoDB;

        public int IdProducto { get; set; }
        public int IdUser { get; set; }

        public ProductosController(PerashopDB perashopDB)
        {
            _ContextoDB = perashopDB;
        }

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

            int conteo = _ContextoDB.CarritoVenta
            .Where(c => c.Cambio == 1 && c.idusuario == IdUser)
            .Count();

            ViewBag.conteo = conteo;
        }

        [HttpGet]
        public IActionResult DetallesProductos(int valor)
        {
            Cookies();
            ViewBag.ID = valor;
            IdProducto = valor;
            ViewBag.idprod = IdProducto;
            var producto = _ContextoDB.Producto.First(p => p.ID == valor);
            ViewBag.NombreProducto = producto.Nombre;
            ViewBag.Categoria = producto.Categoria;

            var averageRating = _ContextoDB.ProductoInter
                                    .Where(r => r.ID == valor)
                                    .Average(r => r.Calificacion);

            ViewBag.AverageRating = averageRating;

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
            carritoVenta.IDPedido = "";

            int t = 0;

            List<CarritoVenta> listacarritoventa = _ContextoDB.CarritoVenta.ToList();

            foreach (var item in listacarritoventa)
            {
                if (item.idusuario == IdUser)
                {
                    if (item.idproductointer == carritoVenta.idproductointer)
                    {
                        if (item.Cambio == 1)
                        {
                            carritoVenta.Cantidad += item.Cantidad;
                            _ContextoDB.CarritoVenta.Remove(item);
                            _ContextoDB.SaveChanges();
                        }
                    }
                    t++;
                }
            }
            if (listacarritoventa.Count == t)
            {
                carritoVenta.Total = carritoVenta.Total * carritoVenta.Cantidad;
                carritoVenta.idusuario = IdUser;
                _ContextoDB.CarritoVenta.Add(carritoVenta);
                _ContextoDB.SaveChanges();
            }
            else
            {
                carritoVenta.Total = carritoVenta.Total * carritoVenta.Cantidad;
                carritoVenta.idusuario = IdUser;
                _ContextoDB.CarritoVenta.Add(carritoVenta);
                _ContextoDB.SaveChanges();
            }

            return RedirectToAction("Carrito");
        }
        [HttpGet]
        public IActionResult ListaProductos(string valor)
        {
            Cookies();
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

        [HttpGet]
        public IActionResult EliminarCarrito(int idinter)
        {
            var prodcarrito = _ContextoDB.CarritoVenta.FirstOrDefault(p => p.ID == idinter);
            _ContextoDB.CarritoVenta.Remove(prodcarrito);
            _ContextoDB.SaveChanges();
            return RedirectToAction(nameof(Carrito));
        }
    }
}
