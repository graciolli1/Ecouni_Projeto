using Ecouni_Projeto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecouni_Projeto.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Ecouni_Projeto.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest("O e-mail é obrigatório para fazer login.");
                }

                var user = await _userService.AuthenticateAsync(model.Email, model.Senha);

                if (user == null)
                {
                    return Unauthorized("Credenciais inválidas");
                }

                var token = _jwtService.GenerateToken(user);

                return Ok(new { Token = token });
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Operação inválida");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(Cadastrar model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newUser = await _userService.RegisterAsync(model.Nome, model.Email, model.Telefone, model.Senha, model.ConfirmarSenha);

                return Created("api/auth/login", newUser);
            }
            catch (ArgumentException)
            {
                return BadRequest("Argumento inválido");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Erro ao registrar usuário");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Novo método para obter informações do usuário
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<Cadastrar>> GetUser(int Cadastrarid)
        {
            var user = await _userService.GetUserByIdAsync(Cadastrarid);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Novo método para atualizar informações do usuário
        [HttpPut("user/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Cadastrar updatedUser)
        {
            if (id != updatedUser.Cadastrarid)
            {
                return BadRequest();
            }

            try
            {
                var user = await _userService.GetUserByIdAsync(updatedUser.Cadastrarid);
                if (user == null)
                {
                    return NotFound();
                }

                user.Nome = updatedUser.Nome;
                user.Telefone = updatedUser.Telefone;
                user.Email = updatedUser.Email;

                await _userService.UpdateUserAsync(user);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userService.UserExistsAsync(updatedUser.Cadastrarid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [HttpGet("/usuarios/{email}")]
        public IActionResult GetDetalhesUsuario(string email)
        {
            // Lógica para buscar o usuário com base no e-mail
            var usuario = _userService.GetUserByEmailAsync(email);
            if (usuario == null)
            {
                return NotFound(); // Retornar NotFound se o usuário não for encontrado
            }

            return Ok(usuario); // Retornar os detalhes do usuário em caso de sucesso
        }

    }
}
