using Microsoft.EntityFrameworkCore;
using Nervestaple.DbAccountRepository.Models.Entities;

namespace Nervestaple.DbAccountRepository.Helpers {
    /// <summary>
    /// Provides a class for configuring database contexts
    /// </summary>
    public class DbAccountContextHelper {
        /// <summary>
        /// Configures the model builder for our accounts
        /// </summary>
        /// <param name="modelBuilder">database context model build</param>
        public static void HandleOnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DbAccount>().HasIndex(e => e.Name).IsUnique();
            modelBuilder.Entity<DbRole>().HasIndex(e => e.Name).IsUnique();
            
            modelBuilder.Entity<DbAccountRole>()
                .HasKey(ar => new { ar.DbAccountId, ar.DbRoleId });
            modelBuilder.Entity<DbAccountRole>()
                .HasOne(ar => ar.DbRole)
                .WithMany(r => r.DbAccountRoles)
                .HasForeignKey(ar => ar.DbRoleId);
            modelBuilder.Entity<DbAccountRole>()
                .HasOne(ar => ar.DbAccount)
                .WithMany(r => r.DbAccountRoles)
                .HasForeignKey(ar => ar.DbAccountId);
        }
    }
}