using BookShop.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Repositories;
using RealEstate.Domain.Result;

namespace RealEstate.Persistance.Base
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly BookShopContext _bookShopContext;
        private DbSet<TEntity> entities;

        public BaseRepository(BookShopContext bookShopContext)
        {
            _bookShopContext = bookShopContext;
            this.entities = _bookShopContext.Set<TEntity>();
        }

        public virtual async Task<OperationResult> GetAll()
        {
            OperationResult result = new OperationResult();

            try
            {
                var datos = await this.entities.ToListAsync();
                result.Data = datos;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Hubo un error obteniendo las entidades.";
            }
            return result;
        }

        public virtual async Task<OperationResult> GetById(int id)
        {
            OperationResult result = new OperationResult();

            try
            {
                var entity = await this.entities.FindAsync(id);
                result.Data = entity;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "hubo un error obteniendo la entidad.";
            }
            return result;
        }

        public virtual async Task<OperationResult> Remove(TEntity entity)
        {
            OperationResult result = new OperationResult();

            try
            {
                entities.Remove(entity);
                await _bookShopContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Hubo un error eliminando la entidad.";
            }
            return result;
        }

        public virtual async Task<OperationResult> Save(TEntity entity)
        {
            OperationResult result = new OperationResult();

            try
            {
                entities.Add(entity);
                await _bookShopContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Hubo un error guardando la entidad.";
            }
            return result;
        }

        public virtual async Task<OperationResult> Update(TEntity entity)
        {
            OperationResult result = new OperationResult();

            try
            {
                entities.Update(entity);
                await _bookShopContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Hubo un error actualizando la entidad.";
            }
            return result;
        }
    }
}
