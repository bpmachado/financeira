using Financeira.Model.Enums;

namespace financeira.Controller.DTO
{
    public record PagamentoResponseDTO(
        DateTime dataPagamento,
        decimal ValorPago,
        StatusPagamento Status
    );
}
