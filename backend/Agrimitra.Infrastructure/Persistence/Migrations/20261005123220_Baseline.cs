using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agrimitra.Infrastructure.Persistence.Migrations
{
    /// <summary>
    /// Baseline: executes the hand-written PostgreSQL/PostGIS migrations (database/migrations/V*.sql) and reference
    /// seed (database/seed/S*.sql) embedded in this assembly. The model snapshot mirrors the resulting schema, so later
    /// `dotnet ef migrations add` calls only diff against changes made after this baseline.
    /// </summary>
    public partial class Baseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var assembly = typeof(Baseline).Assembly;
            var scripts = assembly.GetManifestResourceNames()
                .Where(n => n.StartsWith("db/", StringComparison.Ordinal) && n.EndsWith(".sql", StringComparison.Ordinal))
                .OrderBy(n => n, StringComparer.Ordinal) // V001.. before S001.. (V < S is false: order explicitly below)
                .OrderBy(n => n.StartsWith("db/V", StringComparison.Ordinal) ? 0 : 1)
                .ToList();

            foreach (var name in scripts)
            {
                using var stream = assembly.GetManifestResourceStream(name)!;
                using var reader = new StreamReader(stream);
                migrationBuilder.Sql(reader.ReadToEnd());
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            throw new NotSupportedException("The baseline cannot be rolled back; restore from backup or recreate the database.");
        }
    }
}
