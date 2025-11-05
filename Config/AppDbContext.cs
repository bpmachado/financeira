using System.Collections.Generic;
using Financeira.Model;
using Financeira.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Financeira.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Contrato>()
                .Property(c => c.TipoVeiculo)
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (TipoVeiculo)Enum.Parse(typeof(TipoVeiculo), v, true));

            modelBuilder.Entity<Contrato>()
                .Property(c => c.CondicaoVeiculo)
                .HasConversion(
                    v => v.ToString().ToUpper(),
                    v => (CondicaoVeiculo)Enum.Parse(typeof(CondicaoVeiculo), v, true));

            modelBuilder.Entity<Contrato>()
                .HasMany(c => c.Pagamentos)
                .WithOne(p => p.Contrato)
                .HasForeignKey(p => p.ContratoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pagamento>()
                .Property(p => p.StatusPagamento)
                .HasConversion<string>() 
                .HasMaxLength(20);

            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Contrato)
                .WithMany(c => c.Pagamentos)
                .HasForeignKey(p => p.ContratoId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

