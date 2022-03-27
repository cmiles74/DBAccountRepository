using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nervestaple.DbAccountRepository.Helpers;
using Nervestaple.DbAccountRepository.Models;
using Nervestaple.DbAccountRepository.Models.Entities;
using Nervestaple.DbAccountRepository.Repositories;
using Nervestaple.WebService.Models.security;
using Nervestaple.WebService.Services.security;

namespace Nervestaple.DbAccountRepositoryTest {
    [TestClass]
    public class DbAccountRepositoryTest {
        // name of our in-memory test database
        private static readonly string _databaseName = Guid.NewGuid().ToString();

        private static ServiceProvider _serviceProvider;

        private static ILogger _logger;

        [ClassInitialize]
        public static void Setup(TestContext testContext) {
            
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddLogging(b => {
                    b.AddConsole();
                    b.AddDebug();
                });
            AccountRepositoryStartupHelper.ConfigureAccountRepository(serviceCollection);
            serviceCollection.AddDbContext<DbAccountContext>(o => {
                o.UseInMemoryDatabase(_databaseName);
                o.UseLoggerFactory(_serviceProvider.GetService<ILoggerFactory>());
            });

            // service provider to provide logging
            _serviceProvider = serviceCollection.BuildServiceProvider();
            
            // database context
            var dbContext = _serviceProvider.GetService<DbAccountContext>();

            // get a logger instance
            _logger = _serviceProvider.GetService<ILogger<DbAccountRepositoryTest>>();

            // create our test account
            var repository = _serviceProvider.GetService<IDbAccountRepository>();
            var dbAccount = new DbAccount();
            dbAccount.Name = "hhughes";
            dbAccount.FullName = "Howard Hughes";
            dbAccount.Mail = "hhughes@hughes.net";
            dbAccount = repository.Create(dbAccount);
            dbContext.Entry(dbAccount).State = EntityState.Detached;
            _logger.LogInformation("Created account with ID {Id}", dbAccount.Id);
            
            // set credentials for test account
            var credentials = new SimpleAccountCredentials();
            credentials.Id = "hhughes";
            credentials.Password = "spruce1947goose";
            repository.SetPassword(credentials);
            _logger.LogInformation("Updated credentials for account {Id}", credentials.Id);
            
            // create our test role
            var repositoryRole = _serviceProvider.GetService<IDbRoleRepository>();
            var role = new DbRole();
            role.Name = "Administrators";
            var roleSaved = repositoryRole.Create(role);
            dbContext.Entry(roleSaved).State = EntityState.Detached;
            _logger.LogInformation("Created new role with ID {Id}", roleSaved.Id);
        }
        
        [TestMethod]
        public void TestAccountAuthenticate() {
            var repository = _serviceProvider.GetService<IDbAccountRepository>();
            var account = repository.Authenticate(GetCredentials());
            _logger.LogInformation("Authenticated for account {Id}", GetCredentials().Id);
            Assert.IsTrue(account != null);
        }

        [TestMethod]
        public void TestAccountAuthenticateCompat() {
            var repository = _serviceProvider.GetService<IAccountService>();
            var account = repository.Authenticate(GetCredentials());
            Assert.IsTrue(account != null);
        }
        
        [TestMethod]
        public void TestAccountFindCompat() {
            var repository = _serviceProvider.GetService<IAccountService>();
            var account = repository.Find(GetCredentials().Id);
            Assert.IsTrue(account != null);
        }

        [TestMethod]
        public async void TestRoleAccountAdd() {
            var repository = _serviceProvider.GetService<IDbRoleRepository>();
            var role = await repository.FindByName("Administrators");
        
            var repositoryAccount = _serviceProvider.GetService<IDbAccountRepository>();
            var account = await repositoryAccount.FindByName("hhughes");
            await repository.AddAccount(role.Id.Value!, account.Id.Value!);
        
            account = await repositoryAccount.FindByName("hhughes");
            Assert.IsTrue(account.DbAccountRoles.Count == 1);
        }
        
        [TestMethod]
        public async void TestRoleAccountRemove() {
            var repository = _serviceProvider.GetService<IDbRoleRepository>();
            var repositoryAccount = _serviceProvider.GetService<IDbAccountRepository>();
            
            var account = await repositoryAccount.FindByName("hhughes");
            var rolesStart = account.DbAccountRoles.Count;
        
            var role = await repository.FindByName("Administrators");
            await repository.RemoveAccount(role.Id.Value!, account.Id.Value!);
            account = await repositoryAccount.FindByName("hhughes");
            Assert.IsTrue(account.DbRoles.Count == 0);
        }
        
        [TestMethod]
        public async void TestRoleFindByAccount() {
            using (_ = new TransactionScope()) {
                var repository = _serviceProvider.GetService<IDbRoleRepository>();
                var role = await repository.FindByName("Administrators");
            
                var repositoryAccount = _serviceProvider.GetService<IDbAccountRepository>();
                var account = await repositoryAccount.FindByName("hhughes");
                await repository.AddAccount(role.Id.Value!, account.Id.Value!);
            
                var roles = await repository.FindByAccount(account.Id.Value!);
                Assert.IsTrue(roles.Count == 1);
            }
        }

        private IAccountCredentials GetCredentials() {
            var credentials = new SimpleAccountCredentials();
            credentials.Id = "hhughes";
            credentials.Password = "spruce1947goose";
            return credentials;
        }
    }
}