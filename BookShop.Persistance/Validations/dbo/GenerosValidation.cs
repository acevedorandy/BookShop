

using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Validations.Base;
using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.dbo
{
    public class GenerosValidation : IBaseValidation<Generos>
    {
        public OperationResult ValidateRemove(Generos entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.GeneroID <= 0)
            {
                result.Success = false;
                result.Message = "El género es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateSave(Generos entity)
        {
            OperationResult result = new OperationResult();

            if (string.IsNullOrEmpty(entity.Nombre) || entity.Nombre.Length > 100)
            {
                result.Success = false;
                result.Message = "El nombre del género es requerido y debe tener máximo 100 caracteres.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateUpdate(Generos entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.GeneroID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del género es requerido.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.Nombre) || entity.Nombre.Length > 100)
            {
                result.Success = false;
                result.Message = "El nombre del género es requerido y debe tener máximo 100 caracteres.";
                return result;
            }

            return result;
        }
    }
}
