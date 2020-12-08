using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.Domain.Base;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IParcelRepository  : IParcelRepository<Parcel>
    {
    }
    
    public interface IParcelRepository<TProject>  : IBaseRepository<TProject> 
        where TProject : class, IDomainEntityId<Guid>, new()
    {
        Task<bool> ExistsByParcelNumberAsync(string parcelNumber);
    }
}