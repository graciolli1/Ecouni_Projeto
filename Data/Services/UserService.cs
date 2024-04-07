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

        public async Task<Cadastrar> AuthenticateAsync(string email, string password)
        {
            try
            {
                // Busca o usuário pelo nome de usuário (ou outro campo de identificação, como email)
                var user = await _userRepository.GetUserByEmailAsync(email);

                // Verifica se o usuário foi encontrado e se a senha corresponde
                if (user != null && VerifyPassword(password, user.Senha))
                {
                    // Autenticação bem-sucedida, retorna o usuário
                    return user;
                }

                // Se não houver correspondência de usuário ou senha, retorna default (null)
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao autenticar usuário. Detalhes: " + ex.Message);
            }
        }

        public async Task<Cadastrar> RegisterAsync(string fullName, string email, string phone, string password)
        {
            try
            {
                // Verifica se o email já está cadastrado
                var existingUser = await _userRepository.GetUserByEmailAsync(email);
                if (existingUser != null)
                {
                    throw new ArgumentException("O email fornecido já está em uso.");
                }

                // Verifica se o telefone já está cadastrado
                existingUser = await _userRepository.GetUserByPhoneAsync(phone);
                if (existingUser != null)
                {
                    throw new ArgumentException("O telefone fornecido já está em uso.");
                }

                // Cria um novo usuário com os dados fornecidos
                var newUser = new Cadastrar
                {
                    Nome = fullName,
                    Email = email,
                    Telefone = phone,
                    Senha = HashPassword(password) // Hash da senha antes de salvar o usuário
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

        // Método para verificar a senha (exemplo simples)
        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Comparação do hash da senha fornecida com o hash armazenado no banco de dados
            return HashPassword(password) == hashedPassword;
        }

        // Método para hash da senha (exemplo simples, substitua pela lógica adequada)
        private string HashPassword(string password)
        {
            // Aqui você pode implementar a lógica real de hashing da senha, como usando BCrypt, PBKDF2, etc.
            // Por simplicidade, este é apenas um exemplo básico de hash usando SHA256
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
