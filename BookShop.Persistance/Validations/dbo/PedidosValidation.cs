using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Validations.Base;
using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.dbo
{
    public class PedidosValidation : IBaseValidation<Pedidos>
    {
        public OperationResult ValidateRemove(Pedidos entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.PedidoID <= 0)
            {
                result.Success = false;
                result.Message = "El pedido es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateSave(Pedidos entity)
        {
            OperationResult result = new OperationResult();

            if (entity.ClienteID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del cliente es requerido.";
                return result;
            }

            if (entity.MontoTotal < 0)
            {
                result.Success = false;
                result.Message = "El monto total no puede ser negativo.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateUpdate(Pedidos entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.PedidoID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del pedido es requerido.";
                return result;
            }

            if (entity.ClienteID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del cliente es requerido.";
                return result;
            }

            if (entity.MontoTotal < 0)
            {
                result.Success = false;
                result.Message = "El monto total no puede ser negativo.";
                return result;
            }

            return result;
        }
    }
}
