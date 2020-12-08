using System;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork, IBaseEntityTracker
    {
        IBagWithLettersRepository BagWithLetterses { get; }
        IBagWithParcelsRepository BagWithParcelses { get; }
        IParcelRepository Parcels { get; }
        IShipmentRepository Shipments { get; }
    }

}