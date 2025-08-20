using BookShop.Persistance.Interfaces.dbo;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Web.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly BookShop.Persistance.Context.BookShopContext _context;

        public EstadisticasController(BookShop.Persistance.Context.BookShopContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var estadisticas = await ObtenerEstadisticasGenerales();
            return View(estadisticas);
        }

        public async Task<IActionResult> Dashboard(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var inicio = fechaInicio ?? DateTime.Today.AddDays(-30);
            var fin = fechaFin ?? DateTime.Today;

            var estadisticas = await ObtenerEstadisticasPorFecha(inicio, fin);

            ViewBag.FechaInicio = inicio;
            ViewBag.FechaFin = fin;

            return View(estadisticas);
        }

        public async Task<IActionResult> StockBajo()
        {
            var librosStockBajo = await _context.Libros
                .Where(l => l.Stock <= 5)
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .OrderBy(l => l.Stock)
                .ToListAsync();

            var librosViewModel = librosStockBajo.Select(l => new LibroViewModel
            {
                LibroID = l.LibroID,
                Titulo = l.Titulo,
                ISBN = l.ISBN,
                AñoPublicacion = l.AñoPublicacion,
                Precio = l.Precio,
                Stock = l.Stock,
                Portada = l.Portada,
                AutorID = l.AutorID,
                GeneroID = l.GeneroID,
                NombreAutor = l.Autor != null ? $"{l.Autor.Nombre} {l.Autor.Apellido}" : "N/A",
                NombreGenero = l.Genero?.Nombre ?? "N/A"
            }).ToList();

            ViewBag.TotalLibrosStockBajo = librosViewModel.Count;
            ViewBag.UmbralStock = 5;

            return View(librosViewModel);
        }

        public async Task<IActionResult> Exportar(string formato, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var inicio = fechaInicio ?? DateTime.Today.AddDays(-30);
            var fin = fechaFin ?? DateTime.Today;

            var estadisticas = await ObtenerEstadisticasPorFecha(inicio, fin);

            switch (formato.ToLower())
            {
                case "excel":
                    return await ExportarExcel(estadisticas, inicio, fin);
                case "pdf":
                    return await ExportarPdf(estadisticas, inicio, fin);
                default:
                    return RedirectToAction(nameof(Dashboard));
            }
        }

        private async Task<EstadisticasViewModel> ObtenerEstadisticasGenerales()
        {
            var hoy = DateTime.Today;
            var inicioMes = new DateTime(hoy.Year, hoy.Month, 1);

            var ventasHoy = await _context.Pedidos
                .Where(p => p.FechaPedido.Date == hoy)
                .ToListAsync();

            var ventasMes = await _context.Pedidos
                .Where(p => p.FechaPedido.Date >= inicioMes && p.FechaPedido.Date <= hoy)
                .ToListAsync();

            var estadisticas = new EstadisticasViewModel
            {
                Fecha = hoy,
                TotalVentasDia = ventasHoy.Sum(v => v.MontoTotal),
                CantidadLibrosVendidos = await ObtenerCantidadLibrosVendidos(hoy),
                TotalVentasMes = ventasMes.Sum(v => v.MontoTotal),
                LibrosStockBajo = await _context.Libros.CountAsync(l => l.Stock <= 5),
                TotalLibrosInventario = await _context.Libros.CountAsync(),
                ValorTotalInventario = await _context.Libros.SumAsync(l => l.Precio * l.Stock)
            };

            estadisticas.LibrosMasVendidos = await ObtenerLibrosMasVendidos(inicioMes, hoy);
            estadisticas.VentasDiarias = await ObtenerVentasDiarias(inicioMes, hoy);

            return estadisticas;
        }

        private async Task<EstadisticasViewModel> ObtenerEstadisticasPorFecha(DateTime inicio, DateTime fin)
        {
            var ventas = await _context.Pedidos
                .Where(p => p.FechaPedido.Date >= inicio.Date && p.FechaPedido.Date <= fin.Date)
                .ToListAsync();

            var estadisticas = new EstadisticasViewModel
            {
                Fecha = DateTime.Today,
                TotalVentasDia = await ObtenerTotalVentasDia(DateTime.Today),
                CantidadLibrosVendidos = await ObtenerCantidadLibrosVendidos(inicio, fin),
                TotalVentasMes = ventas.Sum(v => v.MontoTotal),
                LibrosStockBajo = await _context.Libros.CountAsync(l => l.Stock <= 5),
                TotalLibrosInventario = await _context.Libros.CountAsync(),
                ValorTotalInventario = await _context.Libros.SumAsync(l => l.Precio * l.Stock)
            };

            estadisticas.LibrosMasVendidos = await ObtenerLibrosMasVendidos(inicio, fin);
            estadisticas.VentasDiarias = await ObtenerVentasDiarias(inicio, fin);

            return estadisticas;
        }

        private async Task<int> ObtenerCantidadLibrosVendidos(DateTime fecha)
        {
            return await _context.DetallePedidos
                .Include(d => d.Pedido)
                .Where(d => d.Pedido.FechaPedido.Date == fecha.Date)
                .SumAsync(d => d.Cantidad);
        }

        private async Task<int> ObtenerCantidadLibrosVendidos(DateTime inicio, DateTime fin)
        {
            return await _context.DetallePedidos
                .Include(d => d.Pedido)
                .Where(d => d.Pedido.FechaPedido.Date >= inicio.Date && d.Pedido.FechaPedido.Date <= fin.Date)
                .SumAsync(d => d.Cantidad);
        }

        private async Task<decimal> ObtenerTotalVentasDia(DateTime fecha)
        {
            return await _context.Pedidos
                .Where(p => p.FechaPedido.Date == fecha.Date)
                .SumAsync(p => p.MontoTotal);
        }

        private async Task<List<LibroMasVendidoViewModel>> ObtenerLibrosMasVendidos(DateTime inicio, DateTime fin)
        {
            return await _context.DetallePedidos
                .Include(d => d.Pedido)
                .Include(d => d.Libro)
                    .ThenInclude(l => l.Autor)
                .Where(d => d.Pedido.FechaPedido.Date >= inicio.Date && d.Pedido.FechaPedido.Date <= fin.Date)
                .GroupBy(d => d.LibroID)
                .Select(g => new LibroMasVendidoViewModel
                {
                    Titulo = g.First().Libro.Titulo,
                    Autor = g.First().Libro.Autor != null ? $"{g.First().Libro.Autor.Nombre} {g.First().Libro.Autor.Apellido}" : "N/A",
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalGenerado = g.Sum(d => d.Cantidad * d.PrecioUnitario)
                })
                .OrderByDescending(l => l.CantidadVendida)
                .Take(10)
                .ToListAsync();
        }

        private async Task<List<VentaDiariaViewModel>> ObtenerVentasDiarias(DateTime inicio, DateTime fin)
        {
            return await _context.Pedidos
                .Where(p => p.FechaPedido.Date >= inicio.Date && p.FechaPedido.Date <= fin.Date)
                .GroupBy(p => p.FechaPedido.Date)
                .Select(g => new VentaDiariaViewModel
                {
                    Fecha = g.Key,
                    CantidadVentas = g.Count(),
                    TotalGenerado = g.Sum(v => v.MontoTotal)
                })
                .OrderBy(v => v.Fecha)
                .ToListAsync();
        }

        private async Task<FileResult> ExportarExcel(EstadisticasViewModel estadisticas, DateTime inicio, DateTime fin)
        {
            var fileName = $"Reporte_Ventas_{inicio:yyyyMMdd}_{fin:yyyyMMdd}.xlsx";
            var content = System.Text.Encoding.UTF8.GetBytes("Reporte de ventas");
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        private async Task<FileResult> ExportarPdf(EstadisticasViewModel estadisticas, DateTime inicio, DateTime fin)
        {
            var fileName = $"Reporte_Ventas_{inicio:yyyyMMdd}_{fin:yyyyMMdd}.pdf";
            var content = System.Text.Encoding.UTF8.GetBytes("Reporte de ventas");
            return File(content, "application/pdf", fileName);
        }

        private string GetAutorName(int autorId)
        {
            var autor = _context.Autores.Find(autorId);
            return autor != null ? $"{autor.Nombre} {autor.Apellido}" : "N/A";
        }

        private string GetGeneroName(int generoId)
        {
            var genero = _context.Generos.Find(generoId);
            return genero?.Nombre ?? "N/A";
        }
    }
}