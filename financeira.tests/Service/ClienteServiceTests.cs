using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeira.Repository;
using financeira.Service;
using Financeira.Model;
using Financeira.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace financeiro.tests.Service
{
    public class ClienteServiceTests
    {
        private readonly Mock<IContratoRepository> _contratoRepoMock;
        private readonly Mock<IPagamentoRepository> _pagamentoRepoMock;
        private readonly Mock<ILogger<ClienteService>> _loggerMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _contratoRepoMock = new Mock<IContratoRepository>();
            _pagamentoRepoMock = new Mock<IPagamentoRepository>();
            _loggerMock = new Mock<ILogger<ClienteService>>();
            _clienteService = new ClienteService(_pagamentoRepoMock.Object, _contratoRepoMock.Object, _loggerMock.Object);
        }

        [Fact(DisplayName = "Deve lançar exceção quando cliente não possui contratos")]
        public async Task ObterResumoClienteAsync_DeveLancarExcecao_QuandoNaoExistemContratos()
        {
            string cpfCnpj = "12345678900";
            _contratoRepoMock.Setup(r => r.GetByClienteCpfCnpjAsync(cpfCnpj))
                             .ReturnsAsync(new List<Contrato>());

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _clienteService.ObterResumoClienteAsync(cpfCnpj));
        }

        [Fact(DisplayName = "Deve calcular resumo corretamente quando não há pagamentos")]
        public async Task ObterResumoClienteAsync_DeveCalcularResumoCorretamente_QuandoHaContratosSemPagamentos()
        {
            string cpfCnpj = "12345678900";
            var contrato = new Contrato
            {
                PrazoMeses = 12,
                ValorTotal = 1200m,
                DataVencimentoPrimeiraParcela = new DateTime(2023, 1, 5),
                Pagamentos = new List<Pagamento>()
            };

            _contratoRepoMock.Setup(r => r.GetByClienteCpfCnpjAsync(cpfCnpj))
                             .ReturnsAsync(new List<Contrato> { contrato });

            var resumo = await _clienteService.ObterResumoClienteAsync(cpfCnpj);

            Assert.Equal(1, resumo.ContratosAtivos);
            Assert.Equal(12, resumo.TotalParcelas);
            Assert.Equal(0, resumo.ParcelasPagas);
            Assert.Equal(0, resumo.ParcelasAtrasadas);
            Assert.Equal(12, resumo.ParcelasAVencer);
            Assert.Equal(0, resumo.PercentualPagasEmDia);
            Assert.Equal(1200m, resumo.SaldoDevedor);
        }

        [Fact(DisplayName = "Deve calcular parcelas em dia e atrasadas corretamente")]
        public async Task ObterResumoClienteAsync_DeveCalcularPagamentosEmDiaEAtrasadosCorretamente()
        {
            string cpfCnpj = "12345678900";
            var contrato = new Contrato
            {
                PrazoMeses = 2,
                ValorTotal = 200m,
                DataVencimentoPrimeiraParcela = new DateTime(2023, 1, 10),
                Pagamentos = new List<Pagamento>
                {
                    new Pagamento { DataPagamento = new DateTime(2023, 1, 9), ValorPago = 100m },
                    new Pagamento { DataPagamento = new DateTime(2023, 2, 15), ValorPago = 100m }
                }
            };

            _contratoRepoMock.Setup(r => r.GetByClienteCpfCnpjAsync(cpfCnpj))
                             .ReturnsAsync(new List<Contrato> { contrato });

            var resumo = await _clienteService.ObterResumoClienteAsync(cpfCnpj);

            Assert.Equal(1, resumo.ContratosAtivos);
            Assert.Equal(2, resumo.TotalParcelas);
            Assert.Equal(2, resumo.ParcelasPagas);
            Assert.Equal(1, resumo.ParcelasAtrasadas);
            Assert.Equal(0, resumo.ParcelasAVencer);
            Assert.Equal(50, resumo.PercentualPagasEmDia);
            Assert.Equal(0m, resumo.SaldoDevedor);
        }

        [Fact(DisplayName = "Deve calcular resumo corretamente com múltiplos contratos")]
        public async Task ObterResumoClienteAsync_DeveCalcularResumoComMultiplosContratos()
        {
            string cpfCnpj = "12345678900";
            var contratos = new List<Contrato>
            {
                new Contrato
                {
                    PrazoMeses = 3,
                    ValorTotal = 300m,
                    DataVencimentoPrimeiraParcela = new DateTime(2023, 1, 5),
                    Pagamentos = new List<Pagamento>
                    {
                        new Pagamento { DataPagamento = new DateTime(2023, 1, 4), ValorPago = 100m },
                        new Pagamento { DataPagamento = new DateTime(2023, 2, 6), ValorPago = 100m }
                    }
                },
                new Contrato
                {
                    PrazoMeses = 2,
                    ValorTotal = 200m,
                    DataVencimentoPrimeiraParcela = new DateTime(2023, 1, 10),
                    Pagamentos = new List<Pagamento>
                    {
                        new Pagamento { DataPagamento = new DateTime(2023, 1, 15), ValorPago = 100m }
                    }
                }
            };

            _contratoRepoMock.Setup(r => r.GetByClienteCpfCnpjAsync(cpfCnpj))
                             .ReturnsAsync(contratos);

            var resumo = await _clienteService.ObterResumoClienteAsync(cpfCnpj);

            Assert.Equal(2, resumo.ContratosAtivos);
            Assert.Equal(5, resumo.TotalParcelas);
            Assert.Equal(3, resumo.ParcelasPagas);
            Assert.Equal(2, resumo.ParcelasAtrasadas);
            Assert.Equal(2, resumo.ParcelasAVencer);
            Assert.Equal(33.33, Math.Round(resumo.PercentualPagasEmDia, 2));
            Assert.Equal(300m + 200m - (100m + 100m + 100m), resumo.SaldoDevedor);
        }

        [Fact(DisplayName = "Deve calcular percentual em dia como zero quando não há pagamentos")]
        public async Task ObterResumoClienteAsync_DeveCalcularPercentualPagasEmDia_QuandoNaoHaPagamentos()
        {
            string cpfCnpj = "00000000000";
            var contrato = new Contrato
            {
                PrazoMeses = 6,
                ValorTotal = 600m,
                DataVencimentoPrimeiraParcela = new DateTime(2023, 1, 5),
                Pagamentos = null
            };

            _contratoRepoMock.Setup(r => r.GetByClienteCpfCnpjAsync(cpfCnpj))
                             .ReturnsAsync(new List<Contrato> { contrato });

            var resumo = await _clienteService.ObterResumoClienteAsync(cpfCnpj);

            Assert.Equal(0, resumo.ParcelasPagas);
            Assert.Equal(0, resumo.ParcelasAtrasadas);
            Assert.Equal(6, resumo.ParcelasAVencer);
            Assert.Equal(0, resumo.PercentualPagasEmDia);
            Assert.Equal(600m, resumo.SaldoDevedor);
        }

        [Fact(DisplayName = "Deve calcular saldo devedor corretamente com pagamentos parciais")]
        public async Task ObterResumoClienteAsync_DeveCalcularSaldoDevedor_QuandoPagamentosParciais()
        {
            string cpfCnpj = "99999999999";
            var contrato = new Contrato
            {
                PrazoMeses = 3,
                ValorTotal = 300m,
                DataVencimentoPrimeiraParcela = new DateTime(2023, 1, 5),
                Pagamentos = new List<Pagamento>
                {
                    new Pagamento { DataPagamento = new DateTime(2023, 1, 5), ValorPago = 50m },
                    new Pagamento { DataPagamento = new DateTime(2023, 2, 5), ValorPago = 50m }
                }
            };

            _contratoRepoMock.Setup(r => r.GetByClienteCpfCnpjAsync(cpfCnpj))
                             .ReturnsAsync(new List<Contrato> { contrato });

            var resumo = await _clienteService.ObterResumoClienteAsync(cpfCnpj);

            Assert.Equal(2, resumo.ParcelasPagas);
            Assert.Equal(1, resumo.ParcelasAVencer);
            Assert.Equal(200m, resumo.SaldoDevedor);
        }
    }
}