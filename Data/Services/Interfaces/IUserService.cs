using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IUserService
    {
        Task<Cadastrar> AuthenticateAsync(string email, string senha);
        Task<Cadastrar> GetUserByEmailAsync(string email); 
        Task<Cadastrar> GetUserByIdAsync(int Cadastrarid);
        Task<Cadastrar> RegisterAsync(string nome, string email, string telefone, string senha, string confirmarSenha);
        Task UpdateUserAsync(Cadastrar user);
        Task<bool> UserExistsAsync(int Cadastrarid);
    }
}
