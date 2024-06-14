using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;
using System.Linq;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using PeroShopWeb.Providers;
using PeroShopWeb.Helpers;
using System.Text.Json;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace PeroShopWeb.Controllers
{
    public class AdminController : Controller
    {
        private HelperUploadFiles helperUpload;
        private readonly PerashopDB _contextDB;
        public static string MyProperty { get; set; }

        public AdminController(HelperUploadFiles helperUpload, PerashopDB perashopDB)
        {
            _contextDB = perashopDB;
            this.helperUpload = helperUpload;
        }
        public static int IdProducto { get; set; }

        public static int idinter { get; set; }
        public static string CategoriaProducto { get; set; }

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

        [HttpPost]
        public IActionResult EditarUsuarios(Usuario usuario)
        {
            Cookies();

           var u = _contextDB.Usuario.FirstOrDefault(u => u.ID == usuario.ID);

            u.TipoUsuario = usuario.TipoUsuario;
            u.DireccionImagen = "../Images/Usuarios/Usuario.jpg";

            _contextDB.Usuario.Update(u);
            _contextDB.SaveChanges();

            return RedirectToAction("Usuarios");
        }

        [HttpGet]
        public IActionResult EliminarUsuarios(int id)
        {
            var usuario = _contextDB.Usuario.FirstOrDefault(p => p.ID == id);
            Cookies();
            return View(usuario);
        }

        [HttpPost]
        public IActionResult EliminarUsuarios(Usuario usuario)
        {
            var usua = _contextDB.Usuario.FirstOrDefault(p => p.ID == usuario.ID);
            Cookies();

            _contextDB.Usuario.Remove(usua);
            _contextDB.SaveChanges();

            return RedirectToAction("Usuarios");
        }

        [HttpGet]
        public IActionResult Ventas()
        {
            List<CarritoVenta> listaventas = _contextDB.CarritoVenta.Where(c => c.Cambio == 2).ToList();
            Cookies();
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
            List<CarritoVenta> listaventas = _contextDB.CarritoVenta.Where(c => c.Cambio == 2 && c.Envio == "Tu paquete a sido entregado").ToList();
            Cookies();
            return View(listaventas);
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

            List<ProductoColorAlamacenamientoInter> listproductoInter = _contextDB.ProductoInter.ToList();
            List<ProductoColor> listproductoColor = _contextDB.Colores.ToList();
            List<ProductoAlmacenamiento> listproductoAlmacenamiento = _contextDB.Almacenamientos.ToList();
            var viewmodel = new ProductosViewModel
            {
                ProductosInter = listproductoInter,
                ProductoColors = listproductoColor,
                productoAlmacenamientos = listproductoAlmacenamiento
            };
            return View(viewmodel);
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

            ViewBag.idprod = IdProducto;
            Cookies();
            ViewBag.Categoria = CategoriaProducto;
            return View(viewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> AgregaCaracteristicas(ProductoColorAlamacenamientoInter productointer, IFormFile[] Imagen)
        {
            ProductoModel productoModel = new ProductoModel(_contextDB);

            if (productointer.idalmacenamiento == 1)
            {
                ViewBag.Error = "Seleccione un almacenamiento valido";
                return RedirectToAction("AgregaCaracteristicas");
            }
            else
            {
                if (Imagen == null)
                {
                    ViewBag.Mensaje = "Seleccione una imagen para el producto";
                    return View();
                }
                else
                {
                    productointer.idproducto = IdProducto;
                    if (CategoriaProducto != "Telefonos")
                    {
                        productointer.idalmacenamiento = 1;
                    }

                    string nombreImagen = +productointer.idproducto + "_" + productointer.idcolor + "_" + productointer.idalmacenamiento + "_" + Imagen[0].FileName;
                    await this.helperUpload.UploadFilesAsync(Imagen[0], nombreImagen, Folders.Productos);

                    productointer.RutaImagen = "../Images/Products/" + nombreImagen;
                    productoModel.NuevasCaracteristicas(productointer);
                    return RedirectToAction("Productos");
                }
            }
        }

        [HttpGet]
        public IActionResult EditarCaracteristicas(int id)
        {
            var listproductoInter = _contextDB.ProductoInter.Where(p => p.ID == id);
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
            ViewBag.idinter = id;

            return View(viewmodel);
        }
        [HttpPost]
        public async Task<IActionResult> EditarCaracteristicas(ProductoColorAlamacenamientoInter productointer, IFormFile[] Imagen)
        {
            ProductoModel productoModel = new ProductoModel(_contextDB);
            productointer.idproducto = IdProducto;
            if (CategoriaProducto != "Telefonos")
            {
                productointer.idalmacenamiento = 1;
            }

            if (Imagen == null)
            {
                string nombreImagen = +productointer.idproducto + "_" + productointer.idcolor + "_" + productointer.idalmacenamiento + "_" + Imagen[0].FileName;
                await this.helperUpload.UploadFilesAsync(Imagen[0], nombreImagen, Folders.Productos);

                productointer.RutaImagen = "../Images/Products/" + nombreImagen;
            }
            else
                productointer.RutaImagen = _contextDB.ProductoInter.FirstOrDefault(p => p.ID == productointer.ID).RutaImagen;

            var u = productointer;


            _contextDB.ProductoInter.Update(u);
            _contextDB.SaveChanges();
            return RedirectToAction("Productos");
        }

        [HttpGet]
        public IActionResult EliminarCaracteristicas(int id)
        {
            idinter = id;
            List<ProductoColorAlamacenamientoInter> listproductoInter = _contextDB.ProductoInter.Where(p => p.idproducto == IdProducto).ToList();
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
        public IActionResult EliminarCaracteristicas()
        {
            var caracteristica = _contextDB.ProductoInter.FirstOrDefault(i => i.ID == idinter);

            _contextDB.ProductoInter.Remove(caracteristica);
            _contextDB.SaveChanges();
            
            return RedirectToAction("Productos");
        }

        public IActionResult Graficas()
        {
            Cookies();

            var coloresMasVendidos = (from cv in _contextDB.CarritoVenta
                                      join pci in _contextDB.ProductoInter on cv.idproductointer equals pci.ID
                                      join pc in _contextDB.Colores on pci.idcolor equals pc.ID
                                      group cv by pc.Color into g
                                      select new
                                      {
                                          Color = g.Key,
                                          CantidadVendida = g.Sum(cv => cv.Cantidad)
                                      })
                             .OrderByDescending(g => g.CantidadVendida)
                             .ToList();

            var almacenamientosMasComprados = (from cv in _contextDB.CarritoVenta
                                               join pci in _contextDB.ProductoInter on cv.idproductointer equals pci.ID
                                               join pa in _contextDB.Almacenamientos on pci.idalmacenamiento equals pa.ID
                                               group cv by pa.Almacenamineto into g
                                               select new
                                               {
                                                   Almacenamiento = g.Key,
                                                   CantidadComprada = g.Sum(cv => cv.Cantidad)
                                               })
                                               .OrderByDescending(g => g.CantidadComprada)
                                               .ToList();

            var productosMasComprados = (from cv in _contextDB.CarritoVenta
                                         join pci in _contextDB.ProductoInter on cv.idproductointer equals pci.ID
                                         join p in _contextDB.Producto on pci.idproducto equals p.ID
                                         group cv by p.Nombre into g
                                         select new
                                         {
                                             Producto = g.Key,
                                             CantidadComprada = g.Sum(cv => cv.Cantidad)
                                         })
                                         .OrderByDescending(g => g.CantidadComprada)
                                         .ToList();

            // Gráfica: Cantidad de Productos por Categoría
            var productosPorCategoria = _contextDB.Producto
                .GroupBy(p => p.Categoria)
                .Select(g => new { Categoria = g.Key, Cantidad = g.Count() })
                .ToList();

            // Gráfica: Cantidad de Productos Activos vs Inactivos
            var productosActivos = _contextDB.Producto.Count(p => p.Activo == 1);
            var productosInactivos = _contextDB.Producto.Count(p => p.Activo == 0);

            // Gráfica: Distribución de Productos por Fecha de Creación
            var productosPorFecha = _contextDB.Producto
                .ToList() // Trae los datos a la memoria para procesar
                .GroupBy(p => p.Fecha.Date)
                .Select(g => new { Fecha = g.Key.ToString("yyyy-MM-dd"), Cantidad = g.Count() })
                .OrderBy(p => p.Fecha)
                .ToList();

            ViewBag.Colores = coloresMasVendidos.Select(c => c.Color).ToList();
            ViewBag.CantidadesColores = coloresMasVendidos.Select(c => c.CantidadVendida).ToList();
            ViewBag.Almacenamientos = almacenamientosMasComprados.Select(a => a.Almacenamiento).ToList();
            ViewBag.CantidadesAlmacenamientos = almacenamientosMasComprados.Select(a => a.CantidadComprada).ToList();
            ViewBag.Productos = productosMasComprados.Select(p => p.Producto).ToList();
            ViewBag.CantidadesProductos = productosMasComprados.Select(p => p.CantidadComprada).ToList();

            ViewBag.Categorias = productosPorCategoria.Select(p => p.Categoria).ToList();
            ViewBag.CantidadesCategorias = productosPorCategoria.Select(p => p.Cantidad).ToList();
            ViewBag.Activos = productosActivos;
            ViewBag.Inactivos = productosInactivos;
            ViewBag.Fechas = productosPorFecha.Select(p => p.Fecha).ToList();
            ViewBag.CantidadesFechas = productosPorFecha.Select(p => p.Cantidad).ToList();

            return View();
        }

    }
}
