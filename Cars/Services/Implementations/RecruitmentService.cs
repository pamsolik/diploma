using System.Net;
using Core.DataModels;
using Core.Dto;
using Core.Enums;
using Core.Exceptions;
using Core.View;
using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Services.Interfaces;
using Services.Managers.Interfaces;
using Services.Validators;
using static Services.Other.FilterUtilities;
using static Services.Other.FileService;

namespace Services.Implementations;

public class RecruitmentService : IRecruitmentService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<RecruitmentService> _logger;
    private readonly IRecruitmentManager _recruitmentManager;

    public RecruitmentService(ILogger<RecruitmentService> logger,
        IDateTimeProvider dateTimeProvider, IRecruitmentManager recruitmentManager)
    {
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _recruitmentManager = recruitmentManager;
    }

    public async Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string recruiterId)
    {
        addRecruitmentDto.Validate();
        var existingCity = await _recruitmentManager.FindOrCreateCity(addRecruitmentDto.City);

        var dest = addRecruitmentDto.Adapt<Recruitment>();

        dest.City = existingCity;
        dest.RecruiterId = recruiterId;
        dest.StartDate = _dateTimeProvider.GetTimeNow();

        var res = await _recruitmentManager.SaveRecruitment(dest);

        res = await CopyAndSaveThumbnail(addRecruitmentDto, res);
        return res.Entity.Id;
    }

    public async Task<int> EditRecruitment(EditRecruitmentDto editRecruitmentDto)
    {
        var recruitment = await _recruitmentManager.FindById(editRecruitmentDto.Id);

        editRecruitmentDto.Validate(recruitment);

        var existingCity = await _recruitmentManager.FindOrCreateCity(editRecruitmentDto.City);
        UpdateRecruitmentFromDto(editRecruitmentDto, existingCity, recruitment);

        var res = await _recruitmentManager.UpdateRecruitment(recruitment);
        return res.Entity.Id;
    }

    public async Task<bool> CloseRecruitment(CloseRecruitmentDto closeRecruitmentDto)
    {
        var res = await _recruitmentManager.CloseRecruitment(closeRecruitmentDto.RecruitmentId,
            closeRecruitmentDto.RecruitmentsToClose);
        return res is not null;
    }

    public async Task<bool> HideRecruitment(int id)
    {
        await _recruitmentManager.ChangeRecruitmentStatus(id, RecruitmentStatus.Hidden);
        return true;
    }

    public async Task<bool> UnHideRecruitment(int id)
    {
        await _recruitmentManager.ChangeRecruitmentStatus(id, RecruitmentStatus.Open);
        return true;
    }

    public async Task<RecruitmentDetailsView> GetRecruitmentDetails(int recruitmentId)
    {
        var res = await _recruitmentManager.FindById(recruitmentId);
        var dest = res.Adapt<RecruitmentDetailsView>();
        return dest;
    }

    public PaginatedList<RecruitmentView>
        GetRecruitmentsFiltered(RecruitmentFilterDto filter, RecruitmentMode recruitmentMode, string userId = "")
    {
        var recruitments = _recruitmentManager.GetRecruitments(recruitmentMode, userId);

        var recruitmentList = FilterOutAndSortRecruitments(ref recruitments, filter);
        var dest = recruitmentList.Adapt<List<RecruitmentView>>();
        GetDaysAgoDescriptions(ref dest);
        var paginated = PaginatedList<RecruitmentView>.CreateAsync(dest, filter.PageIndex, filter.PageSize);

        return paginated;
    }

    public async Task<RecruitmentApplication> AddApplication(AddApplicationDto addApplicationDto,
        string applicantId)
    {
        addApplicationDto.Validate();

        var recruitment = await _recruitmentManager.FindById(addApplicationDto.RecruitmentId);
        if (recruitment is null) throw new AppBaseException(HttpStatusCode.NotFound, "Recruitment not found");
        if (recruitment.Applications.Any(x => x.ApplicantId == applicantId))
            throw new AppBaseException(HttpStatusCode.Conflict,
                "Applicant has allready applied to this recruitment");

        var dest = addApplicationDto.Adapt<RecruitmentApplication>();
        dest.ApplicantId = applicantId;
        dest.Time = _dateTimeProvider.GetTimeNow();
        var res = await _recruitmentManager.AddApplication(dest);
        res = await _recruitmentManager.CopyAndSaveApplicationFiles(addApplicationDto, res);
        return res.Entity;
    }

    public async Task<List<ApplicationView>> GetApplications(int recruitmentId)
    {
        var res = await _recruitmentManager.GetRecruitmentApplications(recruitmentId);
        var dest = res.Adapt<List<ApplicationView>>();
        return dest;
    }

    private static void UpdateRecruitmentFromDto(EditRecruitmentDto editRecruitmentDto,
        City existingCity, Recruitment recruitment)
    {
        recruitment.City = existingCity;
        recruitment.CityId = existingCity.Id;
        recruitment.Description = editRecruitmentDto.Description;
        recruitment.Field = editRecruitmentDto.Field;
        recruitment.Status = editRecruitmentDto.Status;
        recruitment.Title = editRecruitmentDto.Title;
        recruitment.Type = editRecruitmentDto.Type;
        recruitment.ClauseOpt1 = editRecruitmentDto.ClauseOpt1;
        recruitment.ClauseOpt2 = editRecruitmentDto.ClauseOpt2;
        recruitment.ClauseRequired = editRecruitmentDto.ClauseRequired;
        recruitment.ImgUrl = editRecruitmentDto.ImgUrl;
        recruitment.JobLevel = editRecruitmentDto.JobLevel;
        recruitment.JobType = editRecruitmentDto.JobType;
        recruitment.ShortDescription = editRecruitmentDto.ShortDescription;
        recruitment.TeamSize = editRecruitmentDto.TeamSize;
    }

    private string MoveRecruitmentAndGetUrl(AddRecruitmentDto addRecruitmentDto, EntityEntry<Recruitment> res)
    {
        if (string.IsNullOrEmpty(addRecruitmentDto.ImgUrl)) return ImgPath.PlaceHolder;

        try
        {
            var path = Path.Combine("Resources", "Images", "Thumbnails");
            return MoveAndGetUrl(addRecruitmentDto.ImgUrl, res.Entity.Id.ToString(), path, "Recruitment");
        }
        catch (IOException e)
        {
            _logger.LogInformation(e, "IOException, reverting to the default image");
            return ImgPath.PlaceHolder;
        }
    }

    private async Task<EntityEntry<Recruitment>> CopyAndSaveThumbnail(AddRecruitmentDto addRecruitmentDto,
        EntityEntry<Recruitment> res)
    {
        if (addRecruitmentDto.ImgUrl == ImgPath.PlaceHolder) return res;
        res.Entity.ImgUrl = MoveRecruitmentAndGetUrl(addRecruitmentDto, res);
        res = await _recruitmentManager.UpdateRecruitment(res.Entity);
        return res;
    }

    private void GetDaysAgoDescriptions(ref List<RecruitmentView> dest)
    {
        dest.ForEach(dst => dst.DaysAgo = _dateTimeProvider.GetTimeAgoDescription(dst.StartDate));
    }
}