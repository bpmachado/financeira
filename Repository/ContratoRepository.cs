using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Financeira.Data;
using Financeira.Model;
using Microsoft.EntityFrameworkCore;

namespace Financeira.Repository
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly AppDbContext _context;

        public ContratoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Contrato?> GetByIdAsync(Guid id)
        {
            return await _context.Contratos.FindAsync(id);
        }

        public async Task<List<Contrato>> GetByClienteCpfCnpjAsync(string cpfCnpj, int pageNumber, int pageSize)
        {
            return await _context.Contratos
                .Where(c => c.ClienteCpfCnpj == cpfCnpj)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Contrato>> GetByClienteCpfCnpjAsync(string cpfCnpj)
        {
            return await _context.Contratos
                .Where(c => c.ClienteCpfCnpj == cpfCnpj)
                .ToListAsync();
        }

        public async Task AddAsync(Contrato contrato)
        {
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Contrato contrato)
        {
            _context.Contratos.Remove(contrato);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Contrato> Query()
        {
            return _context.Contratos.AsQueryable();
        }
    }
}
