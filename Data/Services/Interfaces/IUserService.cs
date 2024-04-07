using System;
using System.Threading.Tasks;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IUserService
    {
        Task<Cadastrar> AuthenticateAsync(string username, string password);
        Task<Cadastrar> RegisterAsync(string fullName, string email, string phone, string password);
    }
}
