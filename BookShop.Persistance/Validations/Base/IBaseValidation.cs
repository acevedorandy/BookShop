

using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.Base
{
    public interface IBaseValidation<T>
    {
        OperationResult ValidateSave(T entity);
        OperationResult ValidateUpdate(T entity);
        OperationResult ValidateRemove(T entity);

    }
}
