![Continuous Integration](https://github.com/cmiles74/DBAccountRepository/actions/workflows/ci.yml/badge.svg)

# Nervestaple.DBAccountRepository

This .Net Core library provides a repository for [Microsoft Identity][0] data
that is backed by a database. It's not very complicated, it manages the bare 
minimum necessary to get authentication and authorization working; it will 
store account names, passwords and has the ability to manage groups for those
accounts. Other information like friendly names or email addresses should be
managed and stored by your application. The goal of this application is to
provide a secure alternative to "rolling your own" storage of usernames and
password information in your application; it's meant for prototypes or smaller
projects.

There is also a similar project that exposes the data in an LDAP directory in
much the same manner.

* [Nervestaple.LdapRepository][1]

They're both available on [NuGet.org][2], you may follow the directions on that
page to add them to your project.

* [Nervestaple.DBAccountRepository NuGet Package][2]

## Configuration

This library provides its own set of entities for managing account and role
information, these are linked to the `DbAccountContext` instance. You will 
need to configure this context with your own database information. In your
`Startup.cs` file (or wherever you setup your database context) setup the
context for this library as well.

```c#
...
services.AddDbContext<DbAccountContext>(options => {
   ...
});
```

The context options should point to whatever database you want to use to store
your account information, it can be the same as the rest of your application.

We need to set some of the relationships between the entities used to manage
account and role information. You can get those in place by calling our to 
our helper method. In your database context class, add the following:

```c#
protected override void OnModelCreating(ModelBuilder modelBuilder) {
            
    // configura accounts
    DbAccountContextHelper.HandleOnModelCreating(modelBuilder);
}
```

Lastly you will need to add the database sets for the account, role and join
tables to your database context.

```c#
/// <summary>
/// Returns a set of account instances
/// </summary>
public DbSet<DbAccount> DbAccounts { get; set; }

/// <summary>
/// Returns a set of group instances
/// </summary>
public DbSet<DbRole> DbRoles { get; set; }

/// <summary>
/// Returns accounts linked to roles
/// </summary>
public DbSet<DbAccountRole> DbAccountRoles { get; set; }
```

### Entity Framework Core Migrations

If you are using Entity Framework Core migrations, you can let them create
the tables for the account and role information. With the entities added
to your context and the helper method setting up the model relationships,
you may create migrations as usual. `:-D`

More information on how Entity Framework manages migrations can be found on
the [Entity Framework website][5].

## Using the Library

When you are setting up your services in `Startup.cs` file, you may call our
helper method to configure the repositories provided by this library.

```c#
AccountRepositoryStartupHelper.ConfigureAccountRepository(services);
```

You will now have repositories for managing accounts and roles as well as
for authenticating.

* `IDbRoleRepository, DbRoleRepository` - for managing roles
* `IDbAccountRepository, DbAccountRepository` - for managing accounts
* `IAccountRepository, Repositories.AccountRepository` - for authentication
* `IAccountService, AccountService` - service that handles authentication

The `AccountService` will pull in the repository that implements the 
`IAccountRepository` interface, that will be the repository that uses 
 your database for authentication.

# Documentation

This project uses [Doxygen](http://www.doxygen.nl/) for documentation. Doxygen 
will collect inline comments from your code, along with any accompanying README 
files, and create a documentation website for the project. If you do not have 
Doxygen installed, you can download it from their website and place it on your 
path. To run Doxygen...

    $ cd src
    $ doxygen

The documentation will be written to the `doc/html` folder at the root of the 
project, you can read this documentation with your web browser.

----
[0]: https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-overview
[1]: https://www.nuget.org/packages/Nervestaple.DbAccountRepository/
[5]: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
