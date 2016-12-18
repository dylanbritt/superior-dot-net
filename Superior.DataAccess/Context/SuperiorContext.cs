using Superior.DataAccess.Configurations;
using Superior.Domain.Models;
using System.Data.Entity;

namespace Superior.DataAccess.Context
{
    // enable-migrations –EnableAutomaticMigration:$true
    // add-migration "description"
    // update-database
    // update-database -TargetMigration:"First School DB schema"

    public class SuperiorContext : DbContext
    {
        public SuperiorContext() : base("Superior")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<UserLoginMonitor> UserLoginMonitors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.SetUserConfiguration();
        }
    }
}
