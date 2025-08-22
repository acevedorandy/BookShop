

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities.dbo
{
    [Table("Generos", Schema = "dbo")]
    public class Generos
    {
        [Key]
        public int GeneroID { get; set; }
        public string Nombre { get; set; }
    }
}
