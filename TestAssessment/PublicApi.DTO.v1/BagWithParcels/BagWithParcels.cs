using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;

namespace PublicApi.v1.DTO.BagWithParcels
{
    public class BagWithParcels : IDomainEntityId
    {
        [Required]
        [MaxLength(15)] [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$",
            ErrorMessage = "No special symbols allowed.")]
        public string BagNumber { get; set; } = default!;
        
        [Required]
        public Guid ShipmentId { get; set; }
        
        [Required]
        public Guid Id { get; set; }
    }
}