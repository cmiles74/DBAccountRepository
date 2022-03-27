using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Nervestaple.EntityFrameworkCore.Models.Entities;
using Newtonsoft.Json;

namespace Nervestaple.DbAccountRepository.Models.Entities {
    /// <summary>
    /// Models a Role instance that is stored in a database
    /// </summary>
    public class DbRole : Entity<Guid> {
        /// <summary>
        /// The unique ID (in the case of Active Directory, the sAMAccountName,
        /// in other cases the uid) of the account.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column("role_id")]
        public override Guid? Id { get; set; }
        
        /// <summary>
        /// The common name for this role
        /// </summary>
        [Column(TypeName = "varchar(255)")]
        public string Name { get; set; }
        
        /// <summary>
        /// List of the roles linked to this account.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<DbAccountRole> DbAccountRoles { get; set; }
    }
}