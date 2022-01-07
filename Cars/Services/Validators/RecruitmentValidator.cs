using System.Net;
using Core.DataModels;
using Core.Dto;
using Core.Exceptions;

namespace Services.Validators;

public static class RecruitmentValidator
{
    public static void Validate(this AddApplicationDto applicationDto)
    {
        if (applicationDto.Projects is { Count: < 1 or > 5 })
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "There has to be between 1 and 5 projects in application.");
    }  

    public static void Validate(this AddRecruitmentDto recruitment)
    {
        if (string.IsNullOrEmpty(recruitment.Title))
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "Description cannot be empty");
        if (string.IsNullOrEmpty(recruitment.Description))
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "Description cannot be empty");
        if (string.IsNullOrEmpty(recruitment.ShortDescription))
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "Description cannot be empty");
    }

    public static void Validate(this EditRecruitmentDto recruitmentDto, Recruitment recruitment)
    {
        if (recruitment is null)
            throw new AppBaseException(HttpStatusCode.NotFound, $"Recruitment {recruitmentDto.Id} not found.");

        if (string.IsNullOrEmpty(recruitmentDto.Description))
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "Description cannot be empty");
        if (string.IsNullOrEmpty(recruitmentDto.ShortDescription))
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "Description cannot be empty");
        if (recruitment.RecruiterId != recruitmentDto.RecruiterId)
            throw new AppBaseException(HttpStatusCode.Forbidden,
                "User is not authorised to edit this recruitment.");
        if (recruitmentDto.Id < 1)
            throw new AppBaseException(HttpStatusCode.BadRequest,
                "Id has to be greater than 0");
    }
}