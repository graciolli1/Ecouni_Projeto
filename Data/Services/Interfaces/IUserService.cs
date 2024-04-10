using System;
using System.Threading.Tasks;
using Ecouni_Projeto.Models;

namespace Ecouni_Projeto.Services.Interfaces
{
    public interface IUserService
    {
        Task<Cadastrar> AuthenticateAsync(string email, string senha);
        Task<Cadastrar> RegisterAsync(string nome, string email, string telefone, string senha, string confirmarSenha);
    }
}
