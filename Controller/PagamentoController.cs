using System;
using financeira.Controller.DTO;
using financeira.Controller.Mappers;
using financeira.Service;
using financeira.Util;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace financeira.Controller
{
    [ApiController]
    [Route("api/v1/contratos")]
    [Tags("Pagamentos")]
    public class PagamentoController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly IPagamentoMapper _pagamentoMapper;
        private readonly IContratoService _contratoService;
        private readonly ILogger<PagamentoController> _logger;

        public PagamentoController(
            IPagamentoService pagamentoService,
            IPagamentoMapper pagamentoMapper,
            IContratoService contratoService,
            ILogger<PagamentoController> logger)
        {
            _pagamentoService = pagamentoService;
            _pagamentoMapper = pagamentoMapper;
            _contratoService = contratoService;
            _logger = logger;
        }

        [HttpPost("{id}/pagamentos")]
        [SwaggerOperation(Summary = "Salvar", Description = "Cadastrar novo pagamento para um contrato")]
        [SwaggerResponse(201, "Cadastro com sucesso.")]
        [SwaggerResponse(422, "Erro de validação.")]
        [SwaggerResponse(409, "Pagamento já cadastrado.")]
        public async Task<IActionResult> CriarPagamentoContrato(Guid id, [FromBody] PagamentoDTO pagamentoDto)
        {
            _logger.LogInformation(
                "Cadastrando um novo pagamento de contrato com {Id}, valor {Valor}, data {Data}",
                pagamentoDto.Id, pagamentoDto.ValorPago, pagamentoDto.DataPagamento
            );

            var contrato = await _contratoService.ObterContratoPorIdAsync(id);
            if (contrato is null)
            {
                return NotFound("Contrato não encontrado");
            }

            var pagamento = _pagamentoMapper.ToEntity(pagamentoDto);
            pagamento.Contrato = contrato;

            await _pagamentoService.SalvarPagamentoContratoAsync(pagamento, contrato);

            var location = UriHelper.GerarHeaderLocation(Request, pagamento.Id);
            return Created(location!, null);
        }
    }
}
