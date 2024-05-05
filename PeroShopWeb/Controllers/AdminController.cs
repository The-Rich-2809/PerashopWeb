using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;

namespace PeroShopWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly PerashopDB _contextDB;

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
        public IActionResult EditarUsuarios(string id)
        {
            List<Usuario> listausuarios = _contextDB.Usuario.ToList();
            Cookies();
            return View(listausuarios);
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
