using System.Net;
using financeira.Controller.DTO;
using financeira.Exceptions;
using Financeira.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

// Adicione a diretiva correta para o tipo Exception do .NET
using System;
using Microsoft.AspNetCore.Http.HttpResults; // <-- Adiciona o tipo Exception

namespace financeira.Controller.Common
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        private static readonly Dictionary<Type, Func<Exception, (ErroResposta resposta, int status)>> ExceptionMap =
        new()
        {
            [typeof(CampoInvalidoException)] = ex =>
            {
                var e = (CampoInvalidoException)ex;
                var resposta = new ErroResposta(422, "Erro de validação", new()
                {
                    new ErroCampo(e.Campo, e.Message)
                });
                return (resposta, 422);
            },

            [typeof(RegistroDuplicadoException)] = ex =>
            {
                var resposta = ErroResposta.Conflito(ex.Message);
                return (resposta, 409);
            },

            [typeof(OperacaoNaoPermitidaException)] = ex =>
            {
                var resposta = ErroResposta.RespostaPadrao(ex.Message);
                return (resposta, 400);
            },

            [typeof(UnauthorizedAccessException)] = ex =>
            {
                var resposta = new ErroResposta(403, "Acesso negado", new());
                return (resposta, 403);
            },

            [typeof(KeyNotFoundException)] = _ =>
            {
                var resposta = new ErroResposta(404, "O recurso não foi encontrado", new());
                return (resposta, 404);
            }
        };

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            ErroResposta resposta;
            int status;

            if (ExceptionMap.TryGetValue(exception.GetType(), out var handler))
            {
                (resposta, status) = handler(exception);
                _logger.LogWarning("Erro tratado: {Tipo} - {Mensagem}", exception.GetType().Name, exception.Message);
            }
            else
            {
                _logger.LogError(exception, "Erro inesperado");
                resposta = new ErroResposta(500, "Ocorreu um erro inesperado, entre em contato com o administrador!", new());
                status = 500;
            }

            context.Result = new ObjectResult(resposta) { StatusCode = status };
            context.ExceptionHandled = true;
        }
    }
}
