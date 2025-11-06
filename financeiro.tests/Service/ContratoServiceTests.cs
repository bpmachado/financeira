using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Financeira.Model;
using Financeira.Repository;
using Financeira.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace financeiro.tests.Service
{
    public class ContratoServiceTests
    {
        private readonly Mock<IContratoRepository> _contratoRepoMock;
        private readonly ContratoService _contratoService;

        public ContratoServiceTests()
        {
            _contratoRepoMock = new Mock<IContratoRepository>();
            _contratoService = new ContratoService(_contratoRepoMock.Object);
        }

        [Fact(DisplayName = "Deve chamar AddAsync ao criar contrato")]
        public async Task CriarContratoAsync_DeveChamarAddAsync()
        {
            var contrato = new Contrato { Id = Guid.NewGuid() };

            _contratoRepoMock.Setup(r => r.AddAsync(contrato))
                             .Returns(Task.CompletedTask)
                             .Verifiable();

            await _contratoService.CriarContratoAsync(contrato);

            _contratoRepoMock.Verify(r => r.AddAsync(contrato), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar contrato quando encontrado por ID")]
        public async Task ObterContratoPorIdAsync_DeveRetornarContrato_QuandoEncontrado()
        {
            var contratoId = Guid.NewGuid();
            var contrato = new Contrato { Id = contratoId };

            _contratoRepoMock.Setup(r => r.GetByIdAsync(contratoId))
                             .ReturnsAsync(contrato);

            var resultado = await _contratoService.ObterContratoPorIdAsync(contratoId);

            Assert.NotNull(resultado);
            Assert.Equal(contratoId, resultado.Id);
        }

        [Fact(DisplayName = "Deve retornar null quando contrato não for encontrado por ID")]
        public async Task ObterContratoPorIdAsync_DeveRetornarNull_QuandoNaoEncontrado()
        {
            var contratoId = Guid.NewGuid();

            _contratoRepoMock.Setup(r => r.GetByIdAsync(contratoId))
                             .ReturnsAsync((Contrato?)null);

            var resultado = await _contratoService.ObterContratoPorIdAsync(contratoId);

            Assert.Null(resultado);
        }

        [Fact(DisplayName = "Deve chamar DeleteAsync ao deletar contrato")]
        public async Task DeletarContratoAsync_DeveChamarDeleteAsync()
        {
            var contrato = new Contrato { Id = Guid.NewGuid() };

            _contratoRepoMock.Setup(r => r.DeleteAsync(contrato))
                             .Returns(Task.CompletedTask)
                             .Verifiable();

            await _contratoService.DeletarContratoAsync(contrato);

            _contratoRepoMock.Verify(r => r.DeleteAsync(contrato), Times.Once);
        }
    }
}
