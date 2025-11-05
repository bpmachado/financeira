using Financeira.Model.DTO;

namespace financeira.Service
{
    public interface IClienteService
    {
        Task<ResumoClienteDTO> ObterResumoClienteAsync(string cpfCnpj);
    }
}
