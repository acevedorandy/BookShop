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
    public sealed class AutoresRepository(BookShopContext bookShopContext,
                                          ILogger<AutoresRepository> logger, AutoresValidation autoresValidation) : BaseRepository<Autores>(bookShopContext), IAutoresRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<AutoresRepository> _logger = logger;
        private readonly AutoresValidation _autoresValidation = autoresValidation;

        public async override Task<OperationResult> Save(Autores autores)
        {
            OperationResult result = new OperationResult();

            var valiadtionResult = _autoresValidation.ValidateSave(autores);

            if (!valiadtionResult.Success)
            {
                result.Success = false;
                result.Message = valiadtionResult.Message;
                return result;
            }

            try
            {
                result = await base.Save(autores);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el autor.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Autores autores)
        {
            OperationResult result = new OperationResult();

            var validationResult = _autoresValidation.ValidateUpdate(autores);
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                Autores? autoresToUpdate = await _bookShopContext.Autores.FindAsync(autores.AutorID);

                autoresToUpdate.AutorID = autores.AutorID;
                autoresToUpdate.Nombre = autores.Nombre;
                autoresToUpdate.Apellido = autores.Apellido;
                autoresToUpdate.FechaNacimiento = autores.FechaNacimiento;
                autoresToUpdate.Pais = autores.Pais;

                result = await base.Update(autoresToUpdate);    
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el autor.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Autores autores)
        {
            OperationResult result = new OperationResult();

            var validationResult = _autoresValidation.ValidateRemove(autores);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Remove(autores);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el autor.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from autores in _bookShopContext.Autores

                                     select new AutoresModel
                                     {
                                         AutorID = autores.AutorID,
                                         Nombre = autores.Nombre,
                                         Apellido = autores.Apellido,
                                         FechaNacimiento = autores.FechaNacimiento,
                                         Pais = autores.Pais

                                     }).AsNoTracking()
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los autores.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from autores in _bookShopContext.Autores
                                     where autores.AutorID == Id    

                                     select new AutoresModel
                                     {
                                         AutorID = autores.AutorID,
                                         Nombre = autores.Nombre,
                                         Apellido = autores.Apellido,
                                         FechaNacimiento = autores.FechaNacimiento,
                                         Pais = autores.Pais

                                     }).AsNoTracking()
                                     .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el autor.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
