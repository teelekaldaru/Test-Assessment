using System;
using System.Collections.Generic;
using Contracts.Domain.Base;

namespace DAL.App.DTO
{
    public class BagWithParcelsView : IDomainEntityId
    {
        public string BagNumber { get; set; } = default!;
        public int ParcelsCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalWeight { get; set; }
        public Guid ShipmentId { get; set; }
        public ICollection<Parcel>? Parcels { get; set; }
        public Guid Id { get; set; }
    }
}