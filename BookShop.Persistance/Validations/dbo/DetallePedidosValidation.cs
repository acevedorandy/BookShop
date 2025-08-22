

using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Validations.Base;
using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.dbo
{
    public class DetallePedidosValidation : IBaseValidation<DetallePedidos>
    {
        public OperationResult ValidateRemove(DetallePedidos entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.DetalleID <= 0)
            {
                result.Success = false;
                result.Message = "El detalle del pedido es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateSave(DetallePedidos entity)
        {
            OperationResult result = new OperationResult();

            if (entity.PedidoID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del pedido es requerido.";
                return result;
            }

            if (entity.LibroID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del libro es requerido.";
                return result;
            }

            if (entity.Cantidad <= 0)
            {
                result.Success = false;
                result.Message = "La cantidad debe ser mayor a 0.";
                return result;
            }

            if (entity.PrecioUnitario <= 0)
            {
                result.Success = false;
                result.Message = "El precio unitario debe ser mayor a 0.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateUpdate(DetallePedidos entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.DetalleID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del detalle del pedido es requerido.";
                return result;
            }

            if (entity.PedidoID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del pedido es requerido.";
                return result;
            }

            if (entity.LibroID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del libro es requerido.";
                return result;
            }

            if (entity.Cantidad <= 0)
            {
                result.Success = false;
                result.Message = "La cantidad debe ser mayor a 0.";
                return result;
            }

            if (entity.PrecioUnitario <= 0)
            {
                result.Success = false;
                result.Message = "El precio unitario debe ser mayor a 0.";
                return result;
            }

            return result;
        }
    }
}
