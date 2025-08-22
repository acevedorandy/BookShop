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
    public sealed class GenerosRepository(BookShopContext bookShopContext,
                                          ILogger<GenerosRepository> logger, GenerosValidation generosValidation) : BaseRepository<Generos>(bookShopContext), IGenerosRepository
    {
        private readonly BookShopContext _bookShopContext = bookShopContext;
        private readonly ILogger<GenerosRepository> _logger = logger;
        private readonly GenerosValidation _generosValidation = generosValidation;

        public async override Task<OperationResult> Save(Generos generos)
        {
            OperationResult result = new OperationResult();

            var validationResult = _generosValidation.ValidateSave(generos);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Save(generos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error guardando el autor.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Update(Generos generos)
        {
            OperationResult result = new OperationResult();

            var validationResult = _generosValidation.ValidateUpdate(generos);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                Generos? generosToUpdate = await _bookShopContext.Generos.FindAsync(generos.GeneroID);

                generosToUpdate.GeneroID = generos.GeneroID;
                generosToUpdate.Nombre = generos.Nombre;

                result = await base.Update(generosToUpdate);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error actualizando el genero.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> Remove(Generos generos)
        {
            OperationResult result = new OperationResult();

            var validationResult = _generosValidation.ValidateRemove(generos);
            
            if (!validationResult.Success)
            {
                result.Success = false;
                result.Message = validationResult.Message;
                return result;
            }

            try
            {
                result = await base.Remove(generos);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error eliminando el genero.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await(from genero in _bookShopContext.Generos
                                    
                                    select new GenerosModel
                                    {
                                        GeneroID = genero.GeneroID,
                                        Nombre = genero.Nombre

                                    }).AsNoTracking()
                                      .ToListAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo los generos.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }

        public async override Task<OperationResult> GetById(int Id)
        {
            OperationResult result = new OperationResult();

            try
            {
                result.Data = await (from genero in _bookShopContext.Generos
                                     where genero.GeneroID == Id

                                     select new GenerosModel
                                     {
                                         GeneroID = genero.GeneroID,
                                         Nombre = genero.Nombre

                                     }).AsNoTracking()
                                      .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Ha ocurrido un error obteniendo el genero.";
                _logger.LogError(result.Message, ex.ToString());
            }
            return result;
        }
    }
}
