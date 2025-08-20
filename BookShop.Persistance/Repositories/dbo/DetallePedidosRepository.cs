using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class DetallePedidosRepository : BaseRepository<DetallePedidos>, IDetallePedidosRepository
    {
        private readonly BookShopContext _bookShopContext;
        private readonly ILogger<DetallePedidosRepository> _logger;

        public DetallePedidosRepository(BookShopContext bookShopContext, ILogger<DetallePedidosRepository> logger) : base(bookShopContext)
        {
            _bookShopContext = bookShopContext;
            _logger = logger;
        }

        public async override Task<OperationResult> Save(DetallePedidos detalle)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await base.Save(detalle);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error guardando el detalle.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Update(DetallePedidos detalle)
        {
            OperationResult result = new OperationResult();
            try
            {
                DetallePedidos? detalleToUpdate = await _bookShopContext.DetallePedidos.FindAsync(detalle.DetalleID);
                if (detalleToUpdate == null) throw new Exception("Detalle no encontrado");

                detalleToUpdate.Cantidad = detalle.Cantidad;
                detalleToUpdate.PrecioUnitario = detalle.PrecioUnitario;
                detalleToUpdate.LibroID = detalle.LibroID;
                detalleToUpdate.PedidoID = detalle.PedidoID;

                result = await base.Update(detalleToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error actualizando el detalle.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> Remove(DetallePedidos detalle)
        {
            OperationResult result = new OperationResult();
            try
            {
                result = await base.Remove(detalle);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error eliminando el detalle.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _bookShopContext.DetallePedidos
                    .Include(dp => dp.Libro)
                    .Include(dp => dp.Pedido)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo detalles.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();
            try
            {
                result.Data = await _bookShopContext.DetallePedidos
                    .Include(dp => dp.Libro)
                    .Include(dp => dp.Pedido)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(dp => dp.DetalleID == Id);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Error obteniendo el detalle.";
                _logger.LogError(ex, result.Message);
            }
            return result;
        }
    }
}
