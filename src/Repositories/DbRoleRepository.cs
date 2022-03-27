using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nervestaple.DbAccountRepository.Models;
using Nervestaple.DbAccountRepository.Models.Entities;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides a repository for role information
    /// </summary>
    public class DbRoleRepository : DbAccountReadWriteRepository<DbRole, Guid>, IDbRoleRepository {
        
        /// <inheritdoc/>
        public DbRoleRepository(DbAccountContext context) : base(context)
        {

        }

        /// <inheritdoc />
        public override IQueryable<DbRole> GetEntities() {
            return base.GetEntities().AsNoTracking();
        }

        /// <inheritdoc />
        public async Task<DbRole> FindByName(string name) {
            var role = await GetEntities().FirstOrDefaultAsync(e => e.Name.Equals(name));
            return role;
        }

        /// <inheritdoc />
        public async Task<List<DbRole>> FindByAccount(Guid accountId) {
            var roles = GetEntities().Join(Context.DbAccountRoles,
                    e => e.Id,
                    j => j.DbRoleId,
                    (e, j) => new {
                        DbRole = e,
                        DbAccountRole = j
                    }).Where(z => z.DbAccountRole.DbAccountId.Equals(accountId))
                .Select(z => z.DbRole).Distinct();

            return await roles.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> DoesAccountHaveRole(Guid accountId, Guid roleId) {
            bool accountHasRole = false;

            var role = await GetEntities().Join(Context.DbAccountRoles,
                    e => e.Id,
                    j => j.DbRoleId,
                    (e, j) => new {
                        DbRole = e,
                        DbAccountRole = j
                    }).Where(z => z.DbAccountRole.DbAccountId.Equals(accountId) 
                                  && z.DbAccountRole.DbRoleId.Equals(roleId))
                .Select(z => z.DbRole).FirstOrDefaultAsync();
            if (role != null) {
                accountHasRole = true;
            }
            
            return accountHasRole;
        }

        /// <inheritdoc />
        public async Task<DbRole> AddAccount(Guid roleId, Guid accountId) {
            if (await DoesAccountHaveRole(accountId, roleId)) {
                return await FindAsync(roleId);
            }
            
            var role = await Context.Set<DbRole>().FirstOrDefaultAsync(e => e.Id.Equals(roleId));
            var account = await Context.Set<DbAccount>().FirstOrDefaultAsync(e => e.Id.Equals(accountId));

            if (role == null || account == null) {
                return null;
            }
            
            var accountRole = new DbAccountRole();
            Context.Update(accountRole);
            accountRole.DbAccount = account;
            accountRole.DbRole = role;
            await Context.SaveChangesAsync();

            return await FindAsync(roleId);
        }

        /// <inheritdoc />
        public async Task<DbRole> RemoveAccount(Guid roleId, Guid accountId) {
            var accountRole = await Context.Set<DbAccountRole>()
                .FirstOrDefaultAsync(e => 
                    e.DbRoleId.Equals(roleId) && e.DbAccountId.Equals(accountId));
            Context.Remove(accountRole);
            await Context.SaveChangesAsync();
            
            return await FindAsync(roleId);
        }
    }
}