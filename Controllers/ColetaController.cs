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

        [HttpGet("ObterTodasColetas")]
        public ActionResult<IEnumerable<Coleta>> ObterTodasColetas()
        {
            var coletas = _context.Coleta.ToList();
            return Ok(coletas);
        }

        [HttpPut("EditarColeta/{Cadastrarid}/{id}")]
        public ActionResult EditarColeta(int Cadastrarid, int id, [FromBody] Coleta coletaAtualizada)
        {
            if (coletaAtualizada == null || Cadastrarid <= 0 || id <= 0)
            {
                return BadRequest("Dados de coleta inválidos.");
            }

            var coletaExistente = _context.Coleta.FirstOrDefault(c => c.Cadastrarid == Cadastrarid && c.Id == id);
            if (coletaExistente == null)
            {
                return NotFound("Coleta não encontrada.");
            }

            // Atualiza os campos da coleta
            coletaExistente.TipoResiduo = coletaAtualizada.TipoResiduo;
            coletaExistente.TamanhoSaco = coletaAtualizada.TamanhoSaco;
            coletaExistente.Quantidade = coletaAtualizada.Quantidade;

            _context.Coleta.Update(coletaExistente);
            _context.SaveChanges();

            return Ok("Coleta atualizada com sucesso.");
        }

        [HttpGet("ObterColeta/{Cadastrarid}/{id}")]
        public ActionResult ObterColeta(int Cadastrarid, int id)
        {
            var coleta = _context.Coleta.FirstOrDefault(c => c.Cadastrarid == Cadastrarid && c.Id == id);

            if (coleta == null)
            {
                return NotFound();
            }

            return Ok(coleta);
        }

        [HttpDelete("DeletarColeta/{Cadastrarid}/{id}")]
        public ActionResult DeletarColeta(int Cadastrarid, int id)
        {
            if (Cadastrarid <= 0 || id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var coletaExistente = _context.Coleta.FirstOrDefault(c => c.Cadastrarid == Cadastrarid && c.Id == id);
            if (coletaExistente == null)
            {
                return NotFound("Coleta não encontrada.");
            }

            _context.Coleta.Remove(coletaExistente);
            _context.SaveChanges();

            return Ok("Coleta deletada com sucesso.");
        }
    }
}
