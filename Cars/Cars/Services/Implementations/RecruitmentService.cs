using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;
using Cars.Models.Exceptions;
using Cars.Models.View;
using Cars.Services.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using static Cars.Services.Other.FilterUtilities;
using static Cars.Services.Other.FileService;
using Duende.IdentityServer.Extensions;

namespace Cars.Services.Implementations
{
    public class RecruitmentService : IRecruitmentService
    {
        private readonly IRecruitmentManager _recruitmentManager;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<RecruitmentService> _logger;

        public RecruitmentService(ILogger<RecruitmentService> logger,
            IDateTimeProvider dateTimeProvider, IRecruitmentManager recruitmentManager)
        {
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _recruitmentManager = recruitmentManager;
        }

        public async Task<int> AddRecruitment(AddRecruitmentDto addRecruitmentDto, string recruiterId)
        {
            //TODO: check if valid and respond accordingly
            var existingCity = await _recruitmentManager.FindOrCreateCity(addRecruitmentDto.City);

            var dest = addRecruitmentDto.Adapt<Recruitment>();

            dest.City = existingCity;
            dest.RecruiterId = recruiterId;
            dest.StartDate = _dateTimeProvider.GetTimeNow();

            var res = await _recruitmentManager.UpdateRecruitment(dest);

            res = await CopyAndSaveThumbnail(addRecruitmentDto, res);
            return res.Entity.Id;
        }

        public async Task<int> EditRecruitment(EditRecruitmentDto addRecruitmentDto)
        {
            var recruitment = await _recruitmentManager.FindById(addRecruitmentDto.Id);
            if (recruitment is null)
                throw new AppBaseException(HttpStatusCode.NotFound, $"Recruitment {addRecruitmentDto.Id} not found.");

            var dest = addRecruitmentDto.Adapt<Recruitment>(); //TODO: check if valid and respond accordingly
            var res = await _recruitmentManager.UpdateRecruitment(dest);
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

        public async Task<PaginatedList<RecruitmentView>>
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
            var dest = addApplicationDto.Adapt<RecruitmentApplication>(); //TODO: check if valid and respond accordingly
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

        private string MoveRecruitmentAndGetUrl(AddRecruitmentDto addRecruitmentDto, EntityEntry<Recruitment> res)
        {
            if (addRecruitmentDto.ImgUrl.IsNullOrEmpty()) return ImgPath.PlaceHolder;

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

        private void GetDaysAgoDescriptions(ref List<RecruitmentView> dest) =>
            dest.ForEach(dst => dst.DaysAgo = _dateTimeProvider.GetTimeAgoDescription(dst.StartDate));
    }
}