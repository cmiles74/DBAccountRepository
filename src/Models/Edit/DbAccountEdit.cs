using System;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.EntityFrameworkCore.Models.Entities;

namespace Nervestaple.DbAccountRepository.Models.Edit {
    /// <summary>
    /// Edit object for updating account information
    /// </summary>
    public class DbAccountEdit : EditModel<DbAccount, Guid> {
        // <summary>
        /// The unique name of the account
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The common name for this account
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// The email address for this account
        /// </summary>
        public string Mail { get; set; }
    }
}