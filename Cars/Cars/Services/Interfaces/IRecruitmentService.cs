using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Models.View;

namespace Cars.Services.Interfaces
{
    public interface IRecruitmentService
    {
        Task<bool> AddRecruitment(AddRecruitmentDto addRecruitmentDto);
        Task<bool> EditRecruitment(EditRecruitmentDto addRecruitmentDto);
        Task<List<RecruitmentView>> GetRecruitments();

        Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId);
        Task<PaginatedList<RecruitmentView>> GetRecruitmentsFiltered(RecruitmentFilterDto filter);

        Task<bool> AddApplication(AddApplicationDto addApplicationDto);

        Task<List<ApplicationView>> GetApplications(int recruitmentId);
    }
}