using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Context;
using BookShop.Persistance.Interfaces.dbo;
using BookShop.Persistance.Models.dbo;
using BookShop.Persistance.Validations.dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Result;
using RealEstate.Persistance.Base;

namespace BookShop.Persistance.Repositories.dbo
{
    public sealed class DetallePedidosRepository(BookShopContext bookShopContext,
                                                 ILogger<DetallePedidosRepository> logger, DetallePedidosValidation detallePedidosValidation) : BaseRepository<DetallePedidos>(bookShopContext), IDetallePedidosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<DetallePedidosRepository> _logger = logger;
        private readonly DetallePedidosValidation _detallePedidosValidation = detallePedidosValidation;

        public async override Task<OperationResult> Save(DetallePedidos detallePedidos)
        {
            OperationResult result = new OperationResult();

            var validationResult = _detallePedidosValidation.ValidateSave(detallePedidos);

            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Save(detallePedidos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el detalle del pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(DetallePedidos detallePedidos)
        {
            OperationResult result = new OperationResult();

            var validationResult = _detallePedidosValidation.ValidateUpdate(detallePedidos);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                DetallePedidos? detallePedidosToUpdate = await _bookShopContext.DetallePedidos.FindAsync(detallePedidos.DetalleID);

                detallePedidosToUpdate.DetalleID = detallePedidos.DetalleID;
                detallePedidosToUpdate.PedidoID = detallePedidos.PedidoID;
                detallePedidosToUpdate.LibroID = detallePedidos.LibroID;
                detallePedidosToUpdate.Cantidad = detallePedidos.Cantidad;
                detallePedidosToUpdate.PrecioUnitario = detallePedidos.PrecioUnitario;

                result = await base.Update(detallePedidosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el detalle del pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(DetallePedidos detallePedidos)
        {
            OperationResult result = new OperationResult();

            var validationResult = _detallePedidosValidation.ValidateRemove(detallePedidos);

            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Remove(detallePedidos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el detalle del pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from detallePedido in _bookShopContext.DetallePedidos
                                     join pedido in _bookShopContext.Pedidos on detallePedido.PedidoID equals pedido.PedidoID
                                     join libro in _bookShopContext.Libros on detallePedido.LibroID equals libro.LibroID

                                     select new DetallePedidosModel
                                     {
                                         DetalleID = detallePedido.DetalleID,
                                         PedidoID = detallePedido.PedidoID,
                                         LibroID = libro.LibroID,
                                         Cantidad = detallePedido.Cantidad,
                                         PrecioUnitario = detallePedido.PrecioUnitario

                                     }).AsNoTracking()
                                     .ToListAsync();

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los detalles de los pedidos.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from detallePedido in _bookShopContext.DetallePedidos
                                     join pedido in _bookShopContext.Pedidos on detallePedido.PedidoID equals pedido.PedidoID
                                     join libro in _bookShopContext.Libros on detallePedido.LibroID equals libro.LibroID
                                     where detallePedido.DetalleID == Id

                                     select new DetallePedidosModel
                                     {
                                         DetalleID = detallePedido.DetalleID,
                                         PedidoID = detallePedido.PedidoID,
                                         LibroID = libro.LibroID,
                                         Cantidad = detallePedido.Cantidad,
                                         PrecioUnitario = detallePedido.PrecioUnitario

                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el detalle del pedido.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
