using System;
using System.ComponentModel.DataAnnotations;

namespace Infraestructura.Core.Comun.Validacion
{
    public class UnicoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            throw new Exception();
            
            return ValidationResult.Success;
        }
    }
}
