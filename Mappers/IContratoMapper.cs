using Financeira.DTO;
using Financeira.Model;

namespace financeira.Controller.Mappers
{
    public interface IContratoMapper
    {
        Contrato? ToEntity(ContratoDTO dto);
        ContratoDTO? ToDto(Contrato contrato);
    }
}
