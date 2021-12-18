using System.Net;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Exceptions;
using Castle.Core.Internal;

namespace Cars.Services.Validators
{
    public static class RecruitmentValidator
    {
        public static void Validate(this AddApplicationDto applicationDto)
        {
            if (applicationDto.Projects.Count is < 1 or > 5)
                throw new AppBaseException(HttpStatusCode.BadRequest,
                    "There has to be between 1 and 5 projects in application.");
        }
        
        public static void Validate(this AddRecruitmentDto recruitment)
        {
            if (recruitment.Description.IsNullOrEmpty())
                throw new AppBaseException(HttpStatusCode.BadRequest,
                    "Description cannot be empty");
            if (recruitment.ShortDescription.IsNullOrEmpty())
                throw new AppBaseException(HttpStatusCode.BadRequest,
                    "Description cannot be empty");
        }
        
        public static void Validate(this EditRecruitmentDto recruitmentDto, Recruitment recruitment)
        {
            if (recruitment is null)
                throw new AppBaseException(HttpStatusCode.NotFound, $"Recruitment {recruitmentDto.Id} not found.");

            if (recruitmentDto.Description.IsNullOrEmpty())
                throw new AppBaseException(HttpStatusCode.BadRequest,
                    "Description cannot be empty");
            if (recruitmentDto.ShortDescription.IsNullOrEmpty())
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
}