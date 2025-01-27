using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ImportacionesApp.API.Data;
using ImportacionesApp.API.Models;

namespace ImportacionesApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly GIAXContext _context;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(GIAXContext context, ILogger<CompanyController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCompanies()
        {
            try
            {
                var companies = await _context.BiCompanyViewPbi
                    .Where(c => c.STATUSCOMPANY == "Activa" && 
                           c.COMPANYCLASSIFICATION != "Configuracion")
                    .Select(c => new { c.ID, c.NAME })
                    .OrderBy(c => c.NAME)
                    .ToListAsync();

                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el listado de compañías");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("default")]
        public async Task<ActionResult<object>> GetDefaultCompany()
        {
            try
            {
                var defaultCompany = await _context.BiCompanyViewPbi
                    .Where(c => c.ID == "CMER" && 
                           c.STATUSCOMPANY == "Activa" && 
                           c.COMPANYCLASSIFICATION != "Configuracion")
                    .Select(c => new { c.ID, c.NAME })
                    .FirstOrDefaultAsync();

                if (defaultCompany == null)
                {
                    return NotFound(new { message = "Compañía predeterminada no encontrada" });
                }

                return Ok(defaultCompany);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la compañía predeterminada");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
    }
}