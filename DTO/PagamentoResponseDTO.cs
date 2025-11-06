using System.Text.Json.Serialization;
using Financeira.Model.Enums;

namespace financeira.Controller.DTO
{
    public record PagamentoResponseDTO(
        DateTime dataPagamento,
        decimal ValorPago,
        StatusPagamento Status
    )
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusPagamento Status { get; init; } = Status;
    }
}

