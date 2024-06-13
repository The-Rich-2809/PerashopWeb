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

            int IdUser = 0;

            var miCookie = HttpContext.Request.Cookies["MiCookie"];

            if (miCookie != null)
            {
                List<Usuario> listaUsuarios = _contextDB.Usuario.ToList();
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
            int conteo = _contextDB.CarritoVenta
            .Where(c => c.Cambio == 1 && c.idusuario == IdUser)
            .Count();

            ViewBag.conteo = conteo;


            var inter = _contextDB.ProductoInter.ToList();
            var producto = _contextDB.Producto.OrderByDescending(c => c.Fecha).ToList();

            var model = new ProductosViewModel
            {
                ProductosInter = inter,
                Productos = producto
            };

            return View(model);
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
            register.Telefono = Usuario.NumeroTelefono;
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

            var insertarColor = new ProductoColor[]
                {
                    new ProductoColor() {Color = "Rosa"},
                    new ProductoColor() {Color = "Blanco"},
                    new ProductoColor() {Color = "Negro"},
                    new ProductoColor() {Color = "Azul"}
                };
            var insertarAlmacenamineto = new ProductoAlmacenamiento[]
                {
                    new ProductoAlmacenamiento() {Almacenamineto = "Almacenamiento..."},
                    new ProductoAlmacenamiento() {Almacenamineto = "64"},
                    new ProductoAlmacenamiento() {Almacenamineto = "128"},
                    new ProductoAlmacenamiento() {Almacenamineto = "256"},
                    new ProductoAlmacenamiento() {Almacenamineto = "512"},
                    new ProductoAlmacenamiento() {Almacenamineto = "1TB"}
                };

            var insertardireccion = new Direccion[]
                {
                    new Direccion() {Nombre = "Prueba", ApellidoPaterno = "Prueba", ApellidoMaterno = "Prueba", Calle = "Prueba", Colonia = "Prueba", CodigoPostal = "00000", Delegacion = "Prueba"}
                };

            var insertarusuarios = new Usuario[]
                {
                    new Usuario() {Correo = "aserranoacosta841@gmail.com", Contrasena = "1234", TipoUsuario = "Admin", NumeroTelefono = "5555555555", DireccionImagen = "../Images/Usuarios/Alejandro.jpg", Nombre = "Alejandro"},
                    new Usuario() {Correo = "ricardo_138@outlook.com", Contrasena = "1234", TipoUsuario = "Admin", NumeroTelefono = "5555555555", DireccionImagen = "../Images/Usuarios/Rich.jpg", Nombre = "Rich"},
                   new Usuario() {Correo = "angelgabriel010306@gmail.com", Contrasena = "1234", TipoUsuario = "Admin", NumeroTelefono = "5555555555", DireccionImagen = "../Images/Usuarios/Alejandro.jpg", Nombre = "Angel"},
                    new Usuario() {Correo = "a@gmail.com", Contrasena = "1234", TipoUsuario = "Admin", NumeroTelefono = "5555555555", DireccionImagen = "../Images/Usuarios/Usuario.jpg", Nombre = "A"}
                };
            var insertarProveedor = new Proveedores[]
                {
                    new Proveedores() { Activo = 1, NombreProveedor="Hola", PersonaContacto = "Prueba", Telefono = "55555555", Correo = "hola@gmail.com", iddireccion = 1 }
                };
            var insertarProductos = new Producto[]
                {
                    new Producto() { Activo = 1, Nombre = "Aipods1", idproveedor = 1, Categoria="Audifonos", Fecha = new DateTime(2024, 6, 1, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "Aipods2", idproveedor = 1, Categoria = "Audifonos", Fecha = new DateTime(2024, 6, 1, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "Aipods3", idproveedor = 1, Categoria = "Audifonos", Fecha = new DateTime(2024, 6, 4, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "PearPhone 12", idproveedor = 1, Categoria="Telefonos", Fecha = new DateTime(2024, 6, 1, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "PearPhone 13", idproveedor = 1, Categoria = "Telefonos", Fecha = new DateTime(2024, 6, 1, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "PearPhone 14", idproveedor = 1, Categoria = "Telefonos", Fecha = new DateTime(2024, 6, 4, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "PearPhone 15", idproveedor = 1, Categoria = "Telefonos", Fecha = new DateTime(2024, 6, 1, 15, 30, 0)},
                    new Producto() { Activo = 1, Nombre = "Funda MagSafe", idproveedor = 1, Categoria = "Fundas", Fecha = new DateTime(2024, 6, 4, 15, 30, 0)}
                };
            var insertarProductosInter = new ProductoColorAlamacenamientoInter[]
                {
                    new ProductoColorAlamacenamientoInter() {idproducto = 1, idcolor = 2, idalmacenamiento = 1, Stock = 15, RutaImagen = "../Images/Products/Airpods1.jpeg", Caracteristicas = "Sin cables y sin esfuerzo.\r\nComo por arte de magia.", PrecioCompra = 200, PrecioVenta = 3500},
                    new ProductoColorAlamacenamientoInter() {idproducto = 2, idcolor = 2, idalmacenamiento = 1, Stock = 15, RutaImagen = "../Images/Products/Airpods2.jpeg", Caracteristicas = "Sin cables y sin esfuerzo.\r\nComo por arte de magia.", PrecioCompra = 200, PrecioVenta = 4000},
                    new ProductoColorAlamacenamientoInter() {idproducto = 3, idcolor = 2, idalmacenamiento = 1, Stock = 15, RutaImagen = "../Images/Products/AirpodsPro.jpg", Caracteristicas = "Sin cables y sin esfuerzo.\r\nComo por arte de magia.", PrecioCompra = 200, PrecioVenta = 5000},
                    new ProductoColorAlamacenamientoInter() {idproducto = 4, idcolor = 1, idalmacenamiento = 2, Stock = 25, RutaImagen = "../Images/Products/iphone12.jpg", Caracteristicas = "PANTALLA\r\n\r\nOLED Retina\r\n2.532 x 1.170 píxeles, Super Retina XDR, 19.5:9\r\n460ppp\r\nTrue-tone\r\n\r\nPROCESADOR\r\n\r\nApple A14 Bionic, 5nm\r\nNPU Neural Engine de 4ª gen\r\n\r\nVERSIONES\r\n\r\n64 / 128 / 256 GB\r\n\r\nDIMENSIONES Y PESO\r\n\r\n146,7 mm x 71,5 mm x 7,4mm\r\n162g\r\n\r\nSOFTWARE\r\n\r\niOS 14\r\n\r\nCÁMARAS TRASERAS\r\n\r\nPrincipal: 12MP, f/1.6, OIS, QuadLED flash\r\nSecundaria gran angular: 12MP, f/2.4\r\nVídeo: 4K Dolby Vision, 1080p/240fps, HDR\r\n\r\nCÁMARA FRONTAL\r\n\r\n12MP, f/2.2, TOF 3D, slow-motion\r\n\r\nBATERÍA\r\n\r\nCarga rápida 18W e inalámbrica MagSafe 15W\r\n\r\nOTROS\r\n\r\nWiFi 6, 5G, BT 5.0, NFC, GPS, dualSIM, eSIM, altavoces estéreo Dolby Atmos, reconocimiento facial, resistencia al agua IP68.", PrecioCompra = 600, PrecioVenta = 7500},
                    new ProductoColorAlamacenamientoInter() {idproducto = 5, idcolor = 2, idalmacenamiento = 2, Stock = 25, RutaImagen = "../Images/Products/13.jpg", Caracteristicas = "PANTALLA\r\n\r\nOLED Retina\r\n2.532 x 1.170 píxeles, Super Retina XDR, 19.5:9\r\n460ppp\r\nTrue-tone\r\n\r\nPROCESADOR\r\n\r\nApple A14 Bionic, 5nm\r\nNPU Neural Engine de 4ª gen\r\n\r\nVERSIONES\r\n\r\n64 / 128 / 256 GB\r\n\r\nDIMENSIONES Y PESO\r\n\r\n146,7 mm x 71,5 mm x 7,4mm\r\n162g\r\n\r\nSOFTWARE\r\n\r\niOS 14\r\n\r\nCÁMARAS TRASERAS\r\n\r\nPrincipal: 12MP, f/1.6, OIS, QuadLED flash\r\nSecundaria gran angular: 12MP, f/2.4\r\nVídeo: 4K Dolby Vision, 1080p/240fps, HDR\r\n\r\nCÁMARA FRONTAL\r\n\r\n12MP, f/2.2, TOF 3D, slow-motion\r\n\r\nBATERÍA\r\n\r\nCarga rápida 18W e inalámbrica MagSafe 15W\r\n\r\nOTROS\r\n\r\nWiFi 6, 5G, BT 5.0, NFC, GPS, dualSIM, eSIM, altavoces estéreo Dolby Atmos, reconocimiento facial, resistencia al agua IP68.", PrecioCompra = 600, PrecioVenta = 7500},
                    new ProductoColorAlamacenamientoInter() {idproducto = 6, idcolor = 3, idalmacenamiento = 5, Stock = 25, RutaImagen = "../Images/Products/14.jpg", Caracteristicas = "PANTALLA\r\n\r\nOLED Retina\r\n2.532 x 1.170 píxeles, Super Retina XDR, 19.5:9\r\n460ppp\r\nTrue-tone\r\n\r\nPROCESADOR\r\n\r\nApple A14 Bionic, 5nm\r\nNPU Neural Engine de 4ª gen\r\n\r\nVERSIONES\r\n\r\n64 / 128 / 256 GB\r\n\r\nDIMENSIONES Y PESO\r\n\r\n146,7 mm x 71,5 mm x 7,4mm\r\n162g\r\n\r\nSOFTWARE\r\n\r\niOS 14\r\n\r\nCÁMARAS TRASERAS\r\n\r\nPrincipal: 12MP, f/1.6, OIS, QuadLED flash\r\nSecundaria gran angular: 12MP, f/2.4\r\nVídeo: 4K Dolby Vision, 1080p/240fps, HDR\r\n\r\nCÁMARA FRONTAL\r\n\r\n12MP, f/2.2, TOF 3D, slow-motion\r\n\r\nBATERÍA\r\n\r\nCarga rápida 18W e inalámbrica MagSafe 15W\r\n\r\nOTROS\r\n\r\nWiFi 6, 5G, BT 5.0, NFC, GPS, dualSIM, eSIM, altavoces estéreo Dolby Atmos, reconocimiento facial, resistencia al agua IP68.", PrecioCompra = 600, PrecioVenta = 7500},
                    new ProductoColorAlamacenamientoInter() {idproducto = 8, idcolor = 2, idalmacenamiento = 1, Stock = 80, RutaImagen = "../Images/Products/Magsafe.png", Caracteristicas = "La funda cuenta con imanes que se alinean a la perfección con el, para que puedas conectarlo y cargarlo de forma inalámbrica con gran facilidad. No es necesario quitarla para cargar el teléfono con un cargador con certificación Qi o MagSafe", PrecioCompra = 200, PrecioVenta = 6000}
                };

            var insertarventas = new CarritoVenta[]
                {
                    new CarritoVenta() { Nombre = "Prueba", Cantidad = 10, Total = 1500, IVA = Convert.ToDecimal(1500*1.16), Fecha = DateTime.Now, idusuario = 1, Envio = "En Proceso", idproductointer = 2, Cambio = 2, IDOrden = 1, RutaImagen =  "../Images/Products/Airpods2.jpeg", IDPedido = $"{Guid.NewGuid()}"}
                };

            foreach (var u in insertardireccion)
                _contextDB.Direccion.Add(u);
            _contextDB.SaveChanges();

            foreach (var u in insertarusuarios)
                _contextDB.Usuario.Add(u);
            _contextDB.SaveChanges();

            foreach (var u in insertarventas)
                _contextDB.CarritoVenta.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarProveedor)
                _contextDB.Proveedores.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarColor)
                _contextDB.Colores.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarAlmacenamineto)
                _contextDB.Almacenamientos.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarProductos)
                _contextDB.Producto.Add(u);

            _contextDB.SaveChanges();

            foreach (var u in insertarProductosInter)
                _contextDB.ProductoInter.Add(u);

            _contextDB.SaveChanges();
        }
    }
}
