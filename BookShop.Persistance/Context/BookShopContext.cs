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
        public DbSet<DetallePedidos> DetallePedidos { get; set; }
        public DbSet<Generos> Generos { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Libro -> Autor
            modelBuilder.Entity<Libros>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Libro -> Genero
            modelBuilder.Entity<Libros>()
                .HasOne(l => l.Genero)
                .WithMany(g => g.Libros)
                .HasForeignKey(l => l.GeneroID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación DetallePedidos -> Libro
            modelBuilder.Entity<DetallePedidos>()
                .HasOne(dp => dp.Libro)
                .WithMany(l => l.DetallePedidos)
                .HasForeignKey(dp => dp.LibroID)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación DetallePedidos -> Pedido
            modelBuilder.Entity<DetallePedidos>()
                .HasOne(dp => dp.Pedido)
                .WithMany(p => p.DetallePedidos)
                .HasForeignKey(dp => dp.PedidoID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
