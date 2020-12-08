using System;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.Repositories;
using DAL.Base;

namespace DAL.App
{
    public class AppUnitOfWork : BaseUnitOfWork<Guid, AppDbContext>, IAppUnitOfWork
    {
        public AppUnitOfWork(AppDbContext unitOfWorkDbContext) : base(unitOfWorkDbContext)
        {
        }
        
        public IBagWithLettersRepository BagWithLetterses =>
            GetRepository<IBagWithLettersRepository>(() => new BagWithLettersRepository(UowDbContext));
        
        public IBagWithParcelsRepository BagWithParcelses =>
            GetRepository<IBagWithParcelsRepository>(() => new BagWithParcelsRepository(UowDbContext));
        
        public IParcelRepository Parcels =>
            GetRepository<IParcelRepository>(() => new ParcelRepository(UowDbContext));
        
        public IShipmentRepository Shipments =>
            GetRepository<IShipmentRepository>(() => new ShipmentRepository(UowDbContext));
    }

}