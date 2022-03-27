using Nervestaple.DbAccountRepository.Models;
using Nervestaple.EntityFrameworkCore.Models.Entities;
using Nervestaple.WebService.Repositories;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides a read and writeable repository of account information
    /// </summary>
    /// <typeparam name="TEntity">type of entity</typeparam>
    /// <typeparam name="TId">type of the unique identifier of the entities</typeparam>
    public class DbAccountReadWriteRepository<TEntity, TId> : AbstractWebReadWriteRepository<TEntity, TId>
        where TEntity : Entity<TId> where TId : struct {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly DbAccountContext _context;
        
        /// <summary>
        /// Creates a new repository instance.
        /// </summary>
        /// <param name="context">the database context to use</param>
        /// <returns>a new instance</returns>
        public DbAccountReadWriteRepository(DbAccountContext context) : base(context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Returns the database context.
        /// </summary>
        public new DbAccountContext Context { 
            get {
                return _context;
            }
        }
    }
}