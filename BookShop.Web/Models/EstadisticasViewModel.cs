using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace BookShop.Web.Models

{
    public class EstadisticasViewModel
    {
        public DateTime Fecha { get; set; } = DateTime.Today;
        
        [Display(Name = "Total de Ventas del Día")]
        public decimal TotalVentasDia { get; set; }
        
        [Display(Name = "Cantidad de Libros Vendidos")]
        public int CantidadLibrosVendidos { get; set; }
        
        [Display(Name = "Total de Ventas del Mes")]
        public decimal TotalVentasMes { get; set; }
        
        [Display(Name = "Libros con Stock Bajo")]
        public int LibrosStockBajo { get; set; }
        
        [Display(Name = "Total de Libros en Inventario")]
        public int TotalLibrosInventario { get; set; }
        
        [Display(Name = "Valor Total del Inventario")]
        public decimal ValorTotalInventario { get; set; }
        
        public List<LibroMasVendidoViewModel> LibrosMasVendidos { get; set; } = new();
        public List<VentaDiariaViewModel> VentasDiarias { get; set; } = new();
    }

    public class LibroMasVendidoViewModel
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalGenerado { get; set; }
    }

    public class VentaDiariaViewModel
    {
        public DateTime Fecha { get; set; }
        public int CantidadVentas { get; set; }
        public decimal TotalGenerado { get; set; }
    }
}
