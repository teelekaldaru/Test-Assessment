using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class ParcelRepository : BaseRepository<AppDbContext, Domain.Parcel, DAL.App.DTO.Parcel>,
        IParcelRepository
    {
        public ParcelRepository(AppDbContext repoDbContext) : base(repoDbContext,
            new DALMapper<Domain.Parcel, DAL.App.DTO.Parcel>())
        {
        }
        
        public async Task<bool> ExistsByParcelNumberAsync(string parcelNumber)
        {
            return await RepoDbSet.AnyAsync(b => b.ParcelNumber.Equals(parcelNumber));
        }
    }
}