using System.Collections.Generic;
using Financeira.Model;
using Microsoft.EntityFrameworkCore;

namespace Financeira.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
    }
}
