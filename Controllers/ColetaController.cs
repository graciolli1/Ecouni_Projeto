using Microsoft.AspNetCore.Mvc;
using Ecouni_Projeto.Models;
using Ecouni_Projeto.Data;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Ecouni_Projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColetaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ColetaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("RegistrarColeta")]
        [AllowAnonymous]
        public ActionResult RegistrarColeta([FromBody] Coleta coleta)
        {
            if (coleta == null)
            {
                return BadRequest("Dados de coleta inválidos.");
            }

            // Aqui você pode adicionar lógica para validar os dados, se necessário

            // Defina a data da coleta como a data atual
            coleta.DataRegistro = DateTime.Now;

            _context.Coleta.Add(coleta);
            _context.SaveChanges();

            return Ok("Coleta registrada com sucesso.");
        }

        // Endpoint para recuperar os dados de coleta para gerar relatórios
        [HttpGet("ObterColetas")]
        [Authorize]
        public ActionResult<IEnumerable<Coleta>> ObterColetas()
        {
            var coletas = _context.Coleta.ToList(); // Recupera todas as coletas do banco de dados
            return Json(coletas); // Retorna os dados da coleta em formato JSON
        }
    }
}
