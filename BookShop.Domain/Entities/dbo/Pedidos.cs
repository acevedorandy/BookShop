

namespace BookShop.Domain.Entities.dbo
{
    public class Pedidos
    {
        public int PedidoID { get; set; }
        public string ClienteID { get; set; }
        public DateTime FechaPedido {  get; set; }
        public decimal MontoTotal { get; set; }
    }
}
