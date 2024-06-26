﻿ using System;
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

        [Required]
        public string Telefone { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmarSenha { get; set; }
    }
}
