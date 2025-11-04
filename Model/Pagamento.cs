using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Financeira.Model.Enums;

namespace Financeira.Model
{
    [Table("Pagamento")]
    public class Pagamento
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime DataPagamento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorPago { get; set; }

        [Required]
        public StatusPagamento StatusPagamento { get; set; }

        [ForeignKey("ContratoId")]
        public Contrato Contrato { get; set; }

        public Guid ContratoId { get; set; }
    }
}
