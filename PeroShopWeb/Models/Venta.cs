using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PeroShopWeb.Models
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public decimal IVA { get; set; }
        public DateTime Fecha { get; set; }
        public int idusuario { get; set; }

        [ForeignKey("idusuarios")]
        public virtual Usuario Usuario { get; set; }

    }
}
