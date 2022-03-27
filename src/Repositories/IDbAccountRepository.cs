using System;
using System.Threading.Tasks;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.WebService.Models.security;
using Nervestaple.WebService.Repositories;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides an interface that account repositories must implement
    /// </summary>
    public interface IDbAccountRepository : IWebReadWriteRepository<DbAccount, Guid> {
        
        /// <summary>
        /// Returns the account with the matching unique account name
        /// </summary>
        /// <param name="name">unique account name</param>
        /// <returns>matching account instance</returns>
        Task<DbAccount> FindByName(string name);

        /// <summary>
        /// Sets the password for the account with the unique name provided
        /// by the credentials
        /// </summary>
        /// <param name="accountCredentials">credentials with unique account name and password</param>
        /// <returns>updated account instance</returns>
        Task<DbAccount> SetPassword(IAccountCredentials accountCredentials);

        /// <summary>
        /// Returns the account that matches the provided credentials
        /// </summary>
        /// <param name="accountCredentials">account credentials to match</param>
        /// <returns>account with the matching credentials</returns>
        Task<DbAccount> Authenticate(IAccountCredentials accountCredentials);
    }
}