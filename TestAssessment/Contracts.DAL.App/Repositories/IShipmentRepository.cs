using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.Domain.Base;
using DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IShipmentRepository  : IShipmentRepository<Shipment>
    {
    }
    
    public interface IShipmentRepository<TProject>  : IBaseRepository<TProject> 
        where TProject : class, IDomainEntityId<Guid>, new()
    {
        Task<bool> ExistsByShipmentNumberAsync(string shipmentNumber);
    }

}