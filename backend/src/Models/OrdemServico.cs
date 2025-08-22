using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Models
{
    public class OrdemServico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CodigoOrdem { get; set; }

        public int IdCliente { get; set; }

        public string DetalhesVenda { get; set; }

        public decimal ValorTotal { get; set; }

        public DateTime DataCriacao { get; set; }
    }
}
