using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Nervestaple.WebService.Models.security;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides a class to make it easier to work with passwords
    /// </summary>
    public class PasswordHelper {
        /// <summary>
        /// Creates a new salt for a password
        /// </summary>
        /// <returns>byte array with the new salt value</returns>
        public static byte[] CreateSalt() {
            byte[] passwordSaltBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(passwordSaltBytes);
            }

            return passwordSaltBytes;
        }

        /// <summary>
        /// Converts an array of bytes containing a salt value into a string
        /// </summary>
        /// <param name="salt">byte array with the salt</param>
        /// <returns>string representing the salt</returns>
        public static string SaltToString(byte[] salt) {
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// Creates a new byte array containing the salted password hash
        /// </summary>
        /// <param name="password">the password to hash</param>
        /// <param name="salt">salt for the password</param>
        /// <returns>byte array with the salted password hash</returns>
        public static byte[] CreatePasswordHashBytes(string password, byte[] salt) {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
        }

        /// <summary>
        /// Converts an array of bytes containing a salted password hash into a string
        /// </summary>
        /// <param name="password">byte array with the salted password hash</param>
        /// <returns>string representing the salted and hashed passworderic suher </returns
        public static string PasswordHashToString(byte[] password) {
            return Convert.ToBase64String(password);
        }

        /// <summary>
        /// Creates a new salted hash of the provided password
        /// </summary>
        /// <param name="password">password to hash</param>
        /// <param name="salt">salt for the password</param>
        /// <returns>string with the salted password hash</returns>
        public static string CreatePasswordHash(string password, byte[] salt) {
            return PasswordHashToString(CreatePasswordHashBytes(password, salt));
        }
        
        /// <summary>
        /// Createes a new salted hash of the password from the provided credentials
        /// </summary>
        /// <param name="credentials">credentials with the password to hash</param>
        /// <param name="salt">salt for the password</param>
        /// <returns>string with the salted password hash</returns>
        public static string CreatePasswordHash(IAccountCredentials credentials, byte[] salt) {
            return PasswordHashToString(CreatePasswordHashBytes(credentials.Password, salt));
        }
    }
}