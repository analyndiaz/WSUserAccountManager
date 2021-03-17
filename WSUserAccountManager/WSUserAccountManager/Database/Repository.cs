using System;
using Microsoft.EntityFrameworkCore;
using WSUserAccountManager.Abstractions;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Database
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : CreatedInstance
    {
        private readonly UMSEntities _dbContext;
        public Repository(UMSEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return (await GetAll(filter)).FirstOrDefault();
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            var data = await _dbContext.Set<TEntity>().ToListAsync();

            if (filter != null)
            {
                data = data.Where(filter.Compile()).ToList();
            }

            return data;
        }

        public async Task<TEntity> Save(TEntity entity)
        {
            var isExist = _dbContext.Set<TEntity>().Contains(entity);
            if (!isExist)
            {
                entity = _dbContext.Set<TEntity>().Add(entity).Entity;
                entity.CreatedTime = DateTime.Now;
                entity.CreatedBy = "admin";
            }
            else
            {
                var updatedObject = _dbContext.Set<TEntity>().Attach(entity);
                _dbContext.Entry(updatedObject).State = EntityState.Modified;
            }

            entity.UpdatedTime = DateTime.Now;
            entity.UpdatedBy = "admin";

            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
