using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using financeira.Exceptions;
using Financeira.Util;
using Microsoft.Extensions.Logging;
using Moq;

namespace financeiro.tests.Validation
{
    public class ValidadorTests
    {
        private readonly Mock<ILogger> _loggerMock;

        public ValidadorTests()
        {
            _loggerMock = new Mock<ILogger>();
        }

        [Theory(DisplayName = "Deve validar CPF/CNPJ válido sem lançar exceção")]
        [InlineData("529.982.247-25")]    // CPF válido
        [InlineData("04.252.011/0001-10")] // CNPJ válido
        public void ValidarCpfCnpj_DeveRetornarTrue_QuandoValido(string valor)
        {
            // Act
            var result = Validador.ValidarCpfCnpj(valor, _loggerMock.Object);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Deve lançar exceção mesmo sem logger")]
        public void ValidarCpfCnpj_DeveLancarExcecao_SemLogger()
        {
            string valorInvalido = "123";

            var ex = Assert.Throws<OperacaoNaoPermitidaException>(() =>
                Validador.ValidarCpfCnpj(valorInvalido));

            Assert.Equal("CPF ou CNPJ inválido", ex.Message);
        }
    }
}
