using System;
using System.Collections.Generic;
using Contracts.Domain.Base;

namespace DAL.App.DTO
{
    public class Shipment : IDomainEntityId
    {
        public string ShipmentNumber { get; set; } = default!;
        public Airport Airport { get; set; }
        public string FlightNumber { get; set; } = default!;
        public DateTime FlightDate { get; set; }
        public bool IsFinalized { get; set; }
        public Guid Id { get; set; }
    }
}