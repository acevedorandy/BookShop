using BookShop.Domain.Entities.dbo;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Persistance.Context
{
    public partial class BookShopContext : DbContext
    {
        public BookShopContext(DbContextOptions<BookShopContext> options) : base(options)
        {
        }

        public DbSet<Autores> Autores { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<DetallePedidos> DetallePedidos { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
    }
}
