using System;
using System.Linq;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;

namespace CarsTests
{
    public class MockingTest : BaseUnitTest
    {
        [Fact]
        public async void MockingTest1()
        {
            var dbContext = SetupDbContext();
  
            await dbContext.Recruitments.AddRangeAsync( 
                new Recruitment() { Id = 1, Description = "Desc 1", Title = "Rec 1", Type = RecruitmentType.Open, JobType = "Normal"}, 
                new Recruitment() { Id = 2, Description = "Desc 2", Title = "Rec 2", Type = RecruitmentType.InviteOnly, JobType = "Not Normal"} 
            );  
            await dbContext.SaveChangesAsync();  
  
            //Act  
            var result = await dbContext.Recruitments.Select(p => p).ToArrayAsync();  
  
            //Assert  
            Assert.NotNull(result);  
            Assert.NotEmpty(result);  
        }
        
    }
}