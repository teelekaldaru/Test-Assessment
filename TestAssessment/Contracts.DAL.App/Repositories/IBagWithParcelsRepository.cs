using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.Domain.Base;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IBagWithParcelsRepository  : IBagWithParcelsRepository<BagWithParcels>
    {
    }
    
    public interface IBagWithParcelsRepository<TProject>  : IBaseRepository<TProject> 
        where TProject : class, IDomainEntityId<Guid>, new()
    {
        Task<bool> ExistsByBagNumberAsync(string bagNumber);
        Task<List<BagWithParcelsView>> AllByShipmentAsync(Guid shipmentId);
    }
}