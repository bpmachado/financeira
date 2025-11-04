using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Financeira.Model.Enums;

namespace Financeira.Model
{
    [Table("pagamento")]
    public class Pagamento
    {
        public Guid Id { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public string StatusPagamento { get; set; }
        public Guid ContratoId { get; set; }
        public Contrato Contrato { get; set; }
    }
}
