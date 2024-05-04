using System.ComponentModel.DataAnnotations.Schema;

namespace PeroShopWeb.Models
{
    public class ProductoColorAlamacenamientoInter
    {

        public int idproducto { get; set; }
        public int idcolor { get; set; }
        public int idalmacenamiento { get; set; }
        public int Stock { get; set; }
        public string RutaImagen { get; set; }
        public string Caracteristicas { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

        [ForeignKey("idproducto")]
        public virtual Producto Producto { get; set; }
        [ForeignKey("idcolor")]
        public virtual ProductoColor ProductoColor { get; set; }
        [ForeignKey("idalmacenamiento")]
        public virtual ProductoAlmacenamiento ProductoAlmacenamiento { get; set; }
    }
}
