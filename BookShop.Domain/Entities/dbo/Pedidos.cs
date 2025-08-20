
using System.ComponentModel.DataAnnotations;
namespace BookShop.Domain.Entities.dbo
{
    public class Pedidos
    {
        [Key]
        public int PedidoID { get; set; }
        public string? ClienteID { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal MontoTotal { get; set; }
        public virtual ICollection<DetallePedidos> DetallePedidos { get; set; } = new List<DetallePedidos>();
    }
}