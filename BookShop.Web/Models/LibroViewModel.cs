using System.ComponentModel.DataAnnotations;

namespace BookShop.Web.Models
{
    public class LibroViewModel
    {
        public int LibroID { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [Display(Name = "ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año de publicación es obligatorio")]
        [Display(Name = "Año de Publicación")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int AñoPublicacion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Display(Name = "Stock")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        [Display(Name = "Portada")]
        public string? Portada { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [Display(Name = "Autor")]
        public int AutorID { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [Display(Name = "Género")]
        public int GeneroID { get; set; }

        // Propiedades de navegación para mostrar información
        [Display(Name = "Autor")]
        public string? NombreAutor { get; set; }

        [Display(Name = "Género")]
        public string? NombreGenero { get; set; }

        [Display(Name = "Stock Bajo")]
        public bool StockBajo => Stock <= 5;
    }
}
