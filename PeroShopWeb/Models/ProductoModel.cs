namespace PeroShopWeb.Models
{
    public class ProductoModel
    {
        public readonly PerashopDB _ContextoDB;

        public ProductoModel(PerashopDB perashopDB)
        {
            _ContextoDB = perashopDB;
        }

        public List<Producto> Pro()
        {
            List<Producto> listaProductos = _ContextoDB.Producto.ToList();

            return listaProductos;
        }
    }
}
