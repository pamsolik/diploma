using System.Collections.Generic;
using System.Threading.Tasks;
using Cars.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Services.Interfaces
{
    public interface IRecruitmentService
    {
        Task<bool> AddRecruitment(AddRecruitmentDto addRecruitmentDto);
    }
}