using System;
using System.Linq;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarsTests
{
    public class MockingTest : BaseUnitTest
    {
        [Fact]
        public async Task MockingTest1()
        {
            var dbContext = SetupDbContext();

            await dbContext.Recruitments.AddRangeAsync(
                new Recruitment
                {
                    Id = 1,
                    Title = "Rec 1",
                    ShortDescription = "a",
                    Description = "a",
                    StartDate = DateTime.UtcNow,
                    Status = RecruitmentStatus.Open,
                    Type = RecruitmentType.Open,
                    JobType = JobType.Hybrid,
                    JobLevel = JobLevel.Junior,
                    TeamSize = TeamSize.From10To100,
                    Field = "A",
                    ClauseRequired = "A"
                },
                new Recruitment
                {
                    Id = 2,
                    Title = "Rec 2",
                    ShortDescription = "b",
                    Description = "b",
                    StartDate = DateTime.UtcNow,
                    Status = RecruitmentStatus.Open,
                    Type = RecruitmentType.Open,
                    JobType = JobType.Hybrid,
                    JobLevel = JobLevel.Junior,
                    TeamSize = TeamSize.From10To100,
                    Field = "b",
                    ClauseRequired = "b"
                }
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