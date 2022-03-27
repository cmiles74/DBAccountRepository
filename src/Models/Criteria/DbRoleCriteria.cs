using System;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.EntityFrameworkCore.Models.Criteria;

namespace Nervestaple.DbAccountRepository.Models.Criteria {
    /// <summary>
    /// Criteria object of finding sets of roles
    /// </summary>
    public class DbRoleCriteria : SearchCriteria<DbRole, Guid> {
        /// <summary>
        /// The common name for this role
        /// </summary>
        public string Name { get; set; }
    }
}