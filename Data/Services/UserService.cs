using System;
using System.Threading.Tasks;
using Ecouni_Projeto.Models;
using Ecouni_Projeto.Services.Interfaces;

namespace Ecouni_Projeto.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<Cadastrar> AuthenticateAsync(string email, string senha)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);

                if (user != null && user.Senha == senha)
                {
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao autenticar usuário. Detalhes: " + ex.Message);
            }
        }

        public async Task<Cadastrar> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar usuário pelo email. Detalhes: " + ex.Message);
            }
        }

        public async Task<Cadastrar> RegisterAsync(string nome, string email, string telefone, string senha, string confirmarSenha)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(email);
                if (existingUser != null)
                {
                    throw new ArgumentException("O email fornecido já está em uso.");
                }

                var newUser = new Cadastrar
                {
                    Nome = nome,
                    Email = email,
                    Telefone = telefone,
                    Senha = senha,
                    ConfirmarSenha = confirmarSenha
                };

                await _userRepository.AddUserAsync(newUser);

                return newUser;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Erro ao registrar usuário: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao registrar usuário. Detalhes: {ex.Message}", ex);
            }
        }

        public async Task UpdateUserAsync(Cadastrar user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(user.Cadastrarid);
                if (existingUser == null)
                {
                    throw new ArgumentException("Usuário não encontrado.");
                }

                existingUser.Nome = user.Nome;
                existingUser.Email = user.Email;
                existingUser.Telefone = user.Telefone;

                await _userRepository.UpdateUserAsync(existingUser);
            }
            catch (ArgumentException)
            {
                throw; 
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar informações do usuário. Detalhes: " + ex.Message);
            }
        }

        public async Task<Cadastrar> GetUserByIdAsync(int Cadastrarid)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(Cadastrarid);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar usuário pelo id. Detalhes: " + ex.Message);
            }
        }
        public async Task<bool> UserExistsAsync(int Cadastrarid)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(Cadastrarid);
                return user != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao verificar se o usuário existe. Detalhes: " + ex.Message);
            }
        }
    }
}
