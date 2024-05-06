using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;

namespace PeroShopWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly PerashopDB _contextDB;
        public static string MyProperty { get; set; }

        public AdminController(PerashopDB perashopDB)
        {
            _contextDB = perashopDB;
        }

        public void Cookies()
        {
            var miCookie = HttpContext.Request.Cookies["MiCookie"];

            if (miCookie != null)
            {
                List<Usuario> listaUsuarios = _contextDB.Usuario.ToList();
                foreach (var user in listaUsuarios)
                {
                    if (miCookie == user.Correo)
                    {
                        ViewBag.Nombre = user.Nombre;
                        ViewBag.Nivel = user.TipoUsuario;
                        ViewBag.FotoPerfil = user.DireccionImagen;
                    }
                }
            }
        }

        public IActionResult Bienvenida()
        {
            Cookies();
            return View();
        }

        [HttpGet]
        public IActionResult Usuarios()
        {
            List<Usuario> listausuarios = _contextDB.Usuario.ToList();
            Cookies();
            return View(listausuarios);
        }

        [HttpGet]
        public IActionResult EditarUsuarios(int id)
        {
            var usuario = _contextDB.Usuario.FirstOrDefault(p => p.ID == id);
            Cookies();
            return View(usuario);
        }

        [HttpGet]
        public IActionResult EliminarUsuarios(int id)
        {
            var usuario = _contextDB.Usuario.FirstOrDefault(p => p.ID == id);
            Cookies();
            return View(usuario);
        }

        [HttpGet]
        public IActionResult Ventas()
        {
            List<CarritoVenta> listaventas = _contextDB.CarritoVenta.ToList();
            Cookies();
            return View(listaventas);
        }
        [HttpGet]
        public IActionResult DetalleVenta(int id, int idord, string envio)
        {
            var usrtemp = _contextDB.Usuario.FirstOrDefault(d => d.ID == id);

            ViewBag.Direccion = _contextDB.Direccion.FirstOrDefault(d => d.iD == usrtemp.iddireccion);
            ViewBag.Productos = _contextDB.Producto.ToList();
            ViewBag.CarrVen = _contextDB.CarritoVenta.ToList();
            ViewBag.inter = _contextDB.ProductoInter.ToList();
            ViewBag.Usuario = usrtemp;
            ViewBag.IdOrden = idord;
            ViewBag.Envio = envio;
            MyProperty = envio;

            Cookies();

            return View();
        }

        public IActionResult CambioEstado(int Id)
        {
            var orden = _contextDB.CarritoVenta.FirstOrDefault(c => c.Envio == MyProperty);
            if (Id == 1)
            {
                orden.Envio = "Tu paquete esta listo para ser enviado";
            }
            else if (Id == 2)
            {
                orden.Envio = "Tu paquete esta en camino a tu domicilio";
            }
            else if (Id == 3)
            {
                orden.Envio = "Tu paquete a sido entregado";
            }
            _contextDB.CarritoVenta.Update(orden);
            _contextDB.SaveChanges();

            return RedirectToAction("Ventas");
        }

        [HttpGet]
        public IActionResult VentasTerminada()
        {
            List<CarritoVenta> inter = _contextDB.CarritoVenta.ToList();
            Cookies();
            return View(inter);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditarUsuarios(IFormFile Imagen, Usuario usuario, string id, string Contrasena1, string Contrasena2)
        //{
        //    var usuarios = new UsuarioModel(_contextDB);
        //    if (Imagen != null)
        //    {
        //        string nombreImagen = usuario.Correo + Imagen.FileName;
        //        await this.helperUpload.UploadFilesAsync(Imagen, nombreImagen, Fold.Images);
        //        usuario.DireccionImagen = "../Images/Usuarios/" + nombreImagen;
        //    }
        //    else
        //    {
        //        usuario.DireccionImagePerfil = FotoPerfil;
        //    }
        //    if (Contrasena1 != null)
        //    {
        //        usuario.Contrasena = Contrasena1;
        //    }

        //    usuarios.EditarUsuario(usuario);
        //    Cookies();
        //    return RedirectToAction(nameof(Usuarios));
        //}
    }
}
