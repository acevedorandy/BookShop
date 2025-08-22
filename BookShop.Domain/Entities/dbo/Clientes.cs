

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities.dbo
{
    [Table("Clientes", Schema = "dbo")]
    public class Clientes
    {
        [Key]
        public int ClienteID { get; set; }  
        public string Nombre { get; set; } 
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
