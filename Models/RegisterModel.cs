using APIRest.Helpers;
using System.ComponentModel.DataAnnotations;

namespace APIRest.Models;
public class RegisterModel
{
    [Required(ErrorMessage = "El campo Email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El Email ingresado no es válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El campo Password es obligatorio.")]
    [CustomPasswordValidation(ErrorMessage = "La contraseña debe tener al menos 8 caracteres, conteniendo al menos una letra mayúscula, una minúscula y un número.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "El campo Role es obligatorio.")]
    [RegularExpression("^(USER|ADMIN)$", ErrorMessage = "El rol debe ser 'USER' o 'ADMIN'.")]
    public string Role { get; set; }
}
