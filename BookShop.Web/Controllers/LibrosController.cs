using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Web.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ILibrosRepository _librosRepository;
        private readonly IAutoresRepository _autoresRepository;
        private readonly IGenerosRepository _generosRepository;
        private readonly BookShop.Persistance.Context.BookShopContext _context;

        public LibrosController(
            ILibrosRepository librosRepository,
            IAutoresRepository autoresRepository,
            IGenerosRepository generosRepository,
            BookShop.Persistance.Context.BookShopContext context)
        {
            _librosRepository = librosRepository;
            _autoresRepository = autoresRepository;
            _generosRepository = generosRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchString, string? sortOrder)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["PrecioSortParm"] = sortOrder == "Precio" ? "precio_desc" : "Precio";
            ViewData["StockSortParm"] = sortOrder == "Stock" ? "stock_desc" : "Stock";
            ViewData["CurrentFilter"] = searchString;

            var libros = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                libros = libros.Where(l =>
                    (l.Titulo ?? "").Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    (l.ISBN ?? "").Contains(searchString, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            libros = sortOrder switch
            {
                "titulo_desc" => libros.OrderByDescending(l => l.Titulo ?? "").ToList(),
                "Precio" => libros.OrderBy(l => l.Precio).ToList(),
                "precio_desc" => libros.OrderByDescending(l => l.Precio).ToList(),
                "Stock" => libros.OrderBy(l => l.Stock).ToList(),
                "stock_desc" => libros.OrderByDescending(l => l.Stock).ToList(),
                _ => libros.OrderBy(l => l.Titulo ?? "").ToList(),
            };

            var librosViewModel = libros.Select(l => new LibroViewModel
            {
                LibroID = l.LibroID,
                Titulo = l.Titulo ?? "",
                ISBN = l.ISBN ?? "",
                AñoPublicacion = l.AñoPublicacion,
                Precio = l.Precio,
                Stock = l.Stock,
                Portada = l.Portada ?? "",
                AutorID = l.AutorID,
                GeneroID = l.GeneroID,
                NombreAutor = l.Autor != null ? $"{l.Autor.Nombre ?? ""} {l.Autor.Apellido ?? ""}" : "N/A",
                NombreGenero = l.Genero?.Nombre ?? "N/A"
            }).ToList();

            return View(librosViewModel);
        }

        public IActionResult Create()
        {
            ViewBag.Autores = GetAutoresList();
            ViewBag.Generos = GetGenerosList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,ISBN,AñoPublicacion,Precio,Stock,Portada,AutorID,GeneroID")] LibroViewModel libroViewModel)
        {
            if (ModelState.IsValid)
            {
                var libro = new Libros
                {
                    Titulo = libroViewModel.Titulo ?? "",
                    ISBN = libroViewModel.ISBN ?? "",
                    AñoPublicacion = libroViewModel.AñoPublicacion,
                    Precio = libroViewModel.Precio,
                    Stock = libroViewModel.Stock,
                    Portada = libroViewModel.Portada ?? "",
                    AutorID = libroViewModel.AutorID,
                    GeneroID = libroViewModel.GeneroID
                };

                var result = await _librosRepository.Save(libro);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Libro registrado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message ?? "Error al registrar el libro.");
                }
            }

            ViewBag.Autores = GetAutoresList();
            ViewBag.Generos = GetGenerosList();
            return View(libroViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            var libroViewModel = new LibroViewModel
            {
                LibroID = libro.LibroID,
                Titulo = libro.Titulo ?? "",
                ISBN = libro.ISBN ?? "",
                AñoPublicacion = libro.AñoPublicacion,
                Precio = libro.Precio,
                Stock = libro.Stock,
                Portada = libro.Portada ?? "",
                AutorID = libro.AutorID,
                GeneroID = libro.GeneroID
            };

            ViewBag.Autores = GetAutoresList();
            ViewBag.Generos = GetGenerosList();
            return View(libroViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibroID,Titulo,ISBN,AñoPublicacion,Precio,Stock,Portada,AutorID,GeneroID")] LibroViewModel libroViewModel)
        {
            if (id != libroViewModel.LibroID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var libro = new Libros
                {
                    LibroID = libroViewModel.LibroID,
                    Titulo = libroViewModel.Titulo ?? "",
                    ISBN = libroViewModel.ISBN ?? "",
                    AñoPublicacion = libroViewModel.AñoPublicacion,
                    Precio = libroViewModel.Precio,
                    Stock = libroViewModel.Stock,
                    Portada = libroViewModel.Portada ?? "",
                    AutorID = libroViewModel.AutorID,
                    GeneroID = libroViewModel.GeneroID
                };

                var result = await _librosRepository.Update(libro);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Libro actualizado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message ?? "Error al actualizar el libro.");
                }
            }

            ViewBag.Autores = GetAutoresList();
            ViewBag.Generos = GetGenerosList();
            return View(libroViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            var libroViewModel = new LibroViewModel
            {
                LibroID = libro.LibroID,
                Titulo = libro.Titulo ?? "",
                ISBN = libro.ISBN ?? "",
                AñoPublicacion = libro.AñoPublicacion,
                Precio = libro.Precio,
                Stock = libro.Stock,
                Portada = libro.Portada ?? "",
                AutorID = libro.AutorID,
                GeneroID = libro.GeneroID,
                NombreAutor = GetAutorName(libro.AutorID),
                NombreGenero = GetGeneroName(libro.GeneroID)
            };

            return View(libroViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                var result = await _librosRepository.Remove(libro);
                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Libro eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message ?? "Error al eliminar el libro.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetAutoresList()
        {
            var autores = _context.Autores.ToList();
            return autores.Select(a => new SelectListItem
            {
                Value = a.AutorID.ToString(),
                Text = $"{a.Nombre ?? ""} {a.Apellido ?? ""}"
            }).ToList();
        }

        private List<SelectListItem> GetGenerosList()
        {
            var generos = _context.Generos.ToList();
            return generos.Select(g => new SelectListItem
            {
                Value = g.GeneroID.ToString(),
                Text = g.Nombre ?? ""
            }).ToList();
        }

        private string GetAutorName(int autorId)
        {
            var autor = _context.Autores.Find(autorId);
            return autor != null ? $"{autor.Nombre ?? ""} {autor.Apellido ?? ""}" : "N/A";
        }

        private string GetGeneroName(int generoId)
        {
            var genero = _context.Generos.Find(generoId);
            return genero?.Nombre ?? "N/A";
        }
    }
}