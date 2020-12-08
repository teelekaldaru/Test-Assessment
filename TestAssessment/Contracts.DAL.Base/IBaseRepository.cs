using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Domain.Base;

namespace Contracts.DAL.Base
{
    public interface IBaseRepository<TEntity> : IBaseRepository<Guid, TEntity> 
        where TEntity : class, IDomainEntityId<Guid>, new()
    {
    }

    public interface IBaseRepository<in TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IDomainEntityId<TKey>, new()
    {
        Task<IEnumerable<TEntity>> AllAsync();
        Task<TEntity> FirstOrDefaultAsync(TKey id);
        TEntity Add(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> RemoveAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
    }

}