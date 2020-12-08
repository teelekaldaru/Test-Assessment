using System;
using Contracts.Domain.Base;

namespace DAL.App.DTO
{
    public class Parcel : IDomainEntityId
    {
        public string ParcelNumber { get; set; } = default!;
        public string RecipientName { get; set; } = default!;
        public string DestinationCountry { get; set; } = default!;
        public double Weight { get; set; }
        public double Price { get; set; }
        public Guid BagWithParcelsId { get; set; }
        public BagWithParcels? BagWithParcels { get; set; }
        public Guid Id { get; set; }
    }
}