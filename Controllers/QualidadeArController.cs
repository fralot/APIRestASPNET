using Microsoft.AspNetCore.Mvc;

namespace APIRest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QualidadeArController : Controller
    {
        private readonly DatabaseContext _context;

        public QualidadeArController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var qualidades = _context.QualidadeAr.ToList(); 
            return new OkObjectResult(qualidades);
        }
    }
}