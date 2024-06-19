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

        [HttpPost("registro")]
        public async Task<ActionResult<Cadastrar>> Registro(Cadastrar model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest("O e-mail é obrigatório para o registro.");
                }

                var newUser = await _userService.RegisterAsync(model.Nome, model.Email, model.Telefone, model.Senha, model.ConfirmarSenha);

                if (newUser == null)
                {
                    return BadRequest("Erro ao registrar usuário");
                }

                return Created("api/auth/login", newUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }



        [HttpGet("user/{Cadastrarid}")]
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
        public async Task<IActionResult> UpdateUser(int Cadastrarid, [FromBody] Cadastrar updatedUser)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(Cadastrarid);
                if (user == null)
                {
                    return NotFound();
                }

                user.Nome = updatedUser.Nome;
                user.Telefone = updatedUser.Telefone;
                user.Email = updatedUser.Email;

                if (!string.IsNullOrEmpty(updatedUser.Senha) && !string.IsNullOrEmpty(updatedUser.ConfirmarSenha))
                {
                    if (updatedUser.Senha != updatedUser.ConfirmarSenha)
                    {
                        return BadRequest("A senha e a confirmação de senha não correspondem.");
                    }

                    user.Senha = updatedUser.Senha;
                }

                await _userService.UpdateUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
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
