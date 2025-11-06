using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using financeira.Controller.DTO;
using financeira.Service;
using Financeira.Model;
using Financeira.Repository;
using Microsoft.EntityFrameworkCore;

namespace Financeira.Service
{
    public class ContratoService : IContratoService
    {
        private readonly IContratoRepository _contratoRepository;

        public ContratoService(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public async Task CriarContratoAsync(Contrato contrato)
        {
            await _contratoRepository.AddAsync(contrato);
        }

        public async Task<Contrato?> ObterContratoPorIdAsync(Guid id)
        {
            return await _contratoRepository.GetByIdAsync(id);
        }

        public async Task DeletarContratoAsync(Contrato contrato)
        {
            await _contratoRepository.DeleteAsync(contrato);
        }

        public async Task<PagedResult<Contrato>> BuscarPorCpfCnpjAsync(string cpfCnpj, int pageNumber, int pageSize)
        {
            var query = _contratoRepository.Query()
                .Where(c => c.ClienteCpfCnpj == cpfCnpj);

            var totalItems = await query.CountAsync();

            var contratos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Contrato>
            {
                Items = contratos,
                Page = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
        }
    }
}