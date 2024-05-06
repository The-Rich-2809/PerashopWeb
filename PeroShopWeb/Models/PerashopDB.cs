using Microsoft.EntityFrameworkCore;

namespace PeroShopWeb.Models
{
    public class PerashopDB : DbContext
    {
        public PerashopDB(DbContextOptions<PerashopDB> options) : base(options)
        {

        }

        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<ProductoColor> Colores { get; set; }
        public DbSet<ProductoAlmacenamiento> Almacenamientos { get; set; }
        public DbSet<ProductoColorAlamacenamientoInter> ProductoInter { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<CarritoVenta> CarritoVenta { get; set; }
    }
}
