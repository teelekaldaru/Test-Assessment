using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Contracts.Domain.Base;

namespace Domain
{
    public class Parcel : IDomainEntityId
    {
        [Required]
        [RegularExpression(@"[a-zA-Z]{2}\d{6}[a-zA-Z]{2}$")]
        public string ParcelNumber { get; set; } = default!;
        
        [Required]
        [MaxLength(100)] [MinLength(1)]
        public string RecipientName { get; set; } = default!;
        
        [Required]
        [MaxLength(2)] [MinLength(2)]
        public string DestinationCountry { get; set; } = default!;
        
        [Range(0.001, double.MaxValue)]
        [RegularExpression(@"^\d+.\d{1,3}$")]
        public double Weight { get; set; }
        
        [Range(0.01, double.MaxValue)]
        [RegularExpression(@"^\d+.\d{1,2}$")]
        public double Price { get; set; }

        public Guid BagWithParcelsId { get; set; }
        public BagWithParcels? BagWithParcels { get; set; }
        public Guid Id { get; set; }
    }
}