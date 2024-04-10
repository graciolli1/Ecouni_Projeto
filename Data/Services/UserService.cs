using System;
using System.Threading.Tasks;
using Ecouni_Projeto.Services.Interfaces;
using Ecouni_Projeto.Models;

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
                // Busca o usuário pelo email
                var user = await _userRepository.GetUserByEmailAsync(email);

                // Verifica se o usuário foi encontrado e se a senha corresponde
                if (user != null && user.Senha == senha)
                {
                    // Autenticação bem-sucedida, retorna o usuário
                    return user;
                }

                // Se não houver correspondência de usuário ou senha, retorna null
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao autenticar usuário. Detalhes: " + ex.Message);
            }
        }

        public async Task<Cadastrar> RegisterAsync(string nome, string email, string telefone, string senha, string confirmarSenha)
        {
            try
            {
                // Verifica se o email já está cadastrado
                var existingUser = await _userRepository.GetUserByEmailAsync(email);
                if (existingUser != null)
                {
                    throw new ArgumentException("O email fornecido já está em uso.");
                }

                // Cria um novo usuário com os dados fornecidos
                var newUser = new Cadastrar
                {
                    Nome = nome,
                    Email = email,
                    Telefone = telefone,
                    Senha = senha // Por simplicidade, apenas atribuímos a senha diretamente. Você pode modificar esta lógica para armazenar a senha de forma segura.
                };

                // Salva o novo usuário no repositório
                await _userRepository.AddUserAsync(newUser);

                return newUser;
            }
            catch (ArgumentException)
            {
                throw; // Propaga a exceção para que ela seja tratada no controlador
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao registrar usuário. Detalhes: " + ex.Message);
            }
        }
    }
}
