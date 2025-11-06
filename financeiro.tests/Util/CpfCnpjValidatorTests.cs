using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Financeira.Validators;

namespace financeiro.tests.Util
{
    public class CpfCnpjValidatorTests
    {
        private readonly CpfCnpjValidator _validator;

        public CpfCnpjValidatorTests()
        {
            _validator = new CpfCnpjValidator();
        }

        [Theory(DisplayName = "Deve validar CPFs válidos")]
        [InlineData("529.982.247-25")]
        [InlineData("12345678909")]
        public void IsValidCpfCnpj_DeveRetornarTrue_ParaCpfValido(string cpf)
        {
            var result = _validator.IsValidCpfCnpj(cpf);

            Assert.True(result);
        }

        [Theory(DisplayName = "Deve invalidar CPFs inválidos")]
        [InlineData("111.111.111-11")]
        [InlineData("123.456.789-00")]
        [InlineData("123")]
        [InlineData("")]
        [InlineData(null)]
        public void IsValidCpfCnpj_DeveRetornarFalse_ParaCpfInvalido(string cpf)
        {
            var result = _validator.IsValidCpfCnpj(cpf);

            Assert.False(result);
        }

        [Theory(DisplayName = "Deve validar CNPJs válidos")]
        [InlineData("04.252.011/0001-10")]
        [InlineData("04252011000110")]
        public void IsValidCpfCnpj_DeveRetornarTrue_ParaCnpjValido(string cnpj)
        {
            var result = _validator.IsValidCpfCnpj(cnpj);

            Assert.True(result);
        }

        [Theory(DisplayName = "Deve invalidar CNPJs inválidos")]
        [InlineData("11.111.111/1111-11")]
        [InlineData("04252011000100")]
        [InlineData("12345678901234")]
        public void IsValidCpfCnpj_DeveRetornarFalse_ParaCnpjInvalido(string cnpj)
        {
            var result = _validator.IsValidCpfCnpj(cnpj);

            Assert.False(result);
        }

        [Theory(DisplayName = "Deve invalidar valores nulos ou em branco")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValidCpfCnpj_DeveRetornarFalse_ParaValoresNulosOuVazios(string value)
        {
            var result = _validator.IsValidCpfCnpj(value);

            Assert.False(result);
        }
    }
}
