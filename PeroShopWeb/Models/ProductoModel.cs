namespace PeroShopWeb.Models
{
    public class ProductoModel
    {
        public readonly PerashopDB _contextDB;

        public ProductoModel(PerashopDB contextDB)
        {
            _contextDB = contextDB;
        }

        public bool NuevoProducto(Producto producto)
        {
            try
            {
                var Producto = new Producto[]
                {
                    new Producto {Nombre = producto.Nombre, Categoria = producto.Categoria, idproveedor = producto.idproveedor, Activo = 1}
                };

                foreach (var us in Producto)
                {
                    _contextDB.Producto.Add(us);
                }
                _contextDB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool NuevasCaracteristicas(ProductoColorAlamacenamientoInter productointer)
        {
            try
            {
                var Producto = new ProductoColorAlamacenamientoInter[]
                {
                    productointer
                };

                foreach (var us in Producto)
                {
                    _contextDB.ProductoInter.Add(us);
                }
                _contextDB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EditarCaracteristicas(ProductoColorAlamacenamientoInter productointer)
        {
            try
            {
                _contextDB.ProductoInter.Update(productointer);
                _contextDB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
