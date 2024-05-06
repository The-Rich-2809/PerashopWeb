namespace PeroShopWeb.Models
{
    public class ProductosViewModel
    {
        public IEnumerable<Producto> Productos { get; set; }
        public IEnumerable<ProductoColorAlamacenamientoInter> ProductosInter { get; set; }
        public IEnumerable<ProductoAlmacenamiento> productoAlmacenamientos { get; set; }
        public IEnumerable<ProductoColor> ProductoColors { get; set; }
    }
}
