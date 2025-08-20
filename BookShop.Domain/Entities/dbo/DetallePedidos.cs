
using System.ComponentModel.DataAnnotations;
namespace BookShop.Domain.Entities.dbo
{
    public class DetallePedidos
    {
        [Key]
        public int DetalleID { get; set; }
        public int PedidoID { get; set; }
        public int LibroID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public virtual Pedidos? Pedido { get; set; }
        public virtual Libros? Libro { get; set; }
    }
}