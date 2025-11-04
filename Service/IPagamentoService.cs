using financeira.Controller.DTO;
using Financeira.Model;

namespace financeira.Service
{
    public interface IPagamentoService
    {
        Task SalvarPagamentoContratoAsync(Pagamento pagamento, Contrato contrato);
        Task<ResumoContratoDTO> ObterPagamentoPorContratoAsync(Guid idContrato);
    }
}
