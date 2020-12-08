using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using Domain.CustomDataAnnotations;
using Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain
{
    public class Shipment : IDomainEntityId
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\d]{3}-[a-zA-Z\d]{6}$")]
        public string ShipmentNumber { get; set; } = default!;

        [Required]
        public Airport Airport { get; set; }
        
        [Required]
        [RegularExpression(@"^[a-zA-Z]{2}\d{4}$")]
        public string FlightNumber { get; set; } = default!;
        
        [CurrentDate(ErrorMessage = "Flight date cannot be in the past")]
        public DateTime FlightDate { get; set; }

        public bool IsFinalized { get; set; }
        
        public ICollection<BagWithParcels>? BagWithParcelses { get; set; }
        public ICollection<BagWithLetters>? BagWithLetterses { get; set; }
        public Guid Id { get; set; }
    }
}