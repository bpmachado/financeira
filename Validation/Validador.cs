using System;
using financeira.Exceptions;
using Financeira.Validators;
using Microsoft.Extensions.Logging;

namespace Financeira.Util
{
    public static class Validador
    {
        private static readonly CpfCnpjValidator _validator = new CpfCnpjValidator();

        public static bool ValidarCpfCnpj(string cpfCnpj, ILogger? logger = null)
        {
            if (!_validator.IsValidCpfCnpj(cpfCnpj))
            {
                logger?.LogWarning("CpfCnpj inválido");
                throw new OperacaoNaoPermitidaException("CPF ou CNPJ inválido");
            }
            return true;
        }
    }
}
