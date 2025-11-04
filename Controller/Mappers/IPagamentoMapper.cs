using financeira.Controller.DTO;
using Financeira.Model;

namespace financeira.Controller.Mappers
{
    public interface IPagamentoMapper
    {
        PagamentoDTO? ToDto(Pagamento? entity);
        Pagamento? ToEntity(PagamentoDTO? dto);
    }
}
