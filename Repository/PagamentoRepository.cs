using Financeira.Data;
using Financeira.Model;
using Microsoft.EntityFrameworkCore;

namespace financeira.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly AppDbContext _context;

        public PagamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pagamento> SaveAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();
            return pagamento;
        }

        public async Task<List<Pagamento>> FindByContratoIdAsync(Guid contratoId)
        {
            return await _context.Pagamentos
                                 .Include(p => p.Contrato)
                                 .Where(p => p.Contrato.Id == contratoId)
                                 .ToListAsync();
        }

        public async Task<Pagamento?> FindByIdAsync(Guid id)
        {
            return await _context.Pagamentos
                                 .Include(p => p.Contrato)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
