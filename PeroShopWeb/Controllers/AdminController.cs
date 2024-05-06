using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;
using System.Linq;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using PeroShopWeb.Providers;
using PeroShopWeb.Helpers;

namespace PeroShopWeb.Controllers
{
    public class AdminController : Controller
    {
        private HelperUploadFiles helperUpload;
        private readonly PerashopDB _contextDB;

        public AdminController(HelperUploadFiles helperUpload, PerashopDB perashopDB)
        {
            _contextDB = perashopDB;
            this.helperUpload = helperUpload;
        }
        public static int IdProducto { get; set; }
        public static string CategoriaProducto { get; set; }

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
        [HttpGet]
        public IActionResult Productos()
        {
            List<Producto> listproductos = _contextDB.Producto.ToList();
            Cookies();
            return View(listproductos);
        }
        [HttpGet]
        public IActionResult NuevoProducto()
        {
            List<Proveedores> proveedores = _contextDB.Proveedores.ToList();
            return View(proveedores);
        }
        [HttpPost]
        public IActionResult NuevoProducto(Producto producto)
        {
            Cookies();
            ProductoModel productoModel = new ProductoModel(_contextDB);
            if (productoModel.NuevoProducto(producto))
                return RedirectToAction("Productos");
            else
                return View();

        }
        [HttpGet]
        public IActionResult EditarProducto(int Id, string Nombre, string Categoria )
        {
            Cookies();
            ViewBag.Id = Id;
            IdProducto = Id;
            ViewBag.NombreProducto = Nombre;
            CategoriaProducto = Categoria;
            ViewBag.Categoria = Categoria;
            return View();
        }
        [HttpGet]
        public IActionResult AgregaCaracteristicas()
        {
            List<ProductoColorAlamacenamientoInter> listproductoInter = _contextDB.ProductoInter.ToList();
            List<ProductoColor> listproductoColor = _contextDB.Colores.ToList();
            List<ProductoAlmacenamiento> listproductoAlmacenamiento = _contextDB.Almacenamientos.ToList();

            var viewmodel = new ProductosViewModel
            {
                ProductosInter = listproductoInter,
                ProductoColors = listproductoColor,
                productoAlmacenamientos = listproductoAlmacenamiento
            };
            Cookies();
            ViewBag.Categoria = CategoriaProducto;
            return View(viewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> AgregaCaracteristicas(ProductoColorAlamacenamientoInter productointer, IFormFile[] Imagen)
        {
            ProductoModel productoModel = new ProductoModel(_contextDB);
            productointer.idproducto = IdProducto;
            if(CategoriaProducto != "Telefonos")
            {
                productointer.idalmacenamiento = 1;
            }

            string nombreImagen = + productointer.idproducto + "_" + productointer.idcolor + "_" + productointer.idalmacenamiento + "_" + Imagen[0].FileName;
            await this.helperUpload.UploadFilesAsync(Imagen[0], nombreImagen, Folders.Productos);

            productointer.RutaImagen = "../Images/Products/" + nombreImagen;
            productoModel.NuevasCaracteristicas(productointer);
            return RedirectToAction("Productos");
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
