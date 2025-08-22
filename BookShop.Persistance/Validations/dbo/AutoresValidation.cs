using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Validations.Base;
using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.dbo
{
    public class AutoresValidation : IBaseValidation<Autores>
    {
        public OperationResult ValidateRemove(Autores entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.AutorID <= 0)
            {
                result.Success = false;
                result.Message = "El autor es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateSave(Autores entity)
        {
            OperationResult result = new OperationResult();

            if (string.IsNullOrEmpty(entity.Nombre) || entity.Nombre.Length > 100)
            {
                result.Success = false;
                result.Message = "El nombre es requerido y debe tener máximo 100 caracteres.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.Apellido) || entity.Apellido.Length > 100)
            {
                result.Success = false;
                result.Message = "El apellido es requerido y debe tener máximo 100 caracteres.";
                return result;
            }

            if (!string.IsNullOrEmpty(entity.Pais) && entity.Pais.Length > 100)
            {
                result.Success = false;
                result.Message = "El país debe tener máximo 100 caracteres.";
                return result;
            }

            if (entity.FechaNacimiento != null && entity.FechaNacimiento > DateTime.Now)
            {
                result.Success = false;
                result.Message = "La fecha de nacimiento no puede ser en el futuro.";
                return result;
            }
            return result;
        }

        public OperationResult ValidateUpdate(Autores entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.AutorID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del autor es requerido.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.Nombre) || entity.Nombre.Length > 100)
            {
                result.Success = false;
                result.Message = "El nombre es requerido y debe tener máximo 100 caracteres.";
                return result;
            }

            if (string.IsNullOrEmpty(entity.Apellido) || entity.Apellido.Length > 100)
            {
                result.Success = false;
                result.Message = "El apellido es requerido y debe tener máximo 100 caracteres.";
                return result;
            }

            if (!string.IsNullOrEmpty(entity.Pais) && entity.Pais.Length > 100)
            {
                result.Success = false;
                result.Message = "El país debe tener máximo 100 caracteres.";
                return result;
            }

            if (entity.FechaNacimiento != null && entity.FechaNacimiento > DateTime.Now)
            {
                result.Success = false;
                result.Message = "La fecha de nacimiento no puede ser en el futuro.";
                return result;
            }
            return result;
        }
    }
}

