using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;

namespace Domain
{
    public class BagWithLetters : IDomainEntityId
    {
        [Required]
        [MaxLength(15)] [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$")]
        public string BagNumber { get; set; } = default!;

        [Range(1, int.MaxValue)]
        public int CountOfLetters { get; set; }
        
        [Range(0.001, double.MaxValue)]
        [RegularExpression(@"^\d*\.?\d{1,3}$")]
        public double Weight { get; set; }
        
        [Range(0.01, double.MaxValue)]
        [RegularExpression(@"^\d*\.?\d{1,2}$")]
        public double Price { get; set; }

        public Guid ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
        
        public Guid Id { get; set; }
    }
}