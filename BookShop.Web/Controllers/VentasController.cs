using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Web.Controllers
{
    public class VentasController : Controller
    {
        private readonly ILibrosRepository _librosRepository;
        private readonly IPedidosRepository _pedidosRepository;
        private readonly IDetallePedidosRepository _detallePedidosRepository;
        private readonly BookShop.Persistance.Context.BookShopContext _context;

        public VentasController(
            ILibrosRepository librosRepository,
            IPedidosRepository pedidosRepository,
            IDetallePedidosRepository detallePedidosRepository,
            BookShop.Persistance.Context.BookShopContext context)
        {
            _librosRepository = librosRepository;
            _pedidosRepository = pedidosRepository;
            _detallePedidosRepository = detallePedidosRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? fecha)
        {
            var fechaFiltro = fecha ?? DateTime.Today;

            var ventas = await _context.Pedidos
                .Where(p => p.FechaPedido.Date == fechaFiltro.Date)
                .Include(p => p.DetallePedidos)
                .ThenInclude(d => d.Libro)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            var ventasViewModel = ventas.Select(p => new VentaViewModel
            {
                LibroID = p.DetallePedidos.FirstOrDefault()?.LibroID ?? 0,
                Cantidad = p.DetallePedidos.Sum(d => d.Cantidad),
                PrecioUnitario = p.DetallePedidos.FirstOrDefault()?.PrecioUnitario ?? 0,
                TituloLibro = p.DetallePedidos.FirstOrDefault()?.Libro?.Titulo ?? "N/A",
                NombreAutor = GetAutorName(p.DetallePedidos.FirstOrDefault()?.Libro?.AutorID ?? 0)
            }).ToList();

            ViewBag.FechaSeleccionada = fechaFiltro;
            ViewBag.TotalVentas = ventas.Sum(p => p.MontoTotal);
            ViewBag.CantidadVentas = ventas.Count();

            return View(ventasViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var libros = await _context.Libros
                .Where(l => l.Stock > 0)
                .Include(l => l.Autor)
                .ToListAsync();

            ViewBag.Libros = libros.Select(l => new SelectListItem
            {
                Value = l.LibroID.ToString(),
                Text = $"{l.Titulo} - {(l.Autor != null ? $"{l.Autor.Nombre} {l.Autor.Apellido}" : "N/A")} (Stock: {l.Stock})"
            }).ToList();

            return View(new VentaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibroID,Cantidad")] VentaViewModel ventaViewModel)
        {
            if (ModelState.IsValid)
            {
                var libro = await _context.Libros.FindAsync(ventaViewModel.LibroID);
                if (libro == null)
                {
                    ModelState.AddModelError("", "Libro no encontrado.");
                    return View(ventaViewModel);
                }

                if (libro.Stock < ventaViewModel.Cantidad)
                {
                    ModelState.AddModelError("Cantidad", $"Stock insuficiente. Solo hay {libro.Stock} unidades disponibles.");
                    ViewBag.Libros = GetLibrosList();
                    return View(ventaViewModel);
                }

                try
                {
                    var pedido = new Pedidos
                    {
                        ClienteID = "ClienteGeneral",
                        FechaPedido = DateTime.Now,
                        MontoTotal = libro.Precio * ventaViewModel.Cantidad
                    };

                    var resultPedido = await _pedidosRepository.Save(pedido);
                    if (!resultPedido.Success)
                    {
                        ModelState.AddModelError("", "Error al crear el pedido.");
                        ViewBag.Libros = GetLibrosList();
                        return View(ventaViewModel);
                    }

                    var detallePedido = new DetallePedidos
                    {
                        PedidoID = pedido.PedidoID,
                        LibroID = ventaViewModel.LibroID,
                        Cantidad = ventaViewModel.Cantidad,
                        PrecioUnitario = libro.Precio
                    };

                    var resultDetalle = await _detallePedidosRepository.Save(detallePedido);
                    if (!resultDetalle.Success)
                    {
                        ModelState.AddModelError("", "Error al crear el detalle del pedido.");
                        ViewBag.Libros = GetLibrosList();
                        return View(ventaViewModel);
                    }

                    libro.Stock -= ventaViewModel.Cantidad;
                    var resultUpdate = await _librosRepository.Update(libro);
                    if (!resultUpdate.Success)
                    {
                        ModelState.AddModelError("", "Error al actualizar el stock.");
                        ViewBag.Libros = GetLibrosList();
                        return View(ventaViewModel);
                    }

                    TempData["SuccessMessage"] = $"Venta registrada exitosamente. Stock actualizado: {libro.Stock} unidades.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                    ViewBag.Libros = GetLibrosList();
                    return View(ventaViewModel);
                }
            }

            ViewBag.Libros = GetLibrosList();
            return View(ventaViewModel);
        }

        public async Task<IActionResult> Reporte(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var inicio = fechaInicio ?? DateTime.Today.AddDays(-30);
            var fin = fechaFin ?? DateTime.Today;

            var ventas = await _context.Pedidos
                .Where(p => p.FechaPedido.Date >= inicio.Date && p.FechaPedido.Date <= fin.Date)
                .Include(p => p.DetallePedidos)
                .ThenInclude(d => d.Libro)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            var reporteViewModel = new EstadisticasViewModel
            {
                Fecha = DateTime.Today,
                TotalVentasDia = ventas.Where(v => v.FechaPedido.Date == DateTime.Today).Sum(v => v.MontoTotal),
                CantidadLibrosVendidos = ventas.Sum(v => v.DetallePedidos.Sum(d => d.Cantidad)),
                TotalVentasMes = ventas.Sum(v => v.MontoTotal),
                LibrosStockBajo = await _context.Libros.CountAsync(l => l.Stock <= 5),
                TotalLibrosInventario = await _context.Libros.CountAsync(),
                ValorTotalInventario = await _context.Libros.SumAsync(l => l.Precio * l.Stock)
            };

            var librosMasVendidos = await _context.DetallePedidos
                .Include(d => d.Libro)
                    .ThenInclude(l => l.Autor)
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

            reporteViewModel.LibrosMasVendidos = librosMasVendidos;

            var ventasDiarias = ventas
                .GroupBy(v => v.FechaPedido.Date)
                .Select(g => new VentaDiariaViewModel
                {
                    Fecha = g.Key,
                    CantidadVentas = g.Count(),
                    TotalGenerado = g.Sum(v => v.MontoTotal)
                })
                .OrderBy(v => v.Fecha)
                .ToList();

            reporteViewModel.VentasDiarias = ventasDiarias;

            ViewBag.FechaInicio = inicio;
            ViewBag.FechaFin = fin;

            return View(reporteViewModel);
        }

        private List<SelectListItem> GetLibrosList()
        {
            var libros = _context.Libros
                .Where(l => l.Stock > 0)
                .Include(l => l.Autor)
                .ToList();

            return libros.Select(l => new SelectListItem
            {
                Value = l.LibroID.ToString(),
                Text = $"{l.Titulo} - {(l.Autor != null ? $"{l.Autor.Nombre} {l.Autor.Apellido}" : "N/A")} (Stock: {l.Stock})"
            }).ToList();
        }

        private string GetAutorName(int autorId)
        {
            var autor = _context.Autores.Find(autorId);
            return autor != null ? $"{autor.Nombre} {autor.Apellido}" : "N/A";
        }
    }
}