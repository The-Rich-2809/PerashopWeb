using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PeroShopWeb.Models
{
    public class CarritoVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public decimal IVA { get; set; }
        public DateTime Fecha { get; set; }
        public string Envio { get; set; }
        public int idusuario { get; set; }
        public int idproductointer { get; set; }
        public string RutaImagen { get; set; }
        public int Cambio { get; set; }
        public int IDOrden { get; set; }
        public string IDPedido { get; set; }
    }
}
