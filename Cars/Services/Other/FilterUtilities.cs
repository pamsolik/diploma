using System.Linq.Expressions;
using System.Net;
using Core.DataModels;
using Core.Dto;
using Core.Enums;
using Core.Exceptions;
using Geolocation;

namespace Services.Other;

public static class FilterUtilities
{
    public static Expression<Func<City?, bool>> CompareCities(CityDto? city)
    {
        return c => c != null && city != null && c.Name == city.Name;
    }

    public static List<Recruitment> FilterOutAndSortRecruitments(
        ref IQueryable<Recruitment> recruitments, RecruitmentFilterDto filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchString))
            recruitments = recruitments.Where(s =>
                s.Title.Contains(filter.SearchString) || s.Description.Contains(filter.SearchString));

        var filtered = recruitments.AsEnumerable();

        FilterPickedValues(ref filtered, filter.JobLevels, x => (int)x.JobLevel);
        FilterPickedValues(ref filtered, filter.JobTypes, x => (int)x.JobType);
        FilterPickedValues(ref filtered, filter.TeamSizes, x => (int)x.TeamSize);

        filtered = filter.SortOrder switch
        {
            SortOrder.NameAsc => filtered.OrderBy(s => s.Title),
            SortOrder.NameDesc => filtered.OrderByDescending(s => s.Title),
            SortOrder.DateAddedAsc => filtered.OrderBy(s => s.StartDate),
            SortOrder.DateAddedDesc => filtered.OrderByDescending(s => s.StartDate),
            SortOrder.Closest => filtered.OrderByDescending(s =>
            {
                if (filter.City != null) return CalculateDistance(s, filter.City.Latitude, filter.City.Longitude);
                throw new AppBaseException(HttpStatusCode.BadRequest, "City not proviced");
            }),
            _ => recruitments.OrderBy(s => s.Title)
        };

        return filtered.ToList();
    }

    private static double CalculateDistance(Recruitment recruitment, double latitude, double longitude)
    {
        if (recruitment.City is null) return 0;
        var sCoord = new Coordinate(recruitment.City.Latitude, recruitment.City.Longitude);
        var eCoord = new Coordinate(latitude, longitude);
        return GeoCalculator.GetDistance(sCoord, eCoord);
    }

    private static void FilterPickedValues(ref IEnumerable<Recruitment> list,
        IReadOnlyList<bool?> filter, Func<Recruitment, int> predicate)
    {
        if (filter.Any(f => f == true))
            list = list.Where(li =>
            {
                var val = predicate(li);
                return filter.Count > val && filter[val] == true;
            });
    }
}