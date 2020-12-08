using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.v1.DTO.BagWithParcels
{
    public class BagWithParcelsCreate
    {
        [Required]
        [MaxLength(15)] [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$",
            ErrorMessage = "No special symbols allowed.")]
        public string BagNumber { get; set; } = default!;
        
        [Required]
        public Guid ShipmentId { get; set; }
    }
}