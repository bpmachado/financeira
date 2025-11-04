namespace financeira.Exceptions
{
    using System;

    public class CampoInvalidoException : Exception
    {
        public string Campo { get; }

        public CampoInvalidoException(string campo, string mensagem) : base(mensagem)
        {
            Campo = campo;
        }
    }
}
