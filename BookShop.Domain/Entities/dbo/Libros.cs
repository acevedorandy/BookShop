using System.ComponentModel.DataAnnotations;
namespace BookShop.Domain.Entities.dbo
{
    public class Libros
    {
        [Key]
        public int LibroID { get; set; }
        public string? Titulo { get; set; }
        public string? ISBN { get; set; }
        public int AñoPublicacion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? Portada { get; set; }
        public int AutorID { get; set; }
        public int GeneroID { get; set; }
        public Autores? Autor { get; set; }
        public Generos? Genero { get; set; }
        public ICollection<DetallePedidos> DetallePedidos { get; set; } = new List<DetallePedidos>();
    }
}