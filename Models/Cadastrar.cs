using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Ecouni_Projeto.Models
{
    public class Cadastrar
    {
        public int Cadastrarid { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public decimal Email { get; set; }

        [Required]
        public int Telefone { get; set; }

        [Required]
        public decimal Senha { get; set; }

        [Required]
        public decimal ConfirmarSenha { get; set; }
    }
}
