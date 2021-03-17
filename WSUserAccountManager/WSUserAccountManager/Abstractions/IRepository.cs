using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WSUserAccountManager.Database.Entities;

namespace WSUserAccountManager.Abstractions
{
    public interface IRepository<TEntity> where TEntity : CreatedInstance
    {
        Task<TEntity> Save(TEntity entity);

        Task<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter);
    }
}
