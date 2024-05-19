using System.Threading.Tasks;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<Cadastrar> GetUserByEmailAsync(string email);
        Task AddUserAsync(Cadastrar user);
        Task<Cadastrar> GetUserByIdAsync(int Cadastrarid);
        Task UpdateUserAsync(Cadastrar user);
    }
}
