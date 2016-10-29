namespace my.ns.entities.DbContexts.AppMigrations
{
    using MigrationConfig;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<my.ns.entities.DbContexts.ApplicationDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DbContexts\AppMigrations";
            SetHistoryContextFactory("MySql.Data.MySqlClient", HistoryContextFactory.Create);
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationScriptGenerator());

        }

        protected override void Seed(my.ns.entities.DbContexts.ApplicationDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
