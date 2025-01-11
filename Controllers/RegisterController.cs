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
        private readonly UserService _userService;

        public RegisterController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] RegisterModel user )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna errores de validación
            }
            // Encriptar la contraseña con sal aleatoria
            var (hash, salt) = EncryptionHelper.HashPassword(user.Password);
            
            // Guardar el usuario en la base de datos con el hash y la sal
            await _userService.AddUserAsync(user.Email, hash, salt, user.Role);

            //return Ok(new { Message = "Registro exitoso." });

            var token = await _userService.GenerateJwtTokenAsync(user.Email, user.Role);

            HttpContext.Response.Cookies.Append("BearerToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });

            return new OkObjectResult(new { token = token });
        }
    }
}