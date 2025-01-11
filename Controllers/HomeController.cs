using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet] // Ejemplo de acción que responde a GET
        public IActionResult Index()
        {
            return Ok(new { message = "Hello from API!" });
        }

        [HttpGet("GetSomething/{id}")] // Ejemplo de ruta con parámetro
        [Authorize]
        public IActionResult GetSomething(int id)
        {
            return Ok(new { id = id, message = $"You requested item with ID: {id}" });
        }

        [HttpGet("GetSomethingUserRole")]
        [Authorize(Roles = "USER")]
        public IActionResult GetSomethingUserRole(int id)
        {
            // Recuperar email y rol del usuario autenticado
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                id = id,
                email = email,
                role = role,
                message = $"You requested item with ID: {id} as a USER."
            });
        }

        [HttpGet("GetSomethingAdminRole")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult GetSomethingAdminRole(int id)
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                id = id,
                email = email,
                role = role,
                message = $"You requested item with ID: {id} as an ADMIN."
            });
        }

        [HttpPost] // Ejemplo de acción que responde a POST
        public IActionResult PostData([FromBody] SomeData data) // [FromBody] para datos en el cuerpo de la petición
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna errores de validación
            }

            return CreatedAtAction(nameof(GetSomething), new { id = 123 }, data); // Retorna 201 Created con ubicación
        }
    }

    public class SomeData
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
