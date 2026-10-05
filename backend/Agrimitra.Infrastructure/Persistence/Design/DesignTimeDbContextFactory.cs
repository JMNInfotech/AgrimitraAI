using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Agrimitra.Infrastructure.Persistence.Design;

/// <summary>Used by `dotnet ef`. Connection string comes from AGRIMITRA_DB (never hardcoded).</summary>
internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AgrimitraDbContext>
{
    public AgrimitraDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("AGRIMITRA_DB")
                 ?? "Host=localhost;Database=agrimitra;Username=agri;Password=agri";
        var options = new DbContextOptionsBuilder<AgrimitraDbContext>()
            .UseNpgsql(cs, o => { o.UseNetTopologySuite(); o.UseVector(); })
            .Options;
        return new AgrimitraDbContext(options);
    }
}
