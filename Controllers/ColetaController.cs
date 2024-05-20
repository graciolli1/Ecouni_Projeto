using Microsoft.AspNetCore.Mvc;
using Ecouni_Projeto.Models;
using Ecouni_Projeto.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecouni_Projeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColetaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ColetaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("RegistrarColeta")]
        [Authorize]
        public ActionResult RegistrarColeta([FromBody] Coleta coleta)
        {
            if (coleta == null)
            {
                return BadRequest("Dados de coleta inválidos.");
            }

            // Aqui você pode adicionar lógica para validar os dados, se necessário

            // Obtenha o ID do usuário autenticado (presumindo que você está usando autenticação JWT)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Define o ID do usuário na coleta
            coleta.Cadastrarid = int.Parse(userId);

            // Defina a data da coleta como a data atual
            coleta.DataRegistro = DateTime.Now;

            _context.Coleta.Add(coleta);
            _context.SaveChanges();

            return Ok("Coleta registrada com sucesso.");
        }

        // Endpoint para recuperar os dados de coleta para gerar relatórios
        [HttpGet("ObterColetas")]
        public ActionResult<IEnumerable<Coleta>> ObterColetas(int userId)
        {
            var coletas = _context.Coleta.Where(c => c.Cadastrarid == userId).ToList();
            return Ok(coletas); // Retorna os dados da coleta em formato JSON
        }
    }
}
