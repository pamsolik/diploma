using System;
using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarsTests;

public class BaseUnitTest
{
    protected static ApplicationDbContext SetupDbContext()
    {
        //TODO: Base settings
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("InMemoryDb")
            .Options;

        var storeOptions = new OperationalStoreOptions
        {
            //populate needed members
        };

        var sOptions = Options.Create(storeOptions);

        var context = new ApplicationDbContext(options, sOptions);

        try
        {
            context.Database.EnsureDeleted();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return context;
    }
}