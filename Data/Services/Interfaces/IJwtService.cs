using System.Threading.Tasks;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateToken(Cadastrar user);
    }
}
