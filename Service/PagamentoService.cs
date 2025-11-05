using System.Linq;
using financeira.Controller.DTO;
using financeira.Exceptions;
using financeira.Repository;
using Financeira.Model;
using Financeira.Model.Enums;
using Financeira.Repository;

namespace financeira.Service
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IContratoRepository _contratoRepository;

        public PagamentoService(
            IPagamentoRepository pagamentoRepository,
            IContratoRepository contratoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _contratoRepository = contratoRepository;
        }

        public async Task SalvarPagamentoContratoAsync(Pagamento pagamento, Contrato contrato)
        {
            pagamento.StatusPagamento = ValidarStatusPagamento(pagamento, contrato);

            await _pagamentoRepository.SaveAsync(pagamento);
        }

        public async Task<ResumoContratoDTO> ObterPagamentoPorContratoAsync(Guid idContrato)
        {
            var contrato = await _contratoRepository.GetByIdAsync(idContrato);
            if (contrato is null)
                throw new KeyNotFoundException("Contrato não encontrado");

            var pagamentos = await _pagamentoRepository.FindByContratoIdAsync(idContrato);

            var totalPago = pagamentos.Sum(p => p.ValorPago);
            var saldoDevedor = contrato.ValorTotal - totalPago;

            var pagamentoDTOs = pagamentos
                .Select(p => new PagamentoResponseDTO(
                    p.DataPagamento,
                    p.ValorPago,
                    p.StatusPagamento))
                .ToList();

            return new ResumoContratoDTO(
                contrato.Id,
                contrato.ValorTotal,
                totalPago,
                saldoDevedor,
                pagamentoDTOs
            );
        }

        private StatusPagamento ValidarStatusPagamento(Pagamento pagamento, Contrato contrato)
        {
            var diaPagamento = pagamento.DataPagamento.Day;
            var diaVencimento = contrato.DataVencimentoPrimeiraParcela.Day;

            if (diaPagamento < diaVencimento)
                return StatusPagamento.ANTECIPADO;
            else if (diaPagamento == diaVencimento)
                return StatusPagamento.EM_DIA;
            else
                return StatusPagamento.ATRASADO;
        }
    }
}
