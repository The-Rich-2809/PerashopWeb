using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Helpers;
using PeroShopWeb.Models;
using System.Runtime.CompilerServices;

namespace PeroShopWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly PerashopDB _contextDB;
        public int ID { get; set; }
        public static int IDinter { get; set; }
        public static int idorden { get; set; }
        public static int numeroart { get; set; }
        public static int articulocalif { get; set; }

        public UsuarioController(PerashopDB perashopDB)
        {
            _contextDB = perashopDB;
        }

        public void Cookies()
        {
            var miCookie = HttpContext.Request.Cookies["MiCookie"];

            int IdUser = 0;

            if (miCookie != null)
            {
                List<Usuario> listaUsuarios = _contextDB.Usuario.ToList();
                foreach (var user in listaUsuarios)
                {
                    if (miCookie == user.Correo)
                    {
                        IdUser = user.ID;
                        ID = user.ID;
                        ViewBag.Nombre = user.Nombre;
                        ViewBag.Nivel = user.TipoUsuario;
                        ViewBag.FotoPerfil = user.DireccionImagen;
                    }
                }
            }

            int conteo = _contextDB.CarritoVenta
            .Where(c => c.Cambio == 1 && c.idusuario == IdUser)
            .Count();

            ViewBag.conteo = conteo;
        }

        public IActionResult Index()
        {
            Cookies();
            return View();
        }

        public IActionResult Compras()
        {
            Cookies();
            List<CarritoVenta> listaventas = _contextDB.CarritoVenta.Where(c => c.idusuario == ID && c.Cambio == 2 && c.Envio != "Tu paquete a sido entregado").ToList();
            return View(listaventas);
        }

        [HttpGet]
        public IActionResult DetalleVenta(int id, int idord, string envio)
        {
            var usrtemp = _contextDB.Usuario.FirstOrDefault(d => d.ID == id);
            ViewBag.Productos = _contextDB.Producto.ToList();
            ViewBag.CarrVen = _contextDB.CarritoVenta.ToList();
            ViewBag.inter = _contextDB.ProductoInter.ToList();
            ViewBag.Usuario = usrtemp;
            ViewBag.IdOrden = idord;
            ViewBag.Envio = envio;

            Cookies();

            return View();
        }

        public IActionResult RateProductos()
        {
            Cookies();
            List<CarritoVenta> listaventas = _contextDB.CarritoVenta.Where(c => c.idusuario == ID && c.Cambio == 2 && c.Envio == "Tu paquete a sido entregado").ToList();
            return View(listaventas);
        }

        [HttpGet]
        public IActionResult RateDetalle(int id, int idord, string envio, int numarticulos)
        {
            numeroart = numarticulos;
            Cookies();
            var usrtemp = _contextDB.Usuario.FirstOrDefault(d => d.ID == id);
            ViewBag.Productos = _contextDB.Producto.ToList();
            ViewBag.CarrVen = _contextDB.CarritoVenta.Where(c => c.idusuario == ID && c.Cambio == 2 && c.Envio == "Tu paquete a sido entregado").ToList();
            ViewBag.inter = _contextDB.ProductoInter.ToList();
            ViewBag.colores = _contextDB.Colores.ToList();
            ViewBag.alma = _contextDB.Almacenamientos.ToList();
            ViewBag.Usuario = usrtemp;
            ViewBag.IdOrden = idord;
            idorden = idord;
            ViewBag.Envio = envio;

            return View();
        }

        [HttpGet]
        public IActionResult RateProducto(int id)
        {
            ViewBag.Productos = _contextDB.Producto.ToList();
            var pinterproduc = _contextDB.ProductoInter.Where(i => i.ID == id).ToList();
            var interp = _contextDB.ProductoInter.FirstOrDefault(i => i.ID == id);
            ViewBag.inter = pinterproduc;
            IDinter = interp.ID;
            ViewBag.colores = _contextDB.Colores.ToList();
            ViewBag.alma = _contextDB.Almacenamientos.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult RateProducto(ProductoColorAlamacenamientoInter productoColorAlamacenamientoInter)
        {
            var inter = _contextDB.ProductoInter.FirstOrDefault(i => i.ID == IDinter);

            inter.Calificacion += productoColorAlamacenamientoInter.Calificacion;

            _contextDB.ProductoInter.Update(inter);
            _contextDB.SaveChanges();

            articulocalif += 1;

            if (articulocalif == numeroart)
            {
                List<CarritoVenta> listaventas = _contextDB.CarritoVenta.Where(c => c.IDOrden == idorden).ToList();

                foreach (var c in listaventas)
                {
                    c.Envio = "Tu paquete a sido calificado";

                    _contextDB.CarritoVenta.Update(c);
                    _contextDB.SaveChanges();
                }

                articulocalif = 0;
                numeroart = 0;
            }

            return RedirectToAction("RateProductos");
        }
    }
}
