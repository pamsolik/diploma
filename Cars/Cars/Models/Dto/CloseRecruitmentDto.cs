using System.Collections.Generic;

namespace Cars.Models.Dto;

public class CloseRecruitmentDto
{
    public int RecruitmentId { get; set; }

    public List<RecruitmentToClose> RecruitmentsToClose { get; set; }
}

public class RecruitmentToClose
{
    public int ApplicationId { get; set; }
    public bool Selected { get; set; }
}