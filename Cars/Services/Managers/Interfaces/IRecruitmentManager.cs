using Core.DataModels;
using Core.Dto;
using Core.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Services.Managers.Interfaces;

public interface IRecruitmentManager
{
    public Task<City> FindOrCreateCity(CityDto city);

    public Task<EntityEntry<Recruitment>> UpdateRecruitment(Recruitment recruitment);
    public Task<EntityEntry<Recruitment>> SaveRecruitment(Recruitment recruitment);


    public Task<Recruitment> FindById(int id);

    public Task<Recruitment> CloseRecruitment(int recruitmentId, List<RecruitmentToClose> recruitmentsToClose);

    public Task ChangeRecruitmentStatus(int id, RecruitmentStatus status);

    public Task<EntityEntry<RecruitmentApplication>> AddApplication(RecruitmentApplication application);

    public Task<List<RecruitmentApplication>> GetRecruitmentApplications(int recruitmentId);

    public IQueryable<Recruitment> GetRecruitments(RecruitmentMode recruitmentMode, string userId = "");

    public Task<EntityEntry<RecruitmentApplication>> CopyAndSaveApplicationFiles(
        AddApplicationDto applicationDto,
        EntityEntry<RecruitmentApplication> res);

    public string MoveApplicationFileAndGetUrl(string parameter, int id, string subfolder);
}