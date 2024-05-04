using PeroShopWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace PeroShopWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PerashopDB _contextDB;

        public HomeController(ILogger<HomeController> logger, PerashopDB contextDB)
        {
            _logger = logger;
            _contextDB = contextDB;
        }

        public IActionResult Index()
        {
            Initialize();

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

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario Usuario)
        {
            UsuarioModel login = new UsuarioModel(_contextDB);

            login.Correo = Usuario.Correo;
            login.Contrasena = Usuario.Contrasena;

            if (login.Login())
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(365);
                options.IsEssential = true;
                options.Path = "/";
                HttpContext.Response.Cookies.Append("MiCookie", Usuario.Correo, options);

                return RedirectToAction("Index");
            }

            return View(Usuario);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario Usuario, string Contrasena2)
        {
            UsuarioModel register = new UsuarioModel(_contextDB);

            register.Correo = Usuario.Correo;
            register.Contrasena = Usuario.Contrasena;
            register.Contrasena2 = Contrasena2;
            UsuarioModel.Nombre = Usuario.Nombre;

            if (register.Register())
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(365);
                options.IsEssential = true;
                options.Path = "/";
                HttpContext.Response.Cookies.Append("MiCookie", Usuario.Correo, options);

                if (register.Login())
                    return RedirectToAction("Index");
            }
            else
                ViewBag.Mensaje = UsuarioModel.Mensaje;

            return View(Usuario);
        }

        [HttpGet]
        public IActionResult CerrarSesion()
        {
            HttpContext.Response.Cookies.Delete("MiCookie");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void Initialize()
        {
            _contextDB.Database.EnsureCreated();

            if (_contextDB.Usuario.Any())
            {
                return;
            }

            var insertardireccion = new Direccion[]
                {
                    new Direccion() {Nombre = "Prueba", ApellidoPaterno = "Prueba", ApellidoMaterno = "Prueba", Calle = "Prueba", Colonia = "Prueba", CodigoPostal = "00000", Delegacion = "Prueba"}
                };

            var insertarusuarios = new Usuario[]
                {
                    new Usuario() {Correo = "aserranoacosta841@gmail.com", Contrasena = "1234", TipoUsuario = "Admin", iddireccion = 1, DireccionImagen = "../Images/Usuarios/Alejandro.jpg", Nombre = "Alejandro"}
                };
            var insertarProveedor = new Proveedores[]
                {
                    new Proveedores() { Activo = 1, PersonaContacto = "Prueba", Telefono = "55555555", Correo = "hola@gmail.com", iddireccion = 1 }
                };
            var insertarProductos = new Producto[]
                {
                    new Producto() { Activo = 1, Nombre = "Aipods1", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/Airpods1.jpeg", idproveedor = 1,Categoria="Audifonos"},
                    new Producto() { Activo = 1, Nombre = "Aipods2", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/Airpods2.jpeg", idproveedor = 1,Categoria="Audifonos"},
                    new Producto() { Activo = 1, Nombre = "Aipods3", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/Airpods3.jpg", idproveedor = 1,Categoria="Audifonos"},
                    new Producto() { Activo = 1, Nombre = "PearPhone 12", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/iphone12.jpg", idproveedor = 1,Categoria="Telefonos"},
                    new Producto() { Activo = 1, Nombre = "PearPhone 13", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/13.jpg", idproveedor = 1,Categoria="Telefonos"},
                    new Producto() { Activo = 1, Nombre = "PearPhone 14", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/14.jpg", idproveedor = 1,Categoria="Telefonos"},
                    new Producto() { Activo = 1, Nombre = "PearPhone 15", Caracteristicas = "55555555", PrecioCompra = 3, PrecioVenta = 1000, Existencia = 30, RutaImagen = "../Images/Products/15.jpg", idproveedor = 1,Categoria="Telefonos"}
                };

            foreach (var u in insertardireccion)
                _contextDB.Direccion.Add(u);

            foreach (var u in insertarusuarios)
                _contextDB.Usuario.Add(u);

            foreach (var u in insertarProveedor)
                _contextDB.Proveedores.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarProductos)
                _contextDB.Producto.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
