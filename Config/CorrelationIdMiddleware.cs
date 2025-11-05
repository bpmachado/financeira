using Serilog.Context;

namespace financeira.Config
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Verifica se já existe CorrelationId no header
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault()
                                ?? Guid.NewGuid().ToString();

            // Adiciona no header de resposta usando Append para evitar ArgumentException
            context.Response.Headers.Append("X-Correlation-ID", correlationId);

            // Adiciona ao LogContext do Serilog
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}
