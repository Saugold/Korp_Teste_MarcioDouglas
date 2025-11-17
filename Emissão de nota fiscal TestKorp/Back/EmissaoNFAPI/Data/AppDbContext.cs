using EmissaoNFAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmissaoNFAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<ProdutoNotaFiscal> ProdutosNotasFiscais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProdutoNotaFiscal>()
                 .HasKey(pn => new { pn.ProdutoId, pn.NotaFiscalId });

            modelBuilder.Entity<ProdutoNotaFiscal>()
                .HasOne(pn => pn.Produto)
                .WithMany(p => p.ProdutoNotaFiscais)
                .HasForeignKey(pn => pn.ProdutoId);

            modelBuilder.Entity<ProdutoNotaFiscal>()
                .HasOne(pn => pn.NotaFiscal)
                .WithMany(nf => nf.Produtos)
                .HasForeignKey(pn => pn.NotaFiscalId);

            modelBuilder.Entity<NotaFiscal>()
                .HasIndex(n => n.Numero)
                .IsUnique();
        }
    }
}
