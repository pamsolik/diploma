using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.View;

namespace Cars.Services.Interfaces
{
    public interface IRecruitmentService
    {
        Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string recruiterId);
        Task<bool> EditRecruitment(EditRecruitmentDto addRecruitmentDto);
        Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId);

        Task<PaginatedList<RecruitmentView>> GetRecruitmentsFiltered(RecruitmentFilterDto filter,
            RecruitmentMode recruitmentMode, string userId = "");

        Task<RecruitmentApplication> AddApplication(AddApplicationDto addApplicationDto, string applicantId);

        Task<List<ApplicationView>> GetApplications(int recruitmentId);
    }
}