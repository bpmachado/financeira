using Microsoft.AspNetCore.Mvc;

namespace financeira.Controllers
{
    using financeira.Controller.DTO;
    using financeira.Controller.Mappers;
    using financeira.Exceptions;
    using financeira.Service;
    using financeira.Util;
    using global::Financeira.DTO;
    using global::Financeira.Util;
    using Microsoft.AspNetCore.Authorization;
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
            [Authorize]
            public async Task<IActionResult> CriarContrato([FromBody] ContratoDTO dto)
            {
                _logger.LogInformation("Cadastrando um novo contrato para o cliente com CPF ou CNPJ: {CpfCnpj}", dto.ClienteCpfCnpj);

               Validador.ValidarCpfCnpj(dto.ClienteCpfCnpj);

                var contrato = _contratoMapper.ToEntity(dto);
                await _contratoService.CriarContratoAsync(contrato);

                var location = UriHelper.GerarHeaderLocation(Request, contrato.Id);
                return Created(location, null);
            }

            [HttpGet("{id}")]
            [SwaggerOperation(Summary = "Obter contrato por id", Description = "Retorna os dados do contrato por ID")]
            [SwaggerResponse(200, "Contrato encontrado com sucesso.", typeof(ContratoDTO))]
            [SwaggerResponse(404, "Contrato não encontrado.")]
            [Authorize]
            public async Task<IActionResult> ObterContratoPorId(string id)
            {
                _logger.LogInformation("Obter um novo contrato por id: {Id}", id);

                if (!Guid.TryParse(id, out var idContrato))
                    throw new OperacaoNaoPermitidaException("ID inválido.");

                var contrato = await _contratoService.ObterContratoPorIdAsync(idContrato);

                if (contrato is null)
                    throw new KeyNotFoundException();

                var dto = _contratoMapper.ToDto(contrato);
                return Ok(dto);
            }

            [HttpGet]
            [SwaggerOperation(Summary = "Obter contrato por CPF ou CNPJ com paginação", Description = "Retorna os dados do contrato por CPF ou CNPJ")]
            [SwaggerResponse(200, "Contrato encontrado com sucesso.", typeof(PagedResult<ContratoDTO>))]
            [SwaggerResponse(404, "Contrato não encontrado.")]
            [Authorize]
            public async Task<IActionResult> ObterContratosPorCpfCnpj(
                [FromQuery] string? cpfCnpj,
                [FromQuery] int page = 1,
                [FromQuery] int size = 10)
            {
                _logger.LogInformation("Obter contrato por CPF ou CNPJ: {CpfCnpj}", cpfCnpj);

                Validador.ValidarCpfCnpj(cpfCnpj);

                var resultado = await _contratoService.BuscarPorCpfCnpjAsync(cpfCnpj, page, size);

                if (resultado == null || !resultado.Items.Any())
                    throw new KeyNotFoundException();

                var dtoResult = new PagedResult<ContratoDTO>
                {
                    Items = resultado.Items?.Select(_contratoMapper.ToDto)
                        .Where(dto => dto != null)
                        .Cast<ContratoDTO>()
                        .ToList() ?? new List<ContratoDTO>(),
                    Page = resultado.Page,
                    PageSize = resultado.PageSize,
                    TotalItems = resultado.TotalItems
                };

                return Ok(dtoResult);
            }

            [HttpDelete("{id}")]
            [SwaggerOperation(Summary = "Deletar", Description = "Deletar os dados do contrato")]
            [SwaggerResponse(204, "Contrato deletado com sucesso.")]
            [SwaggerResponse(404, "Contrato não encontrado.")]
            [SwaggerResponse(400, "Contrato possui pagamento cadastrado.")]
            [Authorize]
            public async Task<IActionResult> DeletarContrato(string id)
            {
                _logger.LogInformation("Deletando um contrato por id: {Id}", id);

                if (!Guid.TryParse(id, out var idContrato))
                    throw new OperacaoNaoPermitidaException("ID inválido.");

                var contrato = await _contratoService.ObterContratoPorIdAsync(idContrato);

                if (contrato is null)
                    throw new KeyNotFoundException();

                // Regra de negócio: não permitir deletar se houver pagamentos
                if (contrato.Pagamentos != null && contrato.Pagamentos.Any())
                    throw new OperacaoNaoPermitidaException("Não permitir deletar um contrato se tiver um pagamento");

                await _contratoService.DeletarContratoAsync(contrato);
                return NoContent();
            }

        }
    }
}

