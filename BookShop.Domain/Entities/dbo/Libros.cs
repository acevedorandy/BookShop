

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities.dbo
{
    [Table("Libros", Schema = "dbo")]
    public class Libros
    {
        [Key]
        public int LibroID { get; set; }
        public string Titulo { get; set; }
        public string ISBN { get; set; }
        public int AñoPublicacion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Portada { get; set; }
        public int AutorID { get; set; }
        public int GeneroID { get; set; }
    }
}
