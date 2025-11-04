using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Financeira.Model;

namespace Financeira.Repository
{
    public interface IContratoRepository
    {
        Task<Contrato?> GetByIdAsync(Guid id);
        Task<List<Contrato>> GetByClienteCpfCnpjAsync(string cpfCnpj, int pageNumber, int pageSize);
        Task<List<Contrato>> GetByClienteCpfCnpjAsync(string cpfCnpj);
        Task AddAsync(Contrato contrato);
        Task DeleteAsync(Contrato contrato);
        IQueryable<Contrato> Query();
    }
}
