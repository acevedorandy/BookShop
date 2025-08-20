using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Persistance.Models.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class LibrosRepository : BaseRepository<Libros>, ILibrosRepository
    {
        private readonly BookShopContext _bookShopContext;
        private readonly ILogger<LibrosRepository> _logger;

        public LibrosRepository(BookShopContext bookShopContext, ILogger<LibrosRepository> logger)
            : base(bookShopContext)
        {
            _bookShopContext = bookShopContext;
            _logger = logger;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                var libros = await _bookShopContext.Libros
                    .Include(l => l.Autor)
                    .Include(l => l.Genero)
                    .AsNoTracking()
                    .ToListAsync();

                result.Data = libros;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los libros.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var libro = await _bookShopContext.Libros
                    .Include(l => l.Autor)
                    .Include(l => l.Genero)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.LibroID == id);

                result.Data = libro;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el libro.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Save(Libros entity)
        {
            return await base.Save(entity);
        }

        public async override Task<OperationResult> Update(Libros entity)
        {
            return await base.Update(entity);
        }

        public async override Task<OperationResult> Remove(Libros entity)
        {
            return await base.Remove(entity);
        }
    }
}
