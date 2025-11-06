namespace financeira.Exceptions
{
    using System;
    public class OperacaoNaoPermitidaException : Exception
    {
        public OperacaoNaoPermitidaException(string mensagem) : base(mensagem)
        {
        }
    }
}
