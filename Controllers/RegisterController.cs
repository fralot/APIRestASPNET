using Microsoft.AspNetCore.Mvc;
using APIRest.Helpers;
using APIRest.Models;
using APIRest.Services;

namespace APIRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        private readonly IUserRepository _userService;
        private readonly IJWTHelper _jwtHelper;

        public RegisterController(IUserRepository userService, IJWTHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] RegisterModel user )
        {
            if (!ModelState.IsValid)
            {
                // Retornar todas las validaciones que no se cumplieron
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Message = "Errores de validación.", Errors = errors });
            }

            var existingUser = await _userService.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "El email ya está en uso." });
            }

            // Encriptar la contraseña con sal aleatoria
            var (hash, salt) = EncryptionHelper.HashPassword(user.Password);
            
            // Guardar el usuario en la base de datos con el hash y la sal
            await _userService.AddUserAsync(user.Email, hash, salt, user.Role);

            //return Ok(new { Message = "Registro exitoso." });

            var token = await _jwtHelper.GenerateJwtTokenAsync(user.Email, user.Role);

            return new OkObjectResult(new { token = token });
        }
    }
}