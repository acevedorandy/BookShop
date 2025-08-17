using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class DetallePedidosRepository(BookShopContext bookShopContext,
                                                 ILogger<DetallePedidosRepository> logger) : BaseRepository<DetallePedidos>(bookShopContext), IDetallePedidosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<DetallePedidosRepository> _logger = logger;

        public async override Task<OperationResult> Save(DetallePedidos detallePedidos)
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
        public async override Task<OperationResult> Update(DetallePedidos detallePedidos)
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
        public async override Task<OperationResult> Remove(DetallePedidos detallePedidos)
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
