using Microsoft.AspNetCore.Mvc;

namespace financeira.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Mime;
    using financeira.Controller.Mappers;
    using financeira.Service;
    using financeira.Util;
    using global::Financeira.DTO;
    using global::Financeira.Util;
    using Microsoft.AspNetCore.Mvc;

    namespace Financeira.Api.Controllers.v1
    {
        [ApiController]
        [Route("api/v1/contratos")]
        [Produces(MediaTypeNames.Application.Json)]
        public class ContratoController : ControllerBase
        {
            private readonly IContratoService _contratoService;
            private readonly IContratoMapper _contratoMapper;
            private readonly ILogger<ContratoController> _logger;

            public ContratoController(IContratoService contratoService, IContratoMapper contratoMapper, ILogger<ContratoController> logger)
            {
                _contratoService = contratoService;
                _contratoMapper = contratoMapper;
                _logger = logger;
            }

            /// <summary>
            /// Cadastrar novo contrato
            /// </summary>
            /// <param name="dto">Dados do contrato</param>
            /// <returns>Localização do recurso criado</returns>
            [HttpPost]
            [ProducesResponseType(StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
            [ProducesResponseType(StatusCodes.Status409Conflict)]
            public IActionResult CriarContrato([FromBody] ContratoDTO dto)
            {
                _logger.LogInformation("Cadastrando um novo contrato para o cliente com CPF ou CNPJ: {CpfCnpj}", dto.ClienteCpfCnpj);

                try
                {
                    Validador.ValidarCpfCnpj(dto.ClienteCpfCnpj);
                }
                catch
                {
                    return UnprocessableEntity("CPF ou CNPJ inválido.");
                }

                var contrato = _contratoMapper.ToEntity(dto);
                _contratoService.CriarContratoAsync(contrato);

                var location = UriHelper.GerarHeaderLocation(Request, contrato.Id);
                return Created(location, null);
            }
        }
    }
}

