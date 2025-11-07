using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Financeira.Data;
using Financeira.Model;
using Financeira.Repository;
using Microsoft.EntityFrameworkCore;

namespace financeiro.tests.Repository
{
    public class ContratoRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ContratoRepository _repository;

        public ContratoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _repository = new ContratoRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact(DisplayName = "Deve adicionar contrato corretamente")]
        public async Task AddAsync_DeveAdicionarContrato()
        {
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = "12345678900",
                ValorTotal = 1000m
            };

            await _repository.AddAsync(contrato);

            var saved = await _context.Contratos.FindAsync(contrato.Id);
            Assert.NotNull(saved);
            Assert.Equal(contrato.ValorTotal, saved.ValorTotal);
        }

        [Fact(DisplayName = "Deve obter contrato pelo Id")]
        public async Task GetByIdAsync_DeveRetornarContrato()
        {
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = "12345678900",
                ValorTotal = 2000m
            };
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(contrato.Id);

            Assert.NotNull(result);
            Assert.Equal(contrato.ValorTotal, result!.ValorTotal);
        }

        [Fact(DisplayName = "Deve retornar null se contrato não existir")]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExistir()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve obter contratos por CPF/CNPJ com paginação")]
        public async Task GetByClienteCpfCnpjAsync_Paginado_DeveRetornarContratos()
        {
            var contratos = new List<Contrato>
            {
                new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "111", ValorTotal = 100 },
                new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "111", ValorTotal = 200 },
                new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "111", ValorTotal = 300 },
            };
            _context.Contratos.AddRange(contratos);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByClienteCpfCnpjAsync("111", pageNumber: 2, pageSize: 2);

            Assert.Single(result);
            Assert.Equal(300, result[0].ValorTotal);
        }

        [Fact(DisplayName = "Deve obter contratos por CPF/CNPJ incluindo pagamentos")]
        public async Task GetByClienteCpfCnpjAsync_ComPagamentos_DeveRetornarContratos()
        {
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = "222",
                ValorTotal = 500m,
                Pagamentos = new List<Pagamento>
                {
                    new Pagamento { Id = Guid.NewGuid(), ValorPago = 100m, DataPagamento = DateTime.Now }
                }
            };
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByClienteCpfCnpjAsync("222");

            Assert.Single(result);
            Assert.Single(result[0].Pagamentos!);
        }

        [Fact(DisplayName = "Deve deletar contrato corretamente")]
        public async Task DeleteAsync_DeveRemoverContrato()
        {
            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = "333",
                ValorTotal = 700m
            };
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(contrato);

            var exists = await _context.Contratos.FindAsync(contrato.Id);
            Assert.Null(exists);
        }

        [Fact(DisplayName = "Query deve retornar IQueryable de contratos")]
        public void Query_DeveRetornarIQueryable()
        {
            _context.Contratos.Add(new Contrato { Id = Guid.NewGuid(), ClienteCpfCnpj = "444", ValorTotal = 1000m });
            _context.SaveChanges();

            var query = _repository.Query();

            Assert.IsAssignableFrom<IQueryable<Contrato>>(query);
            Assert.Single(query.ToList());
        }
    }
}
