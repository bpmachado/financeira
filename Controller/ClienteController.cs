using financeira.Service;
using Financeira.Model;
using Financeira.Model.DTO;
using Financeira.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace financeira.Controller
{
    [ApiController]
    [Route("api/v1/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IClienteService clienteService, ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        [HttpGet("{cpfCnpj}/resumo")]
        [SwaggerOperation(
            Summary = "Obter resumo do cliente",
            Description = "Retorna os dados do resumo de cliente por Cpf ou CNPJ"
        )]
        [SwaggerResponse(200, "CPF ou CNPJ encontrado com sucesso.", typeof(ResumoClienteDTO))]
        [SwaggerResponse(404, "CPF ou CNPJ não encontrado.")]
        [Authorize]
        public async Task<IActionResult> ObterResumoCliente(string cpfCnpj)
        {
            _logger.LogInformation("Obter resumo do cliente: {CpfCnpj}", cpfCnpj);

            Validador.ValidarCpfCnpj(cpfCnpj);

            var resumoClienteDto = await _clienteService.ObterResumoClienteAsync(cpfCnpj);

            return Ok(resumoClienteDto);
        }
    }
}
