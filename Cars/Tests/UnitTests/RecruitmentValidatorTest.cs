using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using Core.Dto;
using Core.Exceptions;
using Services.Validators;
using Xunit;
namespace CarsTests.UnitTests;

public class RecruitmentValidatorTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void AddApplicationDtoValidatorTest(int projects)
    {
        Fixture fixture = new()
        {
            RepeatCount = projects
        };
        var x = fixture.Create<AddApplicationDto>();
        
        var exception = Record.Exception(() => x.Validate());
        
        if (projects is < 1 or > 5)
            Assert.IsType<AppBaseException>(exception);
        else
            Assert.Null(exception);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void AddAddRecruitmentDtoValidatorTest(int projects)
    {
        Fixture fixture = new()
        {
            RepeatCount = projects
        };
        var x = fixture.Create<AddRecruitmentDto>();
        
        var exception = Record.Exception(() => x.Validate());
        
        if (projects is < 1 or > 5)
            Assert.IsType<AppBaseException>(exception);
        else
            Assert.Null(exception);
    }
}