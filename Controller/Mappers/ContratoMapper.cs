using financeira.Controller.Mappers;
using Financeira.DTO;
using Financeira.Model;

public class ContratoMapper : IContratoMapper
{
    public Contrato ToEntity(ContratoDTO dto)
    {
        return new Contrato
        {
            Id = dto.Id,
            ClienteCpfCnpj = dto.ClienteCpfCnpj,
            ValorTotal = dto.ValorTotal,
            TaxaMensal = dto.TaxaMensal,
            PrazoMeses = dto.PrazoMeses,
            DataVencimentoPrimeiraParcela = DateTime.SpecifyKind(dto.DataVencimentoPrimeiraParcela, DateTimeKind.Utc),
            TipoVeiculo = dto.TipoVeiculo,
            CondicaoVeiculo = dto.CondicaoVeiculo
        };
    }

    public ContratoDTO? ToDto(Contrato contrato)
    {
        if (contrato == null) return null;
        return new ContratoDTO
        {
            Id = contrato.Id,
            ClienteCpfCnpj = contrato.ClienteCpfCnpj,
            ValorTotal = contrato.ValorTotal,
            TaxaMensal = contrato.TaxaMensal,
            PrazoMeses = contrato.PrazoMeses,
            DataVencimentoPrimeiraParcela = contrato.DataVencimentoPrimeiraParcela,
            TipoVeiculo = contrato.TipoVeiculo,
            CondicaoVeiculo = contrato.CondicaoVeiculo
        };
    }
}
