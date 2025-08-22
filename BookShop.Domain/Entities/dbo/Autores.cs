

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities.dbo
{
    [Table("Autores", Schema = "dbo")]
    public class Autores
    {
        [Key]
        public int AutorID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Pais {  get; set; }
    }
}
