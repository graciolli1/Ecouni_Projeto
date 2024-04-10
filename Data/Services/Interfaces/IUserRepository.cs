using System.Threading.Tasks;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<Cadastrar> GetUserByEmailAsync(string email);
        Task<Cadastrar> GetUserByPhoneAsync(string telefone); // Adicione este método
        Task AddUserAsync(Cadastrar user);
    }
}
