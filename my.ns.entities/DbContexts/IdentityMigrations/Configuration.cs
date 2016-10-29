namespace my.ns.entities.DbContexts.IdentityMigrations
{
    using IdentityConfig;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MigrationConfig;
    using System;
    using System.Configuration;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.History;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IdentityDb>
    {
        // Get Admin Account
        string AdminUserName = ConfigurationManager.AppSettings["AdminUserName"] ?? "admin@kirodanmark.dk";
        string AdminPassword = ConfigurationManager.AppSettings["AdminPassword"] ?? "adminadmin";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DbContexts\IdentityMigrations";
            SetHistoryContextFactory("MySql.Data.MySqlClient", HistoryContextFactory.Create);
            SetSqlGenerator("MySql.Data.MySqlClient", new MySqlMigrationScriptGenerator());

        }

        protected override void Seed(IdentityDb context)
        {
            CreateAdminIfNeeded(context);
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
        #region private void CreateAdminIfNeeded()
        private void CreateAdminIfNeeded(IdentityDb context)
        {
            var roleManager =
                new ApplicationRoleManager(
                    new ApplicationRoleStore(context));

            var userManager =
                new ApplicationUserManager(
                    new ApplicationUserStore(context));



            // See if Admin exists
            var objAdminUser = context.Users.Where(m => m.Email.Equals(AdminUserName)).FirstOrDefault();

            if (objAdminUser == null)
            {
                //See if the Admin role exists
                if (!roleManager.RoleExists("Administrator"))
                {
                    // Create the Admin Role (if needed)
                    IdentityRoleIntPk objAdminRole = new IdentityRoleIntPk("Administrator");
                    roleManager.Create(objAdminRole);
                }

                // Create Admin user
                var objNewAdminUser = new ApplicationUser { UserName = AdminUserName, Email = AdminUserName };
                var AdminUserCreateResult = userManager.Create(objNewAdminUser, AdminPassword);
                if (AdminUserCreateResult.Errors.Count() > 0)
                    throw new ArgumentException(AdminUserCreateResult.Errors.First());
                // Put user in Admin role
                userManager.AddToRole(objNewAdminUser.Id, "Administrator");
            }
        }
        #endregion
    }
}
