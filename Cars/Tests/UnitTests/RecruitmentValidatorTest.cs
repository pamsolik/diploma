using System;
using AutoFixture;
using Castle.Core.Internal;
using Core.DataModels;
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
    [InlineData("a", "b", "c", false)]
    [InlineData("", "b", "c", false)]
    [InlineData("", "", "c", false)]
    [InlineData("a", "", "", false)]
    [InlineData("a", "", "c", false)]
    [InlineData("a", "b", "", false)]
    [InlineData("", "b", "", false)]
    [InlineData("", "", "", false)]
    [InlineData("", "", "", true)]
    [InlineData("a", "b", "c", true)]
    public void AddAddRecruitmentDtoValidatorTest(string title, string desc, string shortDesc, bool rand)
    {
        Fixture fixture = new();
        var x = fixture.Create<AddRecruitmentDto>();
        if (!rand)
        {
            x.Title = title;
            x.Description = desc;
            x.ShortDescription = shortDesc;
        }
        var exception = Record.Exception(() => x.Validate());
        
        if (x.Title.IsNullOrEmpty() || x.Description.IsNullOrEmpty() || x.ShortDescription.IsNullOrEmpty())
            Assert.IsType<AppBaseException>(exception);
        else
            Assert.Null(exception);
    }
    
    [Theory]
    [InlineData("a", "b", "c", "1", "1", 1)]
    [InlineData("", "b", "c", "1", "1", 1)]
    [InlineData("", "", "c", "1", "1", 1)]
    [InlineData("a", "b", "c", "1", "1", 666)]
    [InlineData("a", "b", "c", "1", "2", 1)]
    [InlineData("a", "b", "c", "", "1", 1)]
    [InlineData("a", "b", "c", "5", "5", 0)]
    [InlineData("a", "", "c","1", "1", 1)]
    [InlineData("a", "b", "", "1", "1", 1)]
    [InlineData("", "b", "", "1", "1", 1)]
    [InlineData("", "", "", "1", "1", 1)]
    [InlineData("a", "b", "c", "1", "1", -5)]
    public void EditAddRecruitmentDtoValidatorTest(string title, string desc, string shortDesc, string r1, string r2, int id)
    {
        Fixture fixture = new();
        
        var x = fixture.Create<EditRecruitmentDto>();
        x.Title = title;
        x.Description = desc;
        x.ShortDescription = shortDesc;
        x.RecruiterId = r1;
        x.Id = id;

        var r = new Recruitment { Id = 1, RecruiterId = r2};
        if (id == -5) r = null;
        var exception = Record.Exception(() => x.Validate(r));
        
        if (x.Title.IsNullOrEmpty() || x.Description.IsNullOrEmpty() || x.ShortDescription.IsNullOrEmpty() || r1 != r2 || id < 1)
            Assert.IsType<AppBaseException>(exception);
        else
            Assert.Null(exception);
    }
}