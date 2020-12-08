using System;
using Contracts.Domain.Base;

namespace DAL.App.DTO
{
    public class BagWithLetters : IDomainEntityId
    {
        public string BagNumber { get; set; } = default!;
        public int CountOfLetters { get; set; }
        public double Weight { get; set; }
        public double Price { get; set; }
        public Guid ShipmentId { get; set; } = default!;
        public Shipment? Shipment { get; set; }
        public Guid Id { get; set; }
    }
}