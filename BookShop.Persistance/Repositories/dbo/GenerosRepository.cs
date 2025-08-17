using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class GenerosRepository(BookShopContext bookShopContext,
                                          ILogger<GenerosRepository> logger) : BaseRepository<Generos>(bookShopContext), IGenerosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<GenerosRepository> _logger = logger;

        public async override Task<OperationResult> Save(Generos generos)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async override Task<OperationResult> Update(Generos generos)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async override Task<OperationResult> Remove(Generos generos)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();

            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
