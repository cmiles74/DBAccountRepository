namespace Nervestaple.DbAccountRepository.Models.Entities {
    /// <summary>
    /// Data object representing a salted and hashed password
    /// </summary>
    public class SaltedPasswordHash {
        /// <summary>
        /// Creates a new instance and sets its field
        /// </summary>
        /// <param name="salt">salt used when creating the password hash</param>
        /// <param name="passwordHash">hashed password</param>
        public SaltedPasswordHash(string salt, string passwordHash) {
            Salt = salt;
            PasswordHash = passwordHash;
        }
        
        /// <summary>
        /// The salt used to create the hashed password
        /// </summary>
        public string Salt { get; set; }
        
        /// <summary>
        /// The hashed password
        /// </summary>
        public string PasswordHash { get; set; }
    }
}