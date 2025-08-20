using System.ComponentModel.DataAnnotations;
namespace BookShop.Domain.Entities.dbo
{
    public class Autores
    {
        [Key]
        public int AutorID { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Pais { get; set; }
        public ICollection<Libros> Libros { get; set; } = new List<Libros>();
    }
}