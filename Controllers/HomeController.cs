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
