using System.Threading.Tasks;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<Cadastrar> GetUserByEmailAsync(string email);
        Task<Cadastrar> GetUserByPhoneAsync(string telefone); // Adicione este método
        Task<Cadastrar> GetUserByIdAsync(int id); // Adicione este método
        Task AddUserAsync(Cadastrar user);
        Task UpdateUserAsync(Cadastrar user); // Adicione este método
    }
}
