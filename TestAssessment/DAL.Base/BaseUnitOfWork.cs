using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base
{
    public abstract class BaseUnitOfWork<TKey, TDbContext> : IBaseUnitOfWork, IBaseEntityTracker<TKey> 
        where TKey : IEquatable<TKey>
        where TDbContext : DbContext
    {
        private readonly Dictionary<Type, object> _repoCache = new Dictionary<Type, object>();
        
        private readonly Dictionary<IDomainEntityId<TKey>, IDomainEntityId<TKey>> _entityTracker =
            new Dictionary<IDomainEntityId<TKey>, IDomainEntityId<TKey>>();
        
        protected readonly TDbContext UowDbContext;

        protected BaseUnitOfWork(TDbContext uowDbContext)
        {
            UowDbContext = uowDbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await UowDbContext.SaveChangesAsync();
             
            UpdateTrackedEntities();
            return result;
        }

        public TRepository GetRepository<TRepository>(Func<TRepository> repoCreationMethod)
            where TRepository : class
        {
            if (_repoCache.TryGetValue(typeof(TRepository), out var repo))
            {
                return (TRepository) repo;
            }

            var newRepoInstance = repoCreationMethod();
            _repoCache.Add(typeof(TRepository), newRepoInstance);
            return newRepoInstance;
        }

        public void AddToEntityTracker(IDomainEntityId<TKey> internalEntity, IDomainEntityId<TKey> externalEntity)
        {
            _entityTracker.Add(internalEntity, externalEntity);
        }

        private void UpdateTrackedEntities()
        {
            foreach (var (key, value) in _entityTracker)
            {
                value.Id = key.Id;
            }
        }
    }

}