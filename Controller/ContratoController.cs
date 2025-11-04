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
    using Swashbuckle.AspNetCore.Annotations;

    namespace Financeira.Api.Controllers.v1
    {
        [ApiController]
        [Route("api/v1/contratos")]
        [Tags("Contratos")]
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


            [HttpPost]
            [SwaggerOperation(Summary = "Salvar", Description = "Cadastrar novo contrato")]
            [SwaggerResponse(201, "Cadastro com sucesso.")]
            [SwaggerResponse(422, "Erro de validação.")]
            [SwaggerResponse(409, "Contrato já cadastrado.")]
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

            [HttpGet("{id}")]
            [SwaggerOperation(Summary = "Obter contrato por id", Description = "Retorna os dados do contrato por ID")]
            [SwaggerResponse(200, "Contrato encontrado com sucesso.", typeof(ContratoDTO))]
            [SwaggerResponse(404, "Contrato não encontrado.")]
            public async Task<IActionResult> ObterContratoPorId(string id)
            {
                _logger.LogInformation("Obter um novo contrato por id: {Id}", id);

                if (!Guid.TryParse(id, out var idContrato))
                    return BadRequest("ID inválido.");

                var contrato = await _contratoService.ObterContratoPorIdAsync(idContrato);

                if (contrato is null)
                    return NotFound();

                var dto = _contratoMapper.ToDto(contrato);
                return Ok(dto);
            }


        }
    }
}

