using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ecouni_Projeto.Data;
using Ecouni_Projeto.Models;
using Ecouni_Projeto.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecouni_Projeto.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cadastrar> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Cadastrar.FirstOrDefaultAsync(u => u.Nome == username);

            return user ?? throw new Exception("Usuário não encontrado");
        }

        public async Task<Cadastrar> GetUserByEmailAsync(string email)
        {
            var user = await _context.Cadastrar.FirstOrDefaultAsync(u => u.Email == email);

            return user ?? throw new Exception("Usuário não encontrado");
        }

        public async Task AddUserAsync(Cadastrar user)
        {
            _context.Cadastrar.Add(user);
            await _context.SaveChangesAsync();
        }

        public Task<Cadastrar> GetUserByPhoneAsync(string phone)
        {
            throw new NotImplementedException();
        }
    }
}
