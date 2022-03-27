using System;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.EntityFrameworkCore.Models.Criteria;

namespace Nervestaple.DbAccountRepository.Models.Criteria {
    /// <summary>
    /// Criteria object for finding sets of accounts
    /// </summary>
    public class DbAccountCriteria : SearchCriteria<DbAccount, Guid> {
        /// <summary>
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