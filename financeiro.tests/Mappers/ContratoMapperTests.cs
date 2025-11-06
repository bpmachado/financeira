using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Financeira.DTO;
using Financeira.Model;
using Financeira.Model.Enums;

namespace financeiro.tests.Mappers
{
    public class ContratoMapperTests
    {
        private readonly ContratoMapper _mapper;

        public ContratoMapperTests()
        {
            _mapper = new ContratoMapper();
        }

        [Fact(DisplayName = "Deve mapear corretamente de DTO para Entity")]
        public void ToEntity_DeveMapearCorretamente()
        {

            var dto = new ContratoDTO
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = "12345678900",
                ValorTotal = 1000m,
                TaxaMensal = 1.5m,
                PrazoMeses = 12,
                DataVencimentoPrimeiraParcela = new DateTime(2025, 11, 10),
                TipoVeiculo = TipoVeiculo.Automovel,
                CondicaoVeiculo = CondicaoVeiculo.Novo
            };

            var entity = _mapper.ToEntity(dto);

            Assert.Equal(dto.Id, entity.Id);
            Assert.Equal(dto.ClienteCpfCnpj, entity.ClienteCpfCnpj);
            Assert.Equal(dto.ValorTotal, entity.ValorTotal);
            Assert.Equal(dto.TaxaMensal, entity.TaxaMensal);
            Assert.Equal(dto.PrazoMeses, entity.PrazoMeses);
            Assert.Equal(dto.DataVencimentoPrimeiraParcela, entity.DataVencimentoPrimeiraParcela);
            Assert.Equal(DateTimeKind.Utc, entity.DataVencimentoPrimeiraParcela.Kind);
            Assert.Equal(dto.TipoVeiculo, entity.TipoVeiculo);
            Assert.Equal(dto.CondicaoVeiculo, entity.CondicaoVeiculo);
        }

        [Fact(DisplayName = "Deve mapear corretamente de Entity para DTO")]
        public void ToDto_DeveMapearCorretamente()
        {
            var entity = new Contrato
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = "12345678900",
                ValorTotal = 1500m,
                TaxaMensal = 2.0m,
                PrazoMeses = 24,
                DataVencimentoPrimeiraParcela = new DateTime(2025, 12, 15),
                TipoVeiculo = TipoVeiculo.Moto,
                CondicaoVeiculo = CondicaoVeiculo.Usado
            };

            var dto = _mapper.ToDto(entity);

            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.ClienteCpfCnpj, dto.ClienteCpfCnpj);
            Assert.Equal(entity.ValorTotal, dto.ValorTotal);
            Assert.Equal(entity.TaxaMensal, dto.TaxaMensal);
            Assert.Equal(entity.PrazoMeses, dto.PrazoMeses);
            Assert.Equal(entity.DataVencimentoPrimeiraParcela, dto.DataVencimentoPrimeiraParcela);
            Assert.Equal(entity.TipoVeiculo, dto.TipoVeiculo);
            Assert.Equal(entity.CondicaoVeiculo, dto.CondicaoVeiculo);
        }

        [Fact(DisplayName = "ToDto deve retornar null quando a entidade for null")]
        public void ToDto_DeveRetornarNull_QuandoEntityForNull()
        {
            // Act
            var dto = _mapper.ToDto(null);

            // Assert
            Assert.Null(dto);
        }
    }
}
