using Agrimitra.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Agrimitra.Tests.Database;

/// <summary>Runs only when AGRIMITRA_TEST_DB points at a migrated database (see database/README.md).</summary>
public sealed class DbFactAttribute : FactAttribute
{
    public DbFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(DbEnv.Variable)))
            Skip = $"{DbEnv.Variable} is not set";
    }
}

internal static class DbEnv
{
    public const string Variable = "AGRIMITRA_TEST_DB";

    public static AgrimitraDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AgrimitraDbContext>()
            .UseNpgsql(Environment.GetEnvironmentVariable(Variable), o => { o.UseNetTopologySuite(); o.UseVector(); })
            .Options);
}
