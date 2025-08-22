
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities.dbo
{
    [Table("DetallePedidos", Schema = "dbo")]
    public class DetallePedidos
    {
        [Key]
        public int DetalleID { get; set; }
        public int PedidoID { get; set; }
        public int LibroID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

    }
}
