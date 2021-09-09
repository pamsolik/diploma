﻿using Cars.Data;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarsTests
{
    public class BaseUnitTest
    {
        protected static ApplicationDbContext SetupDbContext()
        {
            //TODO: Base settings
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb")  
                .Options;
            
            var storeOptions = new OperationalStoreOptions {
                //populate needed members
            };
            
            var sOptions = Options.Create(storeOptions);
            
           var context = new ApplicationDbContext(options, sOptions);
            
           context.Database.EnsureDeleted();
           
           return context;
        }
    }
}