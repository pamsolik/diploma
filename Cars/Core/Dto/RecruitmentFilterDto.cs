using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dto;

public class RecruitmentFilterDto
{
    [Required] public int PageSize { get; set; }

    [Required] public int PageIndex { get; set; }

    public string SearchString { get; set; } = string.Empty;

    public List<bool?> JobLevels { get; set; } = new();

    public List<bool?> JobTypes { get; set; } = new();

    public List<bool?> TeamSizes { get; set;} = new();

    public CityDto? City { get; set; }

    public int Distance { get; set; } = 0;
    public SortOrder SortOrder { get; set; } = SortOrder.NameAsc;
}