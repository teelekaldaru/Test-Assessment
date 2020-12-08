using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.CustomDataAnnotations
{
    public class CurrentDateAttribute : ValidationAttribute
    {
        public CurrentDateAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            var dt = (DateTime)value;
            return dt >= DateTime.Now;
        }
    }
}