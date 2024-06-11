using Microsoft.AspNetCore.Mvc;
using PeroShopWeb.Models;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace PeroShopWeb.Controllers
{
    public class ComprasController : Controller
    {
        public readonly PerashopDB _ContextoDB;

        public int IdUser { get; set; }
        public string CorreoUsuario { get; set; }
        public decimal Total { get; set; }
        public int orden {  get; set; }
        public string pedidoid { get; set; }

        public Usuario usuario { get; set; }

        public ComprasController(PerashopDB perashop)
        {
            _ContextoDB = perashop;
        }

        public void Cookies()
        {
            var miCookie = HttpContext.Request.Cookies["MiCookie"];

            ViewBag.cookie = miCookie;

            if (miCookie != null)
            {
                List<Usuario> listaUsuarios = _ContextoDB.Usuario.ToList();
                foreach (var user in listaUsuarios)
                {
                    if (miCookie == user.Correo)
                    {
                        ViewBag.id = user.ID;
                        IdUser = user.ID;
                        ViewBag.Nombre = user.Nombre;
                        ViewBag.Nivel = user.TipoUsuario;
                        ViewBag.FotoPerfil = user.DireccionImagen;
                        usuario = user;
                    }
                }
            }
            int conteo = _ContextoDB.CarritoVenta
            .Where(c => c.Cambio == 1 && c.idusuario == IdUser)
            .Count();

            ViewBag.conteo = conteo;
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

        [HttpGet]
        public IActionResult Compra(int total)
        {
            Cookies();

            var carro = _ContextoDB.CarritoVenta.Where(c => c.idusuario == IdUser && c.Cambio == 1).ToList();
            var inter = _ContextoDB.ProductoInter.ToList();
            var ordenid = _ContextoDB.CarritoVenta.Max(c => c.IDOrden) + 1;

            foreach (var c in carro)
            {
                foreach (var o in inter)
                {
                    if (c.idproductointer == o.ID)
                    {
                        var comprada = (Int32)c.Cantidad;
                        var existencia = (Int32)o.Stock;

                        var restante = existencia - comprada;

                        if (restante < 0)
                        {
                            o.Stock = 0;
                        }
                        else
                            o.Stock = restante;

                        _ContextoDB.ProductoInter.Update(o);
                        _ContextoDB.SaveChanges();
                    }
                }

                c.Cambio = 2;
                c.Envio = "En Proceso";
                c.Fecha = DateTime.Now;
                c.IDOrden = ordenid;
                c.IDPedido = $"{Guid.NewGuid()}";
                pedidoid = c.IDPedido;

                _ContextoDB.CarritoVenta.Update(c);
                _ContextoDB.SaveChanges();
            }

            decimal totalToPay = total; 

            Total = totalToPay;
            orden = ordenid;

            ViewBag.Total = totalToPay;

            MandarCorreo();

            return View();
        }
        public async void MandarCorreo()
        {
            //xqqn prqe jrje culx
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tiendaperashop@gmail.com", "shjsvtgehcqnflgl"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("tiendaperashop@gmail.com"),
                Subject = "Confirmacion de la compra",
                Body = $@"
                <html>
                <body>
                    <h1>¡Gracias por tu compra!</h1>
                    <p>Tu pedido ha sido procesado exitosamente.</p>
                    <div>
                        <p><strong>Detalles del Pedido:</strong></p>
                        <p>Nombre: <strong>{usuario.Nombre}</strong></p>
                        <p>Telefono: <strong>{usuario.NumeroTelefono}</strong></p>
                        <p>Correo: <strong>{usuario.Correo}</strong></p>
                        <p>Fecha: <strong>{DateTime.Now}</strong></p>
                        <p>ID de Pedido: <strong>{pedidoid}</strong></p>
                        <p>Total: <strong>${Total}</strong></p>
                    </div>
                    <p>Debe presentar este correo en la tienda para que se le pueda entregar su compra</p>
                </body>
                </html>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add($"{usuario.Correo}");

            smtpClient.Send(mailMessage);
        }
    }
}
