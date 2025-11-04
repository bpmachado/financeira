using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Financeira.Model.Enums;

namespace Financeira.Model
{
    [Table("Contrato")]
    public class Contrato
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ClienteCpfCnpj { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxaMensal { get; set; }

        [Required]
        public int PrazoMeses { get; set; }

        [Required]
        public DateTime DataVencimentoPrimeiraParcela { get; set; }

        [Required]
        public TipoVeiculo TipoVeiculo { get; set; }

        [Required]
        public CondicaoVeiculo CondicaoVeiculo { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}