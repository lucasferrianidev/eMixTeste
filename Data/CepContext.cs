using EMixApi.Data.Models;
using EMixApi.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Cep.Data;

public class CepDbContext : DbContext
{
    public CepDbContext(DbContextOptions<CepDbContext> opts) : base(opts)
    {
    }

    public DbSet<CEP> Ceps { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CepMap());
    }
}
