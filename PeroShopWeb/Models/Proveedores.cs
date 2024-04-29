using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PeroShopWeb.Models
{
    public class Proveedores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Activo { get; set; }
        public string PersonaContacto { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int iddireccion { get; set; }

        [ForeignKey("iddireccion")]
        public virtual Direccion Direccion { get; set; }
    }
}
