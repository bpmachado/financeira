using financeira.Repository;
using Financeira.Model;
using Financeira.Model.DTO;
using Financeira.Repository;

namespace financeira.Service
{
    public class ClienteService : IClienteService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(
            IPagamentoRepository pagamentoRepository,
            IContratoRepository contratoRepository,
            ILogger<ClienteService> logger)
        {
            _pagamentoRepository = pagamentoRepository;
            _contratoRepository = contratoRepository;
            _logger = logger;
        }

        public async Task<ResumoClienteDTO> ObterResumoClienteAsync(string cpfCnpj)
        {
            _logger.LogInformation("Serviço para obter resumo do cliente: {CpfCnpj}", cpfCnpj);

            int contratosAtivos = 0;
            int totalParcelas = 0;
            int parcelasPagas = 0;
            int parcelasAtrasadas = 0;
            int parcelasAVencer = 0;
            int parcelasEmDia = 0;
            decimal saldoDevedor = 0m;

            var contratos = await _contratoRepository.GetByClienteCpfCnpjAsync(cpfCnpj);

            if (!contratos.Any())
            {
                throw new KeyNotFoundException();
            }

            contratosAtivos = contratos.Count;

            foreach (var contrato in contratos)
            {
                int prazo = contrato.PrazoMeses;
                totalParcelas += prazo;

                var pagamentos = contrato.Pagamentos ?? new List<Pagamento>();
                parcelasPagas += pagamentos.Count;

                int pagamentoIndex = 0;
                foreach (var pagamento in pagamentos)
                {
                    var vencimento = contrato.DataVencimentoPrimeiraParcela.Day;

                    if (pagamento.DataPagamento.Day > vencimento)
                        parcelasAtrasadas++;
                    else
                        parcelasEmDia++;

                    pagamentoIndex++;
                }

                parcelasAVencer += Math.Max(0, contrato.PrazoMeses - pagamentos.Count);

                decimal totalPago = pagamentos.Sum(p => p.ValorPago);
                saldoDevedor += contrato.ValorTotal - totalPago;
            }

            double percentualPagasEmDia = parcelasPagas > 0
                ? (double)parcelasEmDia / parcelasPagas * 100
                : 0;

            return new ResumoClienteDTO(
                cpfCnpj,
                contratosAtivos,
                totalParcelas,
                parcelasPagas,
                parcelasAtrasadas,
                parcelasAVencer,
                percentualPagasEmDia,
                saldoDevedor
            );
        }
    }
}
