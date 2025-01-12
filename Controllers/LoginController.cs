using Microsoft.AspNetCore.Mvc;
using APIRest.Models;
using APIRest.Services;
using APIRest.Helpers;

namespace APIRest.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJWTHelper _jwtHelper;

        public LoginController(IUserService userService, IJWTHelper jwtHelper)
        {
            _userService = userService;
            _jwtHelper = jwtHelper;
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
                var token = await _jwtHelper.GenerateJwtTokenAsync(user.Email, userRole);
                return new OkObjectResult( new { token = token});
            }
            else
            {
                return BadRequest(new { Message = "Credenciais incorretas." });
            }
        }
    }

}