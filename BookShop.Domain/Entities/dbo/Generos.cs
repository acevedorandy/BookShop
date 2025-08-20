
using System.ComponentModel.DataAnnotations;
namespace BookShop.Domain.Entities.dbo
{
    public class Generos
    {
        [Key]
        public int GeneroID { get; set; }
        public string? Nombre { get; set; }
        public ICollection<Libros> Libros { get; set; } = new List<Libros>();
    }
}