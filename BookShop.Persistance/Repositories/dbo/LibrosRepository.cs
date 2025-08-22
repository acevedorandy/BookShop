using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Persistance.Validations.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class LibrosRepository(BookShopContext bookShopContext,
                                         ILogger<LibrosRepository> logger, LibrosValidation librosValidation) : BaseRepository<Libros>(bookShopContext), ILibrosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<LibrosRepository> _logger = logger;
        private readonly LibrosValidation _librosValidation = librosValidation;

        public async override Task<OperationResult> Save(Libros libros)
        {
            OperationResult result = new OperationResult();

            var validationResult = _librosValidation.ValidateSave(libros);

            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Save(libros);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el libro.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Libros libros)
        {
            OperationResult result = new OperationResult();

            var validationResult = _librosValidation.ValidateUpdate(libros);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                Libros? librosToUpdate = await _bookShopContext.Libros.FindAsync(libros.LibroID);

                librosToUpdate.LibroID = libros.LibroID;
                librosToUpdate.Titulo = libros.Titulo;
                librosToUpdate.ISBN = libros.ISBN;
                librosToUpdate.AñoPublicacion = libros.AñoPublicacion;
                librosToUpdate.Precio = libros.Precio;
                librosToUpdate.Stock = libros.Stock;
                librosToUpdate.Portada = libros.Portada;
                librosToUpdate.AutorID = libros.AutorID;
                librosToUpdate.GeneroID = libros.GeneroID;

                result = await base.Update(librosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el libro.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Libros libros)
        {
            OperationResult result = new OperationResult();

            var validationResult = _librosValidation.ValidateRemove(libros);

            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Remove(libros);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el libro.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from libro in _bookShopContext.Libros
                                            join autor in _bookShopContext.Autores on libro.AutorID equals autor.AutorID
                                            join genero in _bookShopContext.Generos on libro.GeneroID equals genero.GeneroID
                                            select new
                                            {
                                                libro.LibroID,
                                                libro.Titulo,
                                                libro.ISBN,
                                                libro.AñoPublicacion,
                                                libro.Precio,
                                                libro.Stock,
                                                libro.Portada,
                                                Autor = autor.Nombre + " " + autor.Apellido,
                                                Genero = genero.Nombre

                                            }).AsNoTracking()
                                            .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los libros.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from libro in _bookShopContext.Libros
                                     join autor in _bookShopContext.Autores on libro.AutorID equals autor.AutorID
                                     join genero in _bookShopContext.Generos on libro.GeneroID equals genero.GeneroID
                                     where libro.LibroID == Id  

                                     select new
                                     {
                                         libro.LibroID,
                                         libro.Titulo,
                                         libro.ISBN,
                                         libro.AñoPublicacion,
                                         libro.Precio,
                                         libro.Stock,
                                         libro.Portada,
                                         Autor = autor.Nombre + " " + autor.Apellido,
                                         Genero = genero.Nombre

                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el libro.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
