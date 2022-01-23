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
        {
            filter.SearchString = filter.SearchString.ToUpper(); 
            recruitments = recruitments.Where(s =>
                s.Title.ToUpper().Contains(filter.SearchString) ||
                s.ShortDescription.ToUpper().Contains(filter.SearchString) ||
                s.Description.ToUpper().Contains(filter.SearchString));
        }

        var filtered = recruitments.ToList();

        FilterPickedValues(ref filtered, filter.JobLevels, x => (int)x.JobLevel);
        FilterPickedValues(ref filtered, filter.JobTypes, x => (int)x.JobType);
        FilterPickedValues(ref filtered, filter.TeamSizes, x => (int)x.TeamSize);
        FilterByDistance(ref filtered, filter);
        
        filtered = filter.SortOrder switch
        {
            SortOrder.NameAsc => filtered.OrderBy(s => s.Title).ToList(),
            SortOrder.NameDesc => filtered.OrderByDescending(s => s.Title).ToList(),
            SortOrder.DateAddedAsc => filtered.OrderBy(s => s.StartDate).ToList(),
            SortOrder.DateAddedDesc => filtered.OrderByDescending(s => s.StartDate).ToList(),
            SortOrder.Closest => filtered.OrderBy(s =>
            {
                if (filter.City != null) return CalculateDistance(s, filter.City.Latitude, filter.City.Longitude);
                throw new AppBaseException(HttpStatusCode.BadRequest, "City not proviced");
            }).ToList(),
            _ => filtered.OrderBy(s => s.Title).ToList()
        };

        return filtered.ToList();
    }

    private static void FilterByDistance(ref List<Recruitment> filtered, RecruitmentFilterDto filter)
    {
        if (filter.City is null || string.IsNullOrEmpty(filter.City.Name)) return;
        var dist = filter.Distance + 10;
        filtered = filter.Distance == 0
            ? filtered
                .Where(r => r.City is not null && r.City.Name == filter.City.Name)
                .ToList()
            : filtered
                .Where(r => r.City is not null && CalculateDistance(r, filter.City.Latitude, filter.City.Longitude) <= dist)
                .ToList();
    }

    private static double CalculateDistance(Recruitment recruitment, double latitude, double longitude)
    {
        if (recruitment.City is null) return 0;
        var sCoord = new Coordinate(recruitment.City.Latitude, recruitment.City.Longitude);
        var eCoord = new Coordinate(latitude, longitude);
        return GeoCalculator.GetDistance(sCoord, eCoord, distanceUnit: DistanceUnit.Kilometers);
    }

    private static void FilterPickedValues(ref List<Recruitment> list,
        IReadOnlyList<bool?> filter, Func<Recruitment, int> predicate)
    {
        var cnt = filter.Count;
        if (filter.Any(f => f == true))
            list = list.Where(li =>
            {
                var val = predicate(li);
                return cnt > val && filter[val] == true;
            }).ToList();
    }
}