using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class PedidosRepository : BaseRepository<Pedidos>, IPedidosRepository
    {
        private readonly BookShopContext _bookShopContext;
        private readonly ILogger<PedidosRepository> _logger;

        public PedidosRepository(BookShopContext bookShopContext, ILogger<PedidosRepository> logger) : base(bookShopContext)
        {
            _bookShopContext = bookShopContext;
            _logger = logger;
        }

        public async override Task<OperationResult> Save(Pedidos pedido)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await base.Save(pedido);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el pedido.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Update(Pedidos pedido)
        {
            OperationResult result = new OperationResult();
            try
            {
                Pedidos? pedidoToUpdate = await _bookShopContext.Pedidos.FindAsync(pedido.PedidoID);
                if (pedidoToUpdate == null) throw new Exception("Pedido no encontrado");

                pedidoToUpdate.ClienteID = pedido.ClienteID;
                pedidoToUpdate.FechaPedido = pedido.FechaPedido;
                pedidoToUpdate.MontoTotal = pedido.MontoTotal;

                result = await base.Update(pedidoToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el pedido.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Pedidos pedido)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await base.Remove(pedido);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error eliminando el pedido.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _bookShopContext.Pedidos
                    .Include(p => p.DetallePedidos)
                        .ThenInclude(dp => dp.Libro)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo pedidos.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _bookShopContext.Pedidos
                    .Include(p => p.DetallePedidos)
                        .ThenInclude(dp => dp.Libro)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PedidoID == Id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el pedido.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}
