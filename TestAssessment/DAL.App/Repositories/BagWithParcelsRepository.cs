using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class BagWithParcelsRepository : BaseRepository<AppDbContext, Domain.BagWithParcels, DAL.App.DTO.BagWithParcels>,
        IBagWithParcelsRepository
    {
        public BagWithParcelsRepository(AppDbContext repoDbContext) : base(repoDbContext,
            new DALMapper<Domain.BagWithParcels, DAL.App.DTO.BagWithParcels>())
        {
        }
        
        public async Task<bool> ExistsByBagNumberAsync(string bagNumber)
        {
            return await RepoDbSet.AnyAsync(b => b.BagNumber.Equals(bagNumber));
        }

        public async Task<List<BagWithParcelsView>> AllByShipmentAsync(Guid shipmentId)
        {
            return await RepoDbSet
                .Where(b => b.ShipmentId.Equals(shipmentId))
                .Select(b => new BagWithParcelsView()
                {
                    Id = b.Id,
                    BagNumber = b.BagNumber,
                    ShipmentId = b.ShipmentId,
                    ParcelsCount = b.Parcels.Count(),
                    TotalPrice = b.Parcels.Sum(p => p.Price),
                    TotalWeight = b.Parcels.Sum(p => p.Weight),
                    Parcels = b.Parcels.Select(p => new Parcel()
                    {
                        Id = p.Id,
                        BagWithParcelsId = p.BagWithParcelsId,
                        DestinationCountry = p.DestinationCountry,
                        ParcelNumber = p.ParcelNumber,
                        Price = p.Price,
                        RecipientName = p.RecipientName,
                        Weight = p.Weight
                    }).ToList()
                }).ToListAsync();
        }
    }
}