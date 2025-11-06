using financeira.Controller.DTO;
using Financeira.Model;

namespace financeira.Service
{
    public interface IContratoService
    {
        Task CriarContratoAsync(Contrato contrato);
        Task<Contrato?> ObterContratoPorIdAsync(Guid id);
        Task DeletarContratoAsync(Contrato contrato);
        Task<PagedResult<Contrato>> BuscarPorCpfCnpjAsync(string cpfCnpj, int pageNumber, int pageSize);
    }
}
