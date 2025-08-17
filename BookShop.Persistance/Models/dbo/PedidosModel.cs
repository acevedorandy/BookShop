

namespace BookShop.Persistance.Models.dbo
{
    public class PedidosModel
    {
        public int PedidoID { get; set; }
        public string ClienteID { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal MontoTotal { get; set; }

    }
}
