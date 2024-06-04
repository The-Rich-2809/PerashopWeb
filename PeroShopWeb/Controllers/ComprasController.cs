using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;

namespace PeroShopWeb.Controllers
{
    public class ComprasController : Controller
    {
        public readonly PerashopDB _ContextoDB;
        public int IdUser { get; set; }

        public ComprasController(PerashopDB perashop)
        {
            _ContextoDB = perashop;
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
        }

        public IActionResult Registro(double total)
        {
            ViewBag.Total = total;
            Cookies();
            List<CarritoVenta> carrito = _ContextoDB.CarritoVenta.ToList();
            List<ProductoColorAlamacenamientoInter> listaproint = _ContextoDB.ProductoInter.ToList();
            var listausuarios = _ContextoDB.Usuario.ToList();

            ViewBag.listaususuarios = listausuarios;

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
