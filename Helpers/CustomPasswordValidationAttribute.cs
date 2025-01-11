using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace APIRest.Helpers;
public class CustomPasswordValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var password = value as string;

        if (string.IsNullOrWhiteSpace(password))
        {
            return new ValidationResult("La contraseña no puede estar vacía.");
        }

        // Validar al menos 8 caracteres, una mayúscula, una minúscula y un número
        if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$"))
        {
            return new ValidationResult("La contraseña debe tener al menos 8 caracteres, incluyendo una letra mayúscula, una minúscula y un número.");
        }

        return ValidationResult.Success;
    }
}
