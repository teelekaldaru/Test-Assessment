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
    public class BagWithLettersRepository : BaseRepository<AppDbContext, Domain.BagWithLetters, DAL.App.DTO.BagWithLetters>,
        IBagWithLettersRepository
    {
        public BagWithLettersRepository(AppDbContext repoDbContext) : base(repoDbContext,
            new DALMapper<Domain.BagWithLetters, DAL.App.DTO.BagWithLetters>())
        {
        }

        public async Task<bool> ExistsByBagNumberAsync(string bagNumber)
        {
            return await RepoDbSet.AnyAsync(b => b.BagNumber.Equals(bagNumber));
        }
        
        public async Task<List<BagWithLetters>> AllByShipmentAsync(Guid shipmentId)
        {
            return await RepoDbSet
                .Where(b => b.ShipmentId.Equals(shipmentId))
                .Select(b => new BagWithLetters()
                {
                    Id = b.Id,
                    BagNumber = b.BagNumber,
                    ShipmentId = b.ShipmentId,
                    CountOfLetters = b.CountOfLetters,
                    Price = b.Price,
                    Weight = b.Weight
                }).ToListAsync();
        }
    }

}