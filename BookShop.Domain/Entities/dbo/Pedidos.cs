

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Domain.Entities.dbo
{
    [Table("Pedidos", Schema = "dbo")]
    public class Pedidos
    {
        [Key]
        public int PedidoID { get; set; }
        public int ClienteID { get; set; }
        public DateTime FechaPedido {  get; set; }
        public decimal MontoTotal { get; set; }
    }
}
