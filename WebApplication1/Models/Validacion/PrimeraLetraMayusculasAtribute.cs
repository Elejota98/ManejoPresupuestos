using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models.Validacion
{
    public class PrimeraLetraMayusculasAtribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var primeraLetra = value.ToString()[0].ToString();
            if (primeraLetra != primeraLetra.ToUpper())
            {  // Si la letra es diferente a su version en Mayusculas
                return new ValidationResult("La primera letra debe ser en mayúscula");
            }
            return ValidationResult.Success;

        }
    }
}
