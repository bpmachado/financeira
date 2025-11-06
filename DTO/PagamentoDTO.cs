using System.ComponentModel.DataAnnotations;
using Financeira.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace financeira.Controller.DTO
{
    [SwaggerSchema("Pagamento")]
    public class PagamentoDTO
    {
        [SwaggerSchema("ID do pagamento")]
        public Guid Id { get; set; }

        [SwaggerSchema("Data do pagamento")]
        [Required(ErrorMessage = "Data de pagamento é obrigatória")]
        [DataType(DataType.Date)]
        [FutureOrPresent(ErrorMessage = "A data deve ser hoje ou no futuro")]
        public DateTime DataPagamento { get; set; }

        [SwaggerSchema("Valor pago")]
        [Required(ErrorMessage = "Valor total é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal ValorPago { get; set; }
    }
}
