using System.ComponentModel.DataAnnotations;

namespace BookShop.Web.Models
{
    public class VentaViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un libro")]
        [Display(Name = "Libro")]
        public int LibroID { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Display(Name = "Total")]
        public decimal Total => Cantidad * PrecioUnitario;

        [Display(Name = "Stock Disponible")]
        public int StockDisponible { get; set; }

        // Propiedades para mostrar información
        [Display(Name = "Título del Libro")]
        public string? TituloLibro { get; set; }

        [Display(Name = "Autor")]
        public string? NombreAutor { get; set; }
    }
}
