using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.ns.entities.DbContexts
{
    class ApplicationDb : DbContext
    {
        public DbSet<Log4Net.LogModel> Log { get; set; }

        private void Dbg(string dbg)
        {
            System.Diagnostics.Debug.WriteLine(dbg);
        }
        public ApplicationDb()
            : base("DefaultConnection")
        {
            /* disables checks to migrationhistory table (since it has been renamed - this fails) */
            Database.SetInitializer<ApplicationDb>(null);
            //this.Database.Log = Dbg;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Log4Net.LogModel>().ToTable("logs");
        }
        }
}
