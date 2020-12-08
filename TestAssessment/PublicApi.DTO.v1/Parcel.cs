using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Domain.Base;

namespace PublicApi.v1.DTO
{
    public class Parcel : IDomainEntityId
    {
        [Required]
        [RegularExpression(@"[a-zA-Z]{2}\d{6}[a-zA-Z]{2}$",
            ErrorMessage = "Parcel number must be in the format 'LLNNNNNNLL', where L - letter, N - digit.")]
        public string ParcelNumber { get; set; } = default!;
        
        [Required]
        [MaxLength(100)] [MinLength(1)]
        public string RecipientName { get; set; } = default!;
        
        [Required]
        [MaxLength(2, ErrorMessage = "Destination country must be a 2-letters code.")] 
        [MinLength(2, ErrorMessage = "Destination country must be a 2-letters code.")]
        public string DestinationCountry { get; set; } = default!;
        
        [Range(0.001, double.MaxValue)]
        [RegularExpression(@"\d+(.\d{1,3})?",
            ErrorMessage = "Weight must be a valid number and can have maximum 3 decimal places after comma.")]
        public double Weight { get; set; }
        
        [Range(0.01, double.MaxValue)]
        [RegularExpression(@"\d+(.\d{1,2})?",
            ErrorMessage = "Price must be a valid number and can have maximum 2 decimal places after comma.")]
        public double Price { get; set; }

        public Guid BagWithParcelsId{ get; set; }
        public Guid Id { get; set; }
    }
}