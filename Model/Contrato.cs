using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Financeira.Model.Enums;

namespace Financeira.Model
{
    [Table("contrato")]
    public class Contrato
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("cliente_cpf_cnpj")]
        public string ClienteCpfCnpj { get; set; }

        [Required]
        [Column("valor_total", TypeName = "numeric(15,2)")]
        public decimal ValorTotal { get; set; }

        [Required]
        [Column("taxa_mensal", TypeName = "numeric(5,2)")]
        public decimal TaxaMensal { get; set; }

        [Required]
        [Column("prazo_meses")]
        public int PrazoMeses { get; set; }

        [Required]
        [Column("data_vencimento_primeira_parcela")]
        public DateTime DataVencimentoPrimeiraParcela { get; set; }

        [Required]
        [Column("tipo_veiculo")]
        public TipoVeiculo TipoVeiculo { get; set; }

        [Required]
        [Column("condicao_veiculo")]
        public CondicaoVeiculo CondicaoVeiculo { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
