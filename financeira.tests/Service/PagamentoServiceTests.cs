using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using financeira.Repository;
using financeira.Service;
using Financeira.Model;
using Financeira.Model.Enums;
using Financeira.Repository;
using Moq;

namespace financeiro.tests.Service
{
    public class PagamentoServiceTests
    {
        private readonly Mock<IPagamentoRepository> _pagamentoRepositoryMock;
        private readonly Mock<IContratoRepository> _contratoRepositoryMock;
        private readonly PagamentoService _service;

        public PagamentoServiceTests()
        {
            _pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            _contratoRepositoryMock = new Mock<IContratoRepository>();

            _service = new PagamentoService(
                _pagamentoRepositoryMock.Object,
                _contratoRepositoryMock.Object
            );
        }


        [Fact(DisplayName = "Deve salvar pagamento com status antecipado")]
        public async Task SalvarPagamentoContratoAsync_DeveDefinirStatus_Antecipado()
        {

            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ValorTotal = 1000m,
                DataVencimentoPrimeiraParcela = new DateTime(2025, 11, 10)
            };

            var pagamento = new Pagamento
            {
                Id = Guid.NewGuid(),
                ValorPago = 200m,
                DataPagamento = new DateTime(2025, 11, 5)
            };

            await _service.SalvarPagamentoContratoAsync(pagamento, contrato);

            Assert.Equal(StatusPagamento.ANTECIPADO, pagamento.StatusPagamento);
            _pagamentoRepositoryMock.Verify(r => r.SaveAsync(pagamento), Times.Once);
        }

        [Fact(DisplayName = "Deve salvar pagamento com status em dia")]
        public async Task SalvarPagamentoContratoAsync_DeveDefinirStatus_EmDia()
        {
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ValorTotal = 1000m,
                DataVencimentoPrimeiraParcela = new DateTime(2025, 11, 5)
            };

            var pagamento = new Pagamento
            {
                Id = Guid.NewGuid(),
                ValorPago = 200m,
                DataPagamento = new DateTime(2025, 11, 5)
            };

            await _service.SalvarPagamentoContratoAsync(pagamento, contrato);

            Assert.Equal(StatusPagamento.EM_DIA, pagamento.StatusPagamento);
            _pagamentoRepositoryMock.Verify(r => r.SaveAsync(pagamento), Times.Once);
        }

        [Fact(DisplayName = "Deve salvar pagamento com status atrasado")]
        public async Task SalvarPagamentoContratoAsync_DeveDefinirStatus_Atrasado()
        {
            // Arrange
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ValorTotal = 1000m,
                DataVencimentoPrimeiraParcela = new DateTime(2025, 11, 5)
            };

            var pagamento = new Pagamento
            {
                Id = Guid.NewGuid(),
                ValorPago = 200m,
                DataPagamento = new DateTime(2025, 11, 10)
            };

            // Act
            await _service.SalvarPagamentoContratoAsync(pagamento, contrato);

            // Assert
            Assert.Equal(StatusPagamento.ATRASADO, pagamento.StatusPagamento);
            _pagamentoRepositoryMock.Verify(r => r.SaveAsync(pagamento), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar exceção se contrato não for encontrado")]
        public async Task ObterPagamentoPorContratoAsync_ContratoNaoEncontrado_DeveLancarExcecao()
        {
            // Arrange
            _contratoRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Contrato)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.ObterPagamentoPorContratoAsync(Guid.NewGuid())
            );
        }

        [Fact(DisplayName = "Deve retornar resumo do contrato com totais corretos")]
        public async Task ObterPagamentoPorContratoAsync_DeveRetornarResumoCorreto()
        {
            // Arrange
            var contratoId = Guid.NewGuid();
            var contrato = new Contrato
            {
                Id = contratoId,
                ValorTotal = 1000m,
                DataVencimentoPrimeiraParcela = DateTime.Now
            };

            var pagamentos = new List<Pagamento>
            {
                new Pagamento { ValorPago = 300m, DataPagamento = DateTime.Now, StatusPagamento = StatusPagamento.EM_DIA },
                new Pagamento { ValorPago = 200m, DataPagamento = DateTime.Now, StatusPagamento = StatusPagamento.ANTECIPADO }
            };

            _contratoRepositoryMock.Setup(r => r.GetByIdAsync(contratoId))
                .ReturnsAsync(contrato);

            _pagamentoRepositoryMock.Setup(r => r.FindByContratoIdAsync(contratoId))
                .ReturnsAsync(pagamentos);

            // Act
            var resumo = await _service.ObterPagamentoPorContratoAsync(contratoId);

            // Assert
            Assert.NotNull(resumo);
            Assert.Equal(500m, resumo.TotalPago);
            Assert.Equal(500m, resumo.SaldoDevedor);
            Assert.Equal(2, resumo.Pagamentos.Count);
        }

    }
}
