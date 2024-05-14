using Ecouni_Projeto.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ecouni_Projeto.Models;
using Microsoft.EntityFrameworkCore;

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

                // Verifica se o modelo possui um e-mail fornecido
                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest("O e-mail é obrigatório para fazer login.");
                }

                // Chama o método AuthenticateByEmailAsync para verificar o e-mail e senha
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
    }
}
