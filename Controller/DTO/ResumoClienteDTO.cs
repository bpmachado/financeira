namespace Financeira.Model.DTO
{
    public record ResumoClienteDTO(
        string ClienteCpfCnpj,
        int ContratosAtivos,
        int TotalParcelas,
        int ParcelasPagas,
        int ParcelasAtrasadas,
        int ParcelasAVencer,
        double PercentualPagasEmDia,
        decimal SaldoDevedor
    );
}