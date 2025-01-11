using System.ComponentModel.DataAnnotations;

namespace APIRest.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "El campo Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El Email ingresado no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Password es obligatorio.")]
        public string Password { get; set; }
    }
}
