using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class ShipmentRepository : BaseRepository<AppDbContext, Domain.Shipment, DAL.App.DTO.Shipment>,
        IShipmentRepository
    {
        public ShipmentRepository(AppDbContext repoDbContext) : base(repoDbContext,
            new DALMapper<Domain.Shipment, DAL.App.DTO.Shipment>())
        {
        }
        
        public Task<bool> ExistsByShipmentNumberAsync(string shipmentNumber)
        {
            return RepoDbSet.AnyAsync(s => s.ShipmentNumber.Equals(shipmentNumber));
        }
    }
}