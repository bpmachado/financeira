using Financeira.Model;

namespace financeira.Repository
{
    public interface IPagamentoRepository
    {
        Task<Pagamento> SaveAsync(Pagamento pagamento);
        Task<List<Pagamento>> FindByContratoIdAsync(Guid contratoId);
        Task<Pagamento?> FindByIdAsync(Guid id);
    }
}
