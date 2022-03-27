using System;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.EntityFrameworkCore.Models.Entities;

namespace Nervestaple.DbAccountRepository.Models.Edit {
    /// <summary>
    /// Edit object for updating role information
    /// </summary>
    public class DbRoleEdit : EditModel<DbRole, Guid> {
        /// <summary>
        /// The common name for this role
        /// </summary>
        public string Name { get; set; }
    }
}