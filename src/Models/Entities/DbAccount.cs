using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Nervestaple.EntityFrameworkCore.Models.Entities;

namespace Nervestaple.DbAccountRepository.Models.Entities {
    /// <summary>
    /// Models an Account instance that is stored in a database
    /// </summary>
    public class DbAccount : Entity<Guid> {
        /// <summary>
        /// The unique ID of the account that is stored in the database
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("account_id")]
        public override Guid? Id { get; set; }
        
        /// <summary>
        /// The name of the account, this must be unique across all accounts
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The common name for this account
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        public string FullName { get; set; }
        
        /// <summary>
        /// The email address for this account
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        public string Mail { get; set; }
        
        /// <summary>
        /// The salt used to create the password
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        public string PasswordSalt { get; set; }
        
        /// <summary>
        /// The salted and hashed version of the password
        /// </summary>
        [Column(TypeName = "varchar(512)")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// List of the account and role join records linked to this account.
        /// </summary>
        [JsonIgnore]
        public ICollection<DbAccountRole> DbAccountRoles { get; set; }

        /// <summary>
        /// Returns a list of roles linked to this account
        /// </summary>
        [NotMapped]
        public List<DbRole> DbRoles {
            get {
                var roles = new List<DbRole>();
                foreach (var accountRoles in DbAccountRoles) {
                    roles.Add(accountRoles.DbRole);
                }

                return roles;
            }
        }
    }
}