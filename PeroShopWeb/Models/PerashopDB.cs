using Microsoft.EntityFrameworkCore;

namespace PeroShopWeb.Models
{
    public class PerashopDB : DbContext
    {
        public PerashopDB(DbContextOptions<PerashopDB> options) : base(options)
        {

        }

        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Direccion> Direccion { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Venta> Venta { get; set; }
    }
}
