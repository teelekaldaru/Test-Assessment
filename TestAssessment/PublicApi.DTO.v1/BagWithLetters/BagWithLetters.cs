using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;

namespace PublicApi.v1.DTO.BagWithLetters
{
    public class BagWithLetters : IDomainEntityId
    {
        [Required]
        [MaxLength(15)] [MinLength(1)]
        [RegularExpression(@"^[a-zA-Z0-9 ]+$",
            ErrorMessage = "No special symbols allowed.")]
        public string BagNumber { get; set; } = default!;

        [Range(1, int.MaxValue)]
        public int CountOfLetters { get; set; }
        
        [Range(0.001, double.MaxValue)]
        [RegularExpression(@"\d+(.\d{1,3})?",
            ErrorMessage = "Weight must be a valid number and can have maximum 3 decimal places after comma.")]
        public double Weight { get; set; }
        
        [Range(0.01, double.MaxValue)]
        [RegularExpression(@"\d+(.\d{1,2})?",
            ErrorMessage = "Price must be a valid number and can have maximum 2 decimal places after comma.")]
        public double Price { get; set; }

        [Required]
        public Guid ShipmentId { get; set; }
        
        [Required]
        public Guid Id { get; set; }
    }
}