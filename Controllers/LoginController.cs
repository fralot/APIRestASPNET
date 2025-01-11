using Microsoft.AspNetCore.Mvc;
using APIRest.Models;
using APIRest.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using APIRest.Helpers;

namespace APIRest.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly UserService _userService;

        public LoginController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] LoginModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Dados inválidos. Por favor, tente novamente." });
            }

            var storedUser = await _userService.GetUserByEmailAsync(user.Email);
            if (storedUser == null)
            {
                return Unauthorized(new { Message = "Usuario no encontrado." });
            }
            bool isValid = EncryptionHelper.VerifyPassword(user.Password, storedUser.Password, storedUser.Salt);
            if (!isValid)
            {
                return Unauthorized(new { Message = "Credenciales inválidas." });
            }

            var userRole = await _userService.GetUserRoleAsync(user.Email, user.Password);
            if (!string.IsNullOrEmpty(userRole))
            {
                var token = await _userService.GenerateJwtTokenAsync(user.Email, userRole);
                return new OkObjectResult( new { token = token});
            }
            else
            {
                return BadRequest(new { Message = "Credenciais incorretas." });
            }
        }
    }

}