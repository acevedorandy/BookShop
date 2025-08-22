using BookShop.Domain.Entities.dbo;
using BookShop.Persistance.Validations.Base;
using RealEstate.Domain.Result;

namespace BookShop.Persistance.Validations.dbo
{
    public class ClientesValidation : IBaseValidation<Clientes>
    {
        public OperationResult ValidateRemove(Clientes entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.ClienteID <= 0)
            {
                result.Success = false;
                result.Message = "El cliente es requerido.";
                return result;
            }

            return result;
        }

        public OperationResult ValidateSave(Clientes entity)
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

            if (string.IsNullOrEmpty(entity.Correo) || entity.Correo.Length > 150)
            {
                result.Success = false;
                result.Message = "El correo es requerido y debe tener máximo 150 caracteres.";
                return result;
            }

            if (!string.IsNullOrEmpty(entity.Telefono) && entity.Telefono.Length > 20)
            {
                result.Success = false;
                result.Message = "El teléfono debe tener máximo 20 caracteres.";
                return result;
            }

            if (!string.IsNullOrEmpty(entity.Direccion) && entity.Direccion.Length > 250)
            {
                result.Success = false;
                result.Message = "La dirección debe tener máximo 250 caracteres.";
                return result;
            }
            return result;
        }

        public OperationResult ValidateUpdate(Clientes entity)
        {
            OperationResult result = new OperationResult();

            if (entity == null || entity.ClienteID <= 0)
            {
                result.Success = false;
                result.Message = "El ID del cliente es requerido.";
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

            if (string.IsNullOrEmpty(entity.Correo) || entity.Correo.Length > 150)
            {
                result.Success = false;
                result.Message = "El correo es requerido y debe tener máximo 150 caracteres.";
                return result;
            }

            if (!string.IsNullOrEmpty(entity.Telefono) && entity.Telefono.Length > 20)
            {
                result.Success = false;
                result.Message = "El teléfono debe tener máximo 20 caracteres.";
                return result;
            }

            if (!string.IsNullOrEmpty(entity.Direccion) && entity.Direccion.Length > 250)
            {
                result.Success = false;
                result.Message = "La dirección debe tener máximo 250 caracteres.";
                return result;
            }
            return result;
        }
    }
}
