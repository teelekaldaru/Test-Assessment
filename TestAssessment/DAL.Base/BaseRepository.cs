#pragma warning disable 1998
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.Domain.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base
{
    public class BaseRepository<TDbContext, TDomainEntity, TDALEntity> :
        BaseRepository<Guid, TDbContext, TDomainEntity, TDALEntity>,
        IBaseRepository<TDALEntity>
        where TDALEntity : class, IDomainEntityId<Guid>, new()
        where TDomainEntity : class, IDomainEntityId<Guid>, new()
        where TDbContext : DbContext, IBaseEntityTracker
    {
        public BaseRepository(TDbContext repoDbContext, IBaseMapper<TDomainEntity, TDALEntity> mapper) : base(
            repoDbContext, mapper)
        {
        }
    }

    public class BaseRepository<TKey, TDbContext, TDomainEntity, TDALEntity> :
        IBaseRepository<TKey, TDALEntity>
        where TDALEntity : class, IDomainEntityId<TKey>, new()
        where TDomainEntity : class, IDomainEntityId<TKey>, new()
        where TDbContext : DbContext, IBaseEntityTracker<TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly TDbContext RepoDbContext;
        protected readonly DbSet<TDomainEntity> RepoDbSet;
        protected readonly IBaseMapper<TDomainEntity, TDALEntity> Mapper;

        public BaseRepository(TDbContext repoDbContext, IBaseMapper<TDomainEntity, TDALEntity> mapper)
        {
            RepoDbContext = repoDbContext;
            RepoDbSet = RepoDbContext.Set<TDomainEntity>();
            Mapper = mapper;

            if (RepoDbSet == null)
            {
                throw new ArgumentNullException(typeof(TDALEntity).Name + " was not found as DbSet!");
            }
        }


        public virtual async Task<IEnumerable<TDALEntity>> AllAsync()
        {
            var query = PrepareQuery();
            var domainEntities = await query.ToListAsync();
            var result = domainEntities.Select(e => Mapper.Map(e));
            return result;
        }

        public virtual async Task<TDALEntity> FirstOrDefaultAsync(TKey id)
        {
            var query = PrepareQuery();
            var domainEntity = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
            var result = Mapper.Map(domainEntity);
            return result;
        }

        public virtual TDALEntity Add(TDALEntity entity)
        {
            var domainEntity = Mapper.Map(entity);
            var trackedDomainEntity = RepoDbSet.Add(domainEntity).Entity;
            RepoDbContext.AddToEntityTracker(trackedDomainEntity, entity);
            var result = Mapper.Map(trackedDomainEntity);
            return result;
        }

        public virtual async Task<TDALEntity> UpdateAsync(TDALEntity entity)
        {
            var domainEntity = Mapper.Map(entity);
            var trackedDomainEntity = RepoDbSet.Update(domainEntity).Entity;
            var result = Mapper.Map(trackedDomainEntity);
            return result;
        }

        public virtual async Task<TDALEntity> RemoveAsync(TKey id)
        {
            var query = PrepareQuery();
            var domainEntity = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (domainEntity == null)
            {
                throw new ArgumentException("Entity to be updated was not found in data source!");
            }
            return Mapper.Map(RepoDbSet.Remove(domainEntity).Entity);
        }

        public async Task<bool> ExistsAsync(TKey id)
        {
            var query = PrepareQuery();
            return await query.AnyAsync(e => e.Id.Equals(id));
        }

        protected IQueryable<TDomainEntity> PrepareQuery()
        {
            var query = RepoDbSet.AsQueryable();
            // Disable entity tracking
            query = query.AsNoTracking();
            return query;
        }
    }

}