using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nervestaple.DbAccountRepository.Models;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.WebService.Models.security;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides a repository for account information
    /// </summary>
    public class DbAccountRepository : DbAccountReadWriteRepository<DbAccount, Guid>, IDbAccountRepository {
                
        /// <inheritdoc/>
        public DbAccountRepository(DbAccountContext context) : base(context)
        {
            
        }

        /// <inheritdoc />
        public override IQueryable<DbAccount> GetEntities() {
            return Context.Set<DbAccount>().AsNoTracking()
                .Include(e => e.DbAccountRoles)
                .ThenInclude(e => e.DbRole);
        }

        /// <inheritdoc />
        public async Task<DbAccount> FindByName(string name) {
            var entity = await GetEntities().FirstOrDefaultAsync(e => e.Name.Equals(name));
            return entity;
        }

        /// <inheritdoc />
        public async Task<DbAccount> SetPassword(IAccountCredentials accountCredentials) {
            
            // create a new salt
            byte[] passwordSalt = PasswordHelper.CreateSalt();
            
            // derive the hashed password
            string passwordHash = PasswordHelper.CreatePasswordHash(accountCredentials, passwordSalt);
            
            // find and update the account
            var entity = await FindByName(accountCredentials.Id);
            if (entity != null) {
                Context.Update(entity);
                entity.PasswordSalt = PasswordHelper.SaltToString(passwordSalt);
                entity.PasswordHash = passwordHash;
                await Context.SaveChangesAsync();
            }

            return entity;
        }

        /// <inheritdoc />
        public async Task<DbAccount> Authenticate(IAccountCredentials accountCredentials) {
            DbAccount authenticatedAccount = null;
            
            // find the account
            var entity = await FindByName(accountCredentials.Id);

            if (entity != null) {
                
                // convert the salt
                byte[] passwordSalt = Convert.FromBase64String(entity.PasswordSalt);
                
                // derive the hashed password
                string passwordHash = PasswordHelper.CreatePasswordHash(accountCredentials, passwordSalt);
                
                // find the matching account
                authenticatedAccount = await GetEntities()
                    .FirstOrDefaultAsync(e => e.Name.Equals(accountCredentials.Id) 
                                         && e.PasswordHash.Equals(passwordHash));
            }

            return authenticatedAccount;
        } 
    }
}