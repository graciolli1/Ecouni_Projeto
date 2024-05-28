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

                return Ok(new
                {
                    Token = token,
                    Cadastrarid = user.Cadastrarid,
                    Nome = user.Nome,
                    Email = user.Email
                });
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

        [HttpGet("user/{Cadastrarid}")]
        //[Authorize]
        public async Task<ActionResult<Cadastrar>> GetUser(int Cadastrarid)
        {
            var user = await _userService.GetUserByIdAsync(Cadastrarid);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("user/{Cadastrarid}")]
        //[Authorize]
        public async Task<IActionResult> UpdateUser(int Cadastrarid, [FromBody] Cadastrar updatedUser)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(Cadastrarid);
                if (user == null)
                {
                    return NotFound();
                }

                // Atualize apenas as propriedades relevantes
                user.Nome = updatedUser.Nome;
                user.Telefone = updatedUser.Telefone;
                user.Email = updatedUser.Email;

                // Verifique se a senha e a confirmação de senha estão presentes e correspondem
                if (!string.IsNullOrEmpty(updatedUser.Senha) && !string.IsNullOrEmpty(updatedUser.ConfirmarSenha))
                {
                    // Verifique se a senha e a confirmação de senha correspondem
                    if (updatedUser.Senha != updatedUser.ConfirmarSenha)
                    {
                        return BadRequest("A senha e a confirmação de senha não correspondem.");
                    }

                    // Atualize a senha
                    user.Senha = updatedUser.Senha;
                }

                await _userService.UpdateUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Trate outros tipos de exceção, se necessário
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("/usuarios/{email}")]
        public async Task<IActionResult> GetDetalhesUsuario(string email)
        {
            var usuario = await _userService.GetUserByEmailAsync(email);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }
    }
}
