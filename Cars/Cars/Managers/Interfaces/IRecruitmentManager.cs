using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cars.Managers.Interfaces;

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