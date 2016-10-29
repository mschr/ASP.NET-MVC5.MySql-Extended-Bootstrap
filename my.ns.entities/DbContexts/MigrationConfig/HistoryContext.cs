using System.Data.Common;
using System.Data.Entity;

namespace my.ns.entities.DbContexts.MigrationConfig
{
    public class HistoryContextFactory
    {
        static public System.Data.Entity.Migrations.History.HistoryContext Create(
            DbConnection existingConnection,
            string defaultSchema)
        {
            return new HistoryContext(
              existingConnection, defaultSchema);
        }
    }

    public class HistoryContext : System.Data.Entity.Migrations.History.HistoryContext
    {
        //We have to 'new' this as we are overriding the DbSet type
        public HistoryContext(DbConnection dbConnection, string defaultSchema)
            : base(dbConnection, defaultSchema)
        {
        }

        //This part isn't needed but shows what you can do
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("");

            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>().ToTable("__migrationhistory");
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>().HasKey(
                h => new
                {
                    h.MigrationId,
                    h.ContextKey
                });
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.MigrationId).HasColumnName("migration_id");
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.ContextKey).HasColumnName("context_key");
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.Model).HasColumnName("metadata");

            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.MigrationId).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.ContextKey).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.Model).IsRequired().IsMaxLength();
            modelBuilder.Entity<System.Data.Entity.Migrations.History.HistoryRow>()
                        .Property(h => h.ProductVersion).HasMaxLength(32).IsRequired();
        }
    }
}
