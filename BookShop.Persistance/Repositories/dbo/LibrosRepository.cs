using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class LibrosRepository(BookShopContext bookShopContext,
                                         ILogger<LibrosRepository> logger) : BaseRepository<Libros>(bookShopContext), ILibrosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<LibrosRepository> _logger = logger;

        public async override Task<OperationResult> Save(Libros libros)
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
        public async override Task<OperationResult> Update(Libros libros)
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
        public async override Task<OperationResult> Remove(Libros libros)
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
