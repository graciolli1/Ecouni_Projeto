using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ecouni_Projeto.Models
{
    public class Coleta
    {
        public int Id { get; set; }
        [Required]
        public string TamanhoSaco { get; set; }
        [Required]
        public int Quantidade { get; set; }
        [Required]
        public string Observacoes { get; set; }
        [Required]
        public DateTime DataRegistro { get; set; }
    }
}
