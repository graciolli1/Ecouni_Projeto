using Microsoft.AspNetCore.Mvc;
using Ecouni_Projeto.Models;
using Ecouni_Projeto.Data;
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
        public ActionResult RegistrarColeta([FromBody] Coleta coleta)
        {
            if (coleta == null)
            {
                return BadRequest("Dados de coleta inválidos.");
            }

            // Verifica se o Cadastrarid foi fornecido
            if (coleta.Cadastrarid <= 0)
            {
                return BadRequest("ID do usuário é inválido.");
            }

            // Defina a data da coleta como a data atual
            coleta.DataRegistro = DateTime.Now;

            _context.Coleta.Add(coleta);
            _context.SaveChanges();

            return Ok("Coleta registrada com sucesso.");
        }

        // Endpoint para recuperar os dados de coleta para gerar relatórios
        [HttpGet("ObterColetas/{Cadastrarid}")]
        public ActionResult<IEnumerable<Coleta>> ObterColetas(int Cadastrarid)
        {
            var coletas = _context.Coleta.Where(c => c.Cadastrarid == Cadastrarid).ToList();
            return Ok(coletas);
        }
    }
}
