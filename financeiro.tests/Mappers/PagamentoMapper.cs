using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using financeira.Controller.DTO;
using financeira.Controller.Mappers;
using Financeira.Model;

namespace financeiro.tests.Mappers
{
    public class PagamentoMapperTests
    {
        private readonly PagamentoMapper _mapper;

        public PagamentoMapperTests()
        {
            _mapper = new PagamentoMapper();
        }

        [Fact(DisplayName = "Deve mapear corretamente de Entity para DTO")]
        public void ToDto_DeveMapearCorretamente()
        {
            // Arrange
            var entity = new Pagamento
            {
                Id = Guid.NewGuid(),
                DataPagamento = new DateTime(2025, 11, 6),
                ValorPago = 500m
            };

            // Act
            var dto = _mapper.ToDto(entity);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(entity.Id, dto.Id);
            Assert.Equal(entity.DataPagamento, dto.DataPagamento);
            Assert.Equal(entity.ValorPago, dto.ValorPago);
        }

        [Fact(DisplayName = "ToDto deve retornar null quando a entidade for null")]
        public void ToDto_DeveRetornarNull_QuandoEntityForNull()
        {
            // Act
            var dto = _mapper.ToDto(null);

            // Assert
            Assert.Null(dto);
        }

        [Fact(DisplayName = "Deve mapear corretamente de DTO para Entity")]
        public void ToEntity_DeveMapearCorretamente()
        {
            // Arrange
            var dto = new PagamentoDTO
            {
                Id = Guid.NewGuid(),
                DataPagamento = new DateTime(2025, 12, 1),
                ValorPago = 300m
            };

            // Act
            var entity = _mapper.ToEntity(dto);

            // Assert
            Assert.NotNull(entity);
            Assert.Equal(dto.Id, entity.Id);
            Assert.Equal(dto.DataPagamento, entity.DataPagamento);
            Assert.Equal(dto.ValorPago, entity.ValorPago);
        }

        [Fact(DisplayName = "ToEntity deve retornar null quando o DTO for null")]
        public void ToEntity_DeveRetornarNull_QuandoDtoForNull()
        {
            // Act
            var entity = _mapper.ToEntity(null);

            // Assert
            Assert.Null(entity);
        }
    }
}
