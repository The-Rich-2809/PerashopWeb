using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PeroShopWeb.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Activo { get; set; }
        public string Nombre { get; set; }
        public string Caracteristicas { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Existencia { get; set; }
        public string RutaImagen { get; set; }
        public int idproveedor { get; set; }

        [ForeignKey("idproveedor")]
        public virtual Proveedores Proveedor { get; set; }
    }
}
