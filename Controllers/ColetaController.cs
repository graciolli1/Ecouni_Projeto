using Microsoft.AspNetCore.Mvc;
using Ecouni_Projeto.Models;
using Ecouni_Projeto.Data;
using Microsoft.AspNetCore.Authorization; // Adicione este namespace

namespace SuaAplicacao.Controllers
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

        [HttpPost]
        public ActionResult RegistrarColeta([FromBody] Coleta coleta)
        {
            // Implemente aqui a lógica para verificar se o usuário tem permissão de acesso
            // Você pode acessar o usuário autenticado através do User.Identity.IsAuthenticated
            // e verificar o e-mail cadastrado do usuário (User.Identity.Name)

            // Se o e-mail do usuário não for "zoomkaique1528@gmail.com", retorne um StatusCode 403 (Forbidden)
            if (User.Identity.Name != "zoomkaique1528@gmail.com")
            {
                return Forbid();
            }

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
        [HttpGet("relatorios")]
        [Authorize(Roles = "Administrador")] // Apenas administradores têm acesso aos relatórios
        public ActionResult GerarRelatorios()
        {
            // Implemente aqui a lógica para gerar os relatórios com base nos dados de coleta
            // Certifique-se de retornar os dados apenas se o usuário for um administrador

            return Ok("Relatórios gerados com sucesso.");
        }
    }
}
