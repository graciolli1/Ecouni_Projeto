using System.ComponentModel.DataAnnotations;

namespace Ecouni_Projeto.Models
{
    public class Contato
    {
        public int ContatoId { get; set; }

        public int Nome { get; set; }
        public decimal Email { get; set; }

        [StringLength(1000)]
        public decimal Mensagem { get; set; }
    }
}
