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
    public sealed class AutoresRepository : BaseRepository<Autores>, IAutoresRepository
    {
        private readonly BookShopContext _bookShopContext;
        private readonly ILogger<AutoresRepository> _logger;

        public AutoresRepository(BookShopContext bookShopContext, ILogger<AutoresRepository> logger)
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
                var autores = await _bookShopContext.Autores
                    .AsNoTracking()
                    .ToListAsync();

                result.Data = autores;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los autores.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();
            try
            {
                var autor = await _bookShopContext.Autores
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.AutorID == id);

                result.Data = autor;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el autor.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Save(Autores entity)
        {
            return await base.Save(entity);
        }

        public async override Task<OperationResult> Update(Autores entity)
        {
            return await base.Update(entity);
        }

        public async override Task<OperationResult> Remove(Autores entity)
        {
            return await base.Remove(entity);
        }
    }
}
