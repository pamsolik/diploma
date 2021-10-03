using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Cars.Models.View;

namespace Cars.Services.Interfaces
{
    public interface IRecruitmentService
    {
        Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string recruiterId);
        Task<bool> EditRecruitment(EditRecruitmentDto addRecruitmentDto);
        Task<List<RecruitmentView>> GetRecruitments(string userId);
        
        Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId);
        Task<PaginatedList<RecruitmentView>> GetRecruitmentsFiltered(RecruitmentFilterDto filter);

        Task<bool> AddApplication(AddApplicationDto addApplicationDto);

        Task<List<ApplicationView>> GetApplications(int recruitmentId);
    }
}