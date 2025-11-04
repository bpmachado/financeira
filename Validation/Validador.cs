using System;
using Microsoft.Extensions.Logging;
using Financeira.Validators;

namespace Financeira.Util
{
    public static class Validador
    {
        private static readonly CpfCnpjValidator _validator = new CpfCnpjValidator();

        public static void ValidarCpfCnpj(string cpfCnpj, ILogger logger = null)
        {
            if (!_validator.IsValidCpfCnpj(cpfCnpj))
            {
                logger?.LogWarning("CpfCnpj inválido");
                throw new Exception("CPF ou CNPJ inválido");
            }
        }
    }
}
