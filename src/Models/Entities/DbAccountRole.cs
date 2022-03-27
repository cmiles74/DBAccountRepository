using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nervestaple.DbAccountRepository.Models.Entities {
    /// <summary>
    /// Models the relationship between accounts and roles
    /// </summary>
    public class DbAccountRole {
        /// <summary>
        /// The unique identifier of the account
        /// </summary>
        public Guid? DbAccountId { get; set; }
        
        /// <summary>
        /// The account
        /// </summary>
        [ForeignKey("DbAccountId")]
        public DbAccount DbAccount { get; set; }
        
        /// <summary>
        /// The unique identifier of the role
        /// </summary>
        public Guid? DbRoleId { get; set; }
        
        /// <summary>
        /// The role
        /// </summary>
        [ForeignKey("DbRoleId")]
        public DbRole DbRole { get; set; }
    }
}