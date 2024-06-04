using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PeroShopWeb.Models
{
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Activo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int idproveedor { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("idproveedor")]
        public virtual Proveedores Proveedor { get; set; }
    }
}
