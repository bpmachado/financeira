using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Financeira.Model;
using Financeira.Model.Enums;

namespace Financeira.DTO
{
    public class ContratoDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string ClienteCpfCnpj { get; set; }

        [Required(ErrorMessage = "Valor total é obrigatório")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Valor total deve ser maior que zero e ter até 13 dígitos inteiros e 2 decimais")]
        public decimal ValorTotal { get; set; }

        [Required(ErrorMessage = "Taxa mensal é obrigatória")]
        [Range(0.01, 100.00, ErrorMessage = "Taxa mínima é 0.01 e máxima é 100.00")]
        public decimal TaxaMensal { get; set; }

        [Required(ErrorMessage = "Prazo em meses é obrigatório")]
        [Range(1, 360, ErrorMessage = "Prazo mínimo é 1 mês e máximo é 360 meses")]
        public int PrazoMeses { get; set; }

        [Required(ErrorMessage = "Data de vencimento é obrigatória")]
        [DataType(DataType.Date)]
        [FutureOrPresent(ErrorMessage = "A data deve ser hoje ou no futuro")]
        public DateTime DataVencimentoPrimeiraParcela { get; set; }

        [Required(ErrorMessage = "Tipo de veículo é obrigatório")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipoVeiculo TipoVeiculo { get; set; }

        [Required(ErrorMessage = "Condição do veículo é obrigatória")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CondicaoVeiculo CondicaoVeiculo { get; set; }
    }
}
