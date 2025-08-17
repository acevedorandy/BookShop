using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class PedidosRepository(BookShopContext bookShopContext,
                                          ILogger<PedidosRepository> logger) : BaseRepository<Pedidos>(bookShopContext), IPedidosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<PedidosRepository> _logger = logger;

        public async override Task<OperationResult> Save(Pedidos pedidos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Save(pedidos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> Update(Pedidos pedidos)
        {
            OperationResult result = new OperationResult();

            try
            {
                Pedidos? pedidosToUpdate = await _bookShopContext.Pedidos.FindAsync(pedidos.PedidoID);

                pedidosToUpdate.PedidoID = pedidos.PedidoID;
                pedidosToUpdate.ClienteID = pedidos.ClienteID;
                pedidosToUpdate.FechaPedido = pedidos.FechaPedido;
                pedidosToUpdate.MontoTotal = pedidos.MontoTotal;

                result = await base.Update(pedidosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
        public async override Task<OperationResult> Remove(Pedidos pedidos)
        {
            OperationResult result = new OperationResult();

            try
            {
                result = await base.Remove(pedidos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        //Pendiente de completar hasta que termine el User de Identity

        //public async override Task<OperationResult> GetAll()
        //{
        //    OperationResult result = new OperationResult();

        //    try
        //    {
        //        result.Data = (from pedido in _bookShopContext.Pedidos
        //                       )
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return result;
        //}
        //public async override Task<OperationResult> GetById(int Id)
        //{
        //    OperationResult result = new OperationResult();

        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return result;
        //}
    }
}
