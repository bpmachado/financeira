using financeira.Controller.DTO;
using Financeira.Model;

namespace financeira.Controller.Mappers
{
    public class PagamentoMapper : IPagamentoMapper
    {
        public PagamentoDTO? ToDto(Pagamento? entity)
        {
            if (entity is null) return null;

            return new PagamentoDTO
            {
                Id = entity.Id,
                DataPagamento = entity.DataPagamento,
                ValorPago = entity.ValorPago
            };
        }

        public Pagamento? ToEntity(PagamentoDTO? dto)
        {
            if (dto is null) return null;

            return new Pagamento
            {
                Id = dto.Id,
                DataPagamento = dto.DataPagamento,
                ValorPago = dto.ValorPago
            };
        }
    }
}
