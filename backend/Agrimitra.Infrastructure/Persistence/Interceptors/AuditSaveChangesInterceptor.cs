using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Agrimitra.Infrastructure.Persistence.Interceptors;

public interface ICurrentUserAccessor
{
    Guid? UserId { get; }
}

/// <summary>Fills created/updated audit columns and turns deletes of soft-deletable rows into updates.</summary>
public sealed class AuditSaveChangesInterceptor(ICurrentUserAccessor currentUser, TimeProvider clock) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData e, InterceptionResult<int> result)
    {
        Apply(e.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData e, InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        Apply(e.Context);
        return ValueTask.FromResult(result);
    }

    private void Apply(DbContext? ctx)
    {
        if (ctx is null) return;
        var now = clock.GetUtcNow();
        var user = currentUser.UserId;

        foreach (var entry in ctx.ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    Set(entry, "CreatedAt", now); Set(entry, "CreatedBy", user);
                    Set(entry, "UpdatedAt", now); Set(entry, "UpdatedBy", user);
                    break;
                case EntityState.Modified:
                    Set(entry, "UpdatedAt", now); Set(entry, "UpdatedBy", user);
                    break;
                case EntityState.Deleted when entry.Metadata.FindProperty("IsDeleted") is not null:
                    entry.State = EntityState.Modified;
                    Set(entry, "IsDeleted", true); Set(entry, "DeletedAt", now); Set(entry, "DeletedBy", user);
                    Set(entry, "UpdatedAt", now); Set(entry, "UpdatedBy", user);
                    break;
            }
        }
    }

    private static void Set(EntityEntry entry, string name, object? value)
    {
        var p = entry.Metadata.FindProperty(name);
        if (p is null) return;
        var type = Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType;
        object? v = value;
        if (value is DateTimeOffset dto && type == typeof(DateTime)) v = dto.UtcDateTime;
        if (value is DateTimeOffset dto2 && type == typeof(DateTimeOffset)) v = dto2;
        if (value is null && !p.IsNullable) return;
        if (name.StartsWith("Created") && entry.State == EntityState.Added && entry.Property(name).CurrentValue is { } cur
            && !IsDefault(cur)) return; // respect explicit values
        entry.Property(name).CurrentValue = v;
    }

    private static bool IsDefault(object v) => v is DateTime dt ? dt == default : v is DateTimeOffset o ? o == default : false;
}
