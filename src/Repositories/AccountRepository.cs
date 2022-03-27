using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.WebService.Models.security;
using Nervestaple.WebService.Repositories.security;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides a repository for database backed account information
    /// </summary>
    public class AccountRepository : IAccountRepository, IDisposable {

        /// <summary>
        /// Backing database repository
        /// </summary>
        private readonly IDbAccountRepository _dbAccountRepository;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="accountRepository">database repository backing this instance</param>
        public AccountRepository(IDbAccountRepository accountRepository) {
            _dbAccountRepository = accountRepository;
        }
        
        /// <summary>
        /// Authenticates the credentials against the database and, if
        /// successful, returns the matching account
        /// </summary>
        /// <param name="credentials">credentials to verify</param>
        /// <returns>account matching the credentials</returns>
        public Account Authenticate(IAccountCredentials credentials) {
            return AuthenticateAsync(credentials).Result;
        }
        
        /// <summary>
        /// Authenticates the credentials against the database and, if
        /// successful, returns the matching account
        /// </summary>
        /// <param name="credentials">credentials to verify</param>
        /// <returns>account matching the credentials</returns>
        public async Task<Account> AuthenticateAsync(IAccountCredentials credentials) {
            var account = await _dbAccountRepository.Authenticate(credentials);
            if (account != null) {
                return ConvertDbAccount(account);
            }

            return null;
        }

        /// <summary>
        /// Returns the account with the matching unique account name
        /// </summary>
        /// <param name="id">unique account name to return</param>
        /// <returns>account with the matching unique account name</returns>
        public Account Find(string id) {
            return FindAsync(id).Result;
        }

        /// <inheritdoc />
        public async Task<Account> FindAsync(string id) {
            var account = await _dbAccountRepository.FindByName(id);
            if (account != null) {
                return ConvertDbAccount(account);
            }

            return null;
        }

        /// <summary>
        /// Converts a DbAccount into an Account
        /// </summary>
        /// <param name="dbAccount">DbAccount instance to convert</param>
        /// <returns>Account instance</returns>
        private Account ConvertDbAccount(DbAccount dbAccount) {
            var account = new Account();
            
            if (dbAccount != null) {
                account.Id = dbAccount.Name;
                account.FullName = dbAccount.FullName;
                account.Mail = dbAccount.Mail;

                var roles = new List<string>();
                foreach (var roleThis in dbAccount.DbRoles) {
                    roles.Add(roleThis.Name);
                }

                account.Roles = roles;
            }

            return account;
        }

        /// <summary>
        /// Returns true if this repository has been disposed.
        /// <returns>
        /// true if this repository has been disposed
        /// </returns>
        /// </summary>
        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes the instance
        /// </summary>
        /// <param name="disposing">flag indicating we are disposing</param>
        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // do nothing
                }

                _disposedValue = true;
            }
        }

        /// <inheritdoc/>
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}