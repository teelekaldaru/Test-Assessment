using System;
using System.Collections.Generic;
using Contracts.Domain.Base;

namespace DAL.App.DTO
{
    public class BagWithParcels : IDomainEntityId
    {
        public string BagNumber { get; set; } = default!;
        public int ParcelsCount { get; set; }
        public Guid ShipmentId { get; set; } = default!;
        public ICollection<Parcel>? Parcels { get; set; }
        public Guid Id { get; set; }
    }
}