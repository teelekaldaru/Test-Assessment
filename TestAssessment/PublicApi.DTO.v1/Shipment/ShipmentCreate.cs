using System;
using System.ComponentModel.DataAnnotations;
using Domain.CustomDataAnnotations;

namespace PublicApi.v1.DTO.Shipment
{
    public class ShipmentCreate
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\d]{3}-[a-zA-Z\d]{6}$", 
            ErrorMessage = "Shipment number must be in the format 'XXX-XXXXXX', where X - letter or digit.")]
        public string ShipmentNumber { get; set; } = default!;
        
        [Required]
        public string Airport { get; set; } = default!;
        
        [Required]
        [RegularExpression(@"^[a-zA-Z]{2}\d{4}$",
            ErrorMessage = "Flight number must be in the format 'LLNNNN', where L - letter, N - digit.")]
        public string FlightNumber { get; set; } = default!;
        
        [CurrentDate(ErrorMessage = "Flight date cannot be in the past")]
        public DateTime FlightDate { get; set; }
        
        [Required]
        public bool IsFinalized { get; set; }
    }
}