using Microsoft.Extensions.DependencyInjection;
using Nervestaple.DbAccountRepository.Repositories;
using Nervestaple.WebService.Repositories.security;
using Nervestaple.WebService.Services.security;

namespace Nervestaple.DbAccountRepository.Helpers {
    /// <summary>
    /// Provides helper methods to make it easier to configure your application
    /// for the database backed account repository
    /// </summary>
    public class AccountRepositoryStartupHelper {
        /// <summary>
        /// Configures the services that are backed by the database account
        /// repository.
        /// </summary>
        /// <param name="serviceCollection">Application services</param>
        public static void ConfigureAccountRepository(IServiceCollection serviceCollection) {

            serviceCollection.AddTransient<IDbRoleRepository, DbRoleRepository>();
            serviceCollection.AddTransient<IDbAccountRepository, Repositories.DbAccountRepository>();
            serviceCollection.AddTransient<IAccountRepository, Repositories.AccountRepository>();
            serviceCollection.AddTransient<IAccountService, AccountService>();
        }
    }
}