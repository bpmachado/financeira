using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Financeira.Model.Enums;

namespace Financeira.Model
{
    [Table("pagamento")]
    public class Pagamento
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("data_pagamento", TypeName = "date")]
        public DateTime DataPagamento { get; set; }

        [Required]
        [Column("valor_pago", TypeName = "numeric(15,2)")]
        public decimal ValorPago { get; set; }

        [Required]
        [Column("status_pagamento")]
        [MaxLength(20)]
        public StatusPagamento StatusPagamento { get; set; }

        [Required]
        [Column("contrato_id")]
        public Guid ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public Contrato Contrato { get; set; } = null!;
    }
}
