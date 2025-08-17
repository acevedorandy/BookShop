
namespace BookShop.Domain.Entities.dbo
{
    public class DetallePedidos
    {
        public int DetalleID { get; set; }
        public int PedidoID { get; set; }
        public int LibroID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

    }
}
