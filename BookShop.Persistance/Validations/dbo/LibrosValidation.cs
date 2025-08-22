

using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Validations.Base;
using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.dbo
{
    public class LibrosValidation : IBaseValidation<Libros>
    {
        public OperationResult ValidateRemove(Libros entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.LibroID <= 0)
            {
                result.Success = false;
                result.Message = "El libro es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateSave(Libros entity)
        {
            OperationResult result = new OperationResult();

            if (string.IsNullOrEmpty(entity.Titulo) || entity.Titulo.Length > 200)
            {
                result.Success = false;
                result.Message = "El título es requerido y debe tener máximo 200 caracteres.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.ISBN) || entity.ISBN.Length > 20)
            {
                result.Success = false;
                result.Message = "El ISBN es requerido y debe tener máximo 20 caracteres.";
                return result;
            }

            if (entity.AñoPublicacion != null && (entity.AñoPublicacion < 0 || entity.AñoPublicacion > DateTime.Now.Year))
            {
                result.Success = false;
                result.Message = "El año de publicación no es válido.";
                return result;
            }

            if (entity.Precio <= 0)
            {
                result.Success = false;
                result.Message = "El precio debe ser mayor a 0.";
                return result;
            }

            if (entity.Stock < 0)
            {
                result.Success = false;
                result.Message = "El stock no puede ser negativo.";
                return result;
            }

            if (entity.AutorID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del autor es requerido.";
                return result;
            }

            if (entity.GeneroID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del género es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateUpdate(Libros entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.LibroID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del libro es requerido.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.Titulo) || entity.Titulo.Length > 200)
            {
                result.Success = false;
                result.Message = "El título es requerido y debe tener máximo 200 caracteres.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.ISBN) || entity.ISBN.Length > 20)
            {
                result.Success = false;
                result.Message = "El ISBN es requerido y debe tener máximo 20 caracteres.";
                return result;
            }

            if (entity.AñoPublicacion != null && (entity.AñoPublicacion < 0 || entity.AñoPublicacion > DateTime.Now.Year))
            {
                result.Success = false;
                result.Message = "El año de publicación no es válido.";
                return result;
            }

            if (entity.Precio <= 0)
            {
                result.Success = false;
                result.Message = "El precio debe ser mayor a 0.";
                return result;
            }

            if (entity.Stock < 0)
            {
                result.Success = false;
                result.Message = "El stock no puede ser negativo.";
                return result;
            }

            if (entity.AutorID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del autor es requerido.";
                return result;
            }

            if (entity.GeneroID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del género es requerido.";
                return result;
            }

            return result;
        }
    }
}
