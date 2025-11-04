namespace financeira.Controller.DTO
{
    public record ResumoContratoDTO(
        Guid ContratoId,
        decimal ValorTotal,
        decimal TotalPago,
        decimal SaldoDevedor,
        List<PagamentoResponseDTO> Pagamentos
    );
}
