using Nervestaple.DbAccountRepository.Models;
using Nervestaple.EntityFrameworkCore.Models.Entities;
using Nervestaple.EntityFrameworkCore.Repositories;

namespace Nervestaple.DbAccountRepository.Repositories {
    /// <summary>
    /// Provides a read only repository of account information
    /// </summary>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <typeparam name="TId">Type of unnique identifier for the entities</typeparam>
    public class DbAccountReadOnlyRepository <TEntity, TId> : AbstractReadOnlyRepository<TEntity, TId> 
        where TEntity : Entity<TId> 
        where TId: struct
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly DbAccountContext _context;
        
        /// <summary>
        /// Creates a new repository instance.
        /// </summary>
        /// <param name="context">the database context to use</param>
        /// <returns>a new instance</returns>
        public DbAccountReadOnlyRepository(DbAccountContext context) : base(context)
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