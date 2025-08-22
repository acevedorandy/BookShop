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
    public sealed class ClientesRepository(BookShopContext bookShopContext,
                                           ILogger<ClientesRepository> logger, ClientesValidation clientesValidation) : BaseRepository<Clientes>(bookShopContext), IClientesRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<ClientesRepository> _logger = logger;
        private readonly ClientesValidation _clientesValidation = clientesValidation;

        public async override Task<OperationResult> Save(Clientes clientes)
        {
            OperationResult result = new OperationResult();

            var validationResult = _clientesValidation.ValidateSave(clientes);

            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Save(clientes);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el cliente.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Clientes clientes)
        {
            OperationResult result = new OperationResult();

            var validationResult = _clientesValidation.ValidateUpdate(clientes);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                Clientes? clienteToUpdate = await _bookShopContext.Clientes.FindAsync(clientes.ClienteID);
                clienteToUpdate.ClienteID = clientes.ClienteID;
                clienteToUpdate.Nombre = clientes.Nombre;
                clienteToUpdate.Apellido = clientes.Apellido;
                clienteToUpdate.Correo = clientes.Correo;
                clienteToUpdate.Telefono = clientes.Telefono;
                clienteToUpdate.Direccion = clientes.Direccion;

                result = await base.Update(clienteToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el cliente.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Clientes clientes)
        {
            OperationResult result = new OperationResult();

            var validationResult = _clientesValidation.ValidateRemove(clientes);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Remove(clientes);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el cliente.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from cliente in _bookShopContext.Clientes

                                     select new ClientesModel
                                     {
                                         ClienteID = cliente.ClienteID,
                                         Nombre = cliente.Nombre,
                                         Apellido = cliente.Apellido,
                                         Correo = cliente.Correo,
                                         Telefono = cliente.Telefono,
                                         Direccion = cliente.Direccion,
                                         FechaRegistro = cliente.FechaRegistro

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Hubo un error obteniendo los clientes.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from cliente in _bookShopContext.Clientes
                                     where cliente.ClienteID == id

                                     select new ClientesModel
                                     {
                                         ClienteID = cliente.ClienteID,
                                         Nombre = cliente.Nombre,
                                         Apellido = cliente.Apellido,
                                         Correo = cliente.Correo,
                                         Telefono = cliente.Telefono,
                                         Direccion = cliente.Direccion,
                                         FechaRegistro = cliente.FechaRegistro

                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Hubo un error obteniendo el cliente.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
