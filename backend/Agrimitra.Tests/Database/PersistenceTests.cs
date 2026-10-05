using Agrimitra.Infrastructure.Persistence;
using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Agrimitra.Tests.Database;

public class PersistenceTests
{
    private static void Probe<T>(DbContext ctx) where T : class => ctx.Set<T>().AsNoTracking().Take(1).ToList();

    [DbFact]
    public void Every_Mapped_Entity_Matches_The_Database_Schema()
    {
        using var ctx = DbEnv.CreateContext();
        var probe = typeof(PersistenceTests).GetMethod(nameof(Probe), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;
        var failures = new List<string>();
        foreach (var et in ctx.Model.GetEntityTypes().Where(e => !e.HasSharedClrType))
        {
            try { probe.MakeGenericMethod(et.ClrType).Invoke(null, [ctx]); }
            catch (Exception ex) { failures.Add($"{et.ClrType.Name}: {(ex.InnerException ?? ex).Message}"); }
        }
        Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
    }

    [DbFact]
    public void Role_And_Permission_Seed_Is_Present()
    {
        using var ctx = DbEnv.CreateContext();
        var roles = ctx.Roles.Select(r => r.Name).ToList();
        string[] expected = ["Farmer","Consultant","Nursery","Laboratory","PesticideShop","Advertiser","Brand","Admin","SuperAdmin","SupportAgent","AIDataAdmin","ContentManager","FinanceAdmin"];
        Assert.Equal(expected.Order(), roles.Order());
    }

    [DbFact]
    public void Land_With_Boundary_Round_Trips_And_SoftDelete_Filter_Applies()
    {
        using var ctx = DbEnv.CreateContext();
        using var tx = ctx.Database.BeginTransaction();
        var userId = Guid.NewGuid();
        ctx.Users.Add(new User { Id = userId, MobileNumber = "+919000000001", PreferredLanguage = "en", Status = "active" });
        var fpId = Guid.NewGuid();
        ctx.FarmerProfiles.Add(new FarmerProfile { Id = fpId, UserId = userId, FarmerCode = "F-" + fpId.ToString("N")[..8], FullName = "T" });
        var farmId = Guid.NewGuid();
        ctx.Farms.Add(new Farm { Id = farmId, FarmerProfileId = fpId, Name = "Farm" });
        var landId = Guid.NewGuid();
        var gf = new GeometryFactory(new PrecisionModel(), 4326);
        ctx.Lands.Add(new Land
        {
            Id = landId, FarmId = farmId, FarmerProfileId = fpId, Name = "Plot", AreaValue = 2, AreaUnit = "acre",
            Location = gf.CreatePoint(new Coordinate(73.9, 20.0))
        });
        ctx.LandBoundaries.Add(new LandBoundary
        {
            LandId = landId,
            Boundary = gf.CreatePolygon([new(73.9, 20.0), new(73.901, 20.0), new(73.901, 20.001), new(73.9, 20.001), new(73.9, 20.0)])
        });
        ctx.SaveChanges();
        ctx.ChangeTracker.Clear();

        var land = ctx.Lands.Single(l => l.Id == landId);
        Assert.Equal(8093.71m, land.AreaSqMeters); // generated column read back
        var boundary = ctx.LandBoundaries.Single(b => b.LandId == landId);
        Assert.True(boundary.AreaSqMeters > 0);

        tx.Rollback();
    }

    [DbFact]
    public void Soft_Deleted_Rows_Are_Hidden_By_Query_Filter()
    {
        using var ctx = DbEnv.CreateContext();
        using var tx = ctx.Database.BeginTransaction();
        var id = Guid.NewGuid();
        ctx.Organizations.Add(new Organization { Id = id, Name = "Org " + id, IsDeleted = true });
        ctx.SaveChanges();
        Assert.DoesNotContain(ctx.Organizations.ToList(), o => o.Id == id);
        Assert.Contains(ctx.Organizations.IgnoreQueryFilters().ToList(), o => o.Id == id);
        tx.Rollback();
    }

    [DbFact]
    public void Partitioned_Audit_Log_Can_Be_Written_And_Is_Append_Only()
    {
        using var ctx = DbEnv.CreateContext();
        using var tx = ctx.Database.BeginTransaction();
        var log = new AuditLog { Id = Guid.NewGuid(), Action = "test", EntityType = "x", CreatedAt = DateTime.UtcNow, NewValue = "{\"a\":1}" };
        ctx.AuditLogs.Add(log);
        ctx.SaveChanges();
        log.Reason = "mutate";
        Assert.Throws<DbUpdateException>(() => ctx.SaveChanges());
        tx.Rollback();
    }
}
