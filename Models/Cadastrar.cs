using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ecouni_Projeto.Models
{
    public class Cadastrar
    {
        public int Cadastrarid { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        public string Email { get; set; }

        private int _telefone; // Armazena o telefone como um inteiro

        [Required]
        public string Telefone
        {
            get { return _telefone.ToString(); } // Retorna o telefone como uma string
            set
            {
                // Remove todos os caracteres não numéricos
                string digitsOnly = Regex.Replace(value, @"[^\d]", "");

                // Se o número de dígitos for igual a 11, converte para int e atribui ao campo _telefone
                if (digitsOnly.Length == 11 && int.TryParse(digitsOnly, out int result))
                {
                    _telefone = result;
                }
                else
                {
                    throw new ArgumentException("Telefone inválido.");
                }
            }
        }

        [Required]
        public string Senha { get; set; }

        [Required]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmarSenha { get; set; }
    }
}
