using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class CriarOrdemServicoRequest
    {
        [Required]
        public int IdCliente { get; set; }

        [Required]
        public string DetalhesVenda { get; set; }

        [Required]
        public decimal ValorTotal { get; set; }
    }
}
