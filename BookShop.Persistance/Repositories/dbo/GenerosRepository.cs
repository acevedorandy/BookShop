using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class GenerosRepository : BaseRepository<Generos>, IGenerosRepository
    {
        private readonly BookShopContext _bookShopContext;
        private readonly ILogger<GenerosRepository> _logger;

        public GenerosRepository(BookShopContext bookShopContext, ILogger<GenerosRepository> logger) : base(bookShopContext)
        {
            _bookShopContext = bookShopContext;
            _logger = logger;
        }

        public async override Task<OperationResult> Save(Generos genero)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await base.Save(genero);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el género.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Update(Generos genero)
        {
            OperationResult result = new OperationResult();
            try
            {
                Generos? generoToUpdate = await _bookShopContext.Generos.FindAsync(genero.GeneroID);
                if (generoToUpdate == null) throw new Exception("Género no encontrado");

                generoToUpdate.Nombre = genero.Nombre;
                result = await base.Update(generoToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el género.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Generos genero)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await base.Remove(genero);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error eliminando el género.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _bookShopContext.Generos
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo géneros.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _bookShopContext.Generos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g => g.GeneroID == Id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el género.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}
