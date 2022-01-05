using Core.DataModels;
using Core.Dto;
using Core.Enums;
using Core.View;

namespace Services.Interfaces;

public interface IRecruitmentService
{
    Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string? recruiterId);
    Task<int> EditRecruitment(EditRecruitmentDto editRecruitmentDto);

    Task<bool> CloseRecruitment(CloseRecruitmentDto closeRecruitmentDto);

    Task<bool> HideRecruitment(int id);

    Task<bool> UnHideRecruitment(int id);

    Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId);

    PaginatedList<RecruitmentView> GetRecruitmentsFiltered(RecruitmentFilterDto filter,
        RecruitmentMode recruitmentMode, string? userId = "");

    Task<RecruitmentApplication> AddApplication(AddApplicationDto addApplicationDto, string? applicantId);

    Task<List<ApplicationView>> GetApplications(int recruitmentId);
}