using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using financeira.Repository;
using Financeira.Data;
using Financeira.Model;
using Microsoft.EntityFrameworkCore;

namespace financeiro.tests.Repository
{
    public class PagamentoRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly PagamentoRepository _repository;

        public PagamentoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new PagamentoRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact(DisplayName = "Deve salvar pagamento corretamente")]
        public async Task SaveAsync_DeveSalvarPagamento()
        {
            var contrato = new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "12345678900", ValorTotal = 1000m };
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            var pagamento = new Pagamento
            {
                Id = Guid.NewGuid(),
                ValorPago = 500m,
                DataPagamento = DateTime.UtcNow,
                Contrato = contrato
            };

            var result = await _repository.SaveAsync(pagamento);

            Assert.NotNull(result);
            Assert.Equal(pagamento.Id, result.Id);
            Assert.Equal(pagamento.ValorPago, result.ValorPago);

            var saved = await _context.Pagamentos.FindAsync(pagamento.Id);
            Assert.NotNull(saved);
        }

        [Fact(DisplayName = "Deve encontrar pagamento pelo Id")]
        public async Task FindByIdAsync_DeveRetornarPagamento()
        {
            var contrato = new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "111", ValorTotal = 1000m };
            var pagamento = new Pagamento { Id = Guid.NewGuid(), ValorPago = 200m, DataPagamento = DateTime.UtcNow, Contrato = contrato };
            _context.Contratos.Add(contrato);
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            var result = await _repository.FindByIdAsync(pagamento.Id);

            Assert.NotNull(result);
            Assert.Equal(pagamento.Id, result!.Id);
            Assert.Equal(pagamento.ValorPago, result.ValorPago);
            Assert.NotNull(result.Contrato);
            Assert.Equal(contrato.Id, result.Contrato.Id);
        }

        [Fact(DisplayName = "Deve retornar null se pagamento não existir")]
        public async Task FindByIdAsync_DeveRetornarNull_QuandoNaoExistir()
        {
            var result = await _repository.FindByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve retornar pagamentos por Id do contrato")]
        public async Task FindByContratoIdAsync_DeveRetornarPagamentos()
        {
            var contrato = new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "222", ValorTotal = 1500m };
            var pagamentos = new List<Pagamento>
            {
                new Pagamento { Id = Guid.NewGuid(), ValorPago = 300m, DataPagamento = DateTime.UtcNow, Contrato = contrato },
                new Pagamento { Id = Guid.NewGuid(), ValorPago = 400m, DataPagamento = DateTime.UtcNow, Contrato = contrato }
            };
            _context.Contratos.Add(contrato);
            _context.Pagamentos.AddRange(pagamentos);
            await _context.SaveChangesAsync();

            var result = await _repository.FindByContratoIdAsync(contrato.Id);

            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(contrato.Id, p.Contrato.Id));
        }
    }
}
