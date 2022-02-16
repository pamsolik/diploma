using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dto;

public class ProjectsFilterDto
{
    [Required] public int PageSize { get; set; }

    [Required] public int PageIndex { get; set; }

    public string SearchString { get; set; } = string.Empty;

    public Technology? Technology { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.NameAsc;
}