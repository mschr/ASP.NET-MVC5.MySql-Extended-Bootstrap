using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations.History;
using MySql.Data.Entity;
using my.ns.entities.IdentityConfig;

namespace my.ns.entities.DbContexts
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class IdentityDb : IdentityDbContext<ApplicationUser, IdentityRoleIntPk, int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public IdentityDb()
            : base("DefaultConnection")
        {
            /* disables checks to migrationhistory table (since it has been renamed - this fails) */
            //Database.SetInitializer<AppDb>(null);
        }

        public static IdentityDb Create()
        {
            var context = new IdentityDb();
            if(((MySql.Data.MySqlClient.MySqlConnection)context.Database.Connection).State == System.Data.ConnectionState.Closed)
            {
                System.Diagnostics.Trace.WriteLine("IdentityDb connecting");
            }
            context.Database.CreateIfNotExists();
            return context;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRoleIntPk>()
                .ToTable("aspnetroles")
               .Property(c => c.Name)
               .HasMaxLength(128)
               .IsRequired();
            modelBuilder.Entity<ApplicationUser>()
               .ToTable("aspnetusers")
               .Property(c => c.UserName)
               .HasMaxLength(128)
               .IsRequired();
            modelBuilder.Entity<UserLoginIntPk>().ToTable("aspnetuserlogins");
            modelBuilder.Entity<IdentityRoleIntPk>().ToTable("aspnetroles");
            modelBuilder.Entity<UserRoleIntPk>().ToTable("aspnetuserroles");
        }
    }
}
