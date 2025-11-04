namespace financeira.Exceptions
{
    using System;

    public class RegistroDuplicadoException : Exception
    {
        public RegistroDuplicadoException(string mensagem) : base(mensagem)
        {
        }
    }
}
