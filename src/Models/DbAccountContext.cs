using Microsoft.EntityFrameworkCore;
using Nervestaple.DbAccountRepository.Helpers;
using Nervestaple.DbAccountRepository.Models.Entities;

namespace Nervestaple.DbAccountRepository.Models {
    /// <summary>
    /// Provides the database context for our Account data
    /// </summary>
    public class DbAccountContext : DbContext {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="options"></param>
        public DbAccountContext(DbContextOptions<DbAccountContext> options) : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            DbAccountContextHelper.HandleOnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Returns a set of account instances
        /// </summary>
        public DbSet<DbAccount> DbAccounts { get; set; }
        
        /// <summary>
        /// Returns a set of group instances
        /// </summary>
        public DbSet<DbRole> DbRoles { get; set; }
        
        /// <summary>
        /// Returns accounts linked to roles
        /// </summary>
        public DbSet<DbAccountRole> DbAccountRoles { get; set; }
    }
}