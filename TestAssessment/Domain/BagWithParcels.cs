using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;

namespace Domain
{
    public class BagWithParcels : IDomainEntityId
    {
        [Required]
        [MaxLength(15)] [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$")]
        public string BagNumber { get; set; } = default!;
        
        public Guid ShipmentId { get; set; } = default!;
        public Shipment? Shipment { get; set; }
        
        public ICollection<Parcel>? Parcels { get; set; }
        public Guid Id { get; set; }
    }
}