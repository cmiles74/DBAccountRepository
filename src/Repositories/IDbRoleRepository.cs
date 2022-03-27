using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.WebService.Repositories;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides an interface that all role repositories must implement
    /// </summary>
    public interface IDbRoleRepository : IWebReadWriteRepository<DbRole, Guid> {
        /// <summary>
        /// Returns the role with the matching unique name
        /// </summary>
        /// <param name="name">unique name of a role</param>
        /// <returns>matching roles instance</returns>
        Task<DbRole> FindByName(string name);
        
        /// <summary>
        /// Returns the roles associated with the provided account
        /// </summary>
        /// <param name="id">unique id of the account for which roles will be returned</param>
        /// <returns>list of roles linked to the provided account</returns>
        Task<List<DbRole>> FindByAccount(Guid id);

        /// <summary>
        /// Returns true if the account with the provided unique identifier is
        /// a member of the role with the provided unique identifier
        /// </summary>
        /// <param name="accountId">unique identifier of an account</param>
        /// <param name="roleId">unique identifier of a role</param>
        /// <returns>true if the provided account is in the provided role</returns>
        Task<bool> DoesAccountHaveRole(Guid accountId, Guid roleId);
        
        /// <summary>
        /// Adds an account to the provided role
        /// </summary>
        /// <param name="roleId">unique ID of the role</param>
        /// <param name="accountId">unique ID of the account</param>
        /// <returns>Updated role</returns>
        Task<DbRole> AddAccount(Guid roleId, Guid accountId);

        /// <summary>
        /// Removes an account from the provided role
        /// </summary>
        /// <param name="roleId">unique ID of the role</param>
        /// <param name="accountId">unique ID of the account</param>
        /// <returns>Updated Role</returns>
        Task<DbRole> RemoveAccount(Guid roleId, Guid accountId);
    }
}