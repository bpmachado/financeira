using System.Collections.Generic;
using System.Net;

namespace financeira.Controller.DTO
{
    public record ErroResposta(int Status, string Mensagem, List<ErroCampo> Erros)
    {
        public static ErroResposta RespostaPadrao(string mensagem) =>
            new((int)HttpStatusCode.BadRequest, mensagem, new());

        public static ErroResposta Conflito(string mensagem) =>
            new((int)HttpStatusCode.Conflict, mensagem, new());
    }
}
