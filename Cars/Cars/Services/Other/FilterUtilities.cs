﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;
using Cars.Models.DataModels;
using Cars.Models.Dto;
using Cars.Models.Enums;

namespace Cars.Services.Other
{
    public static class FilterUtilities
    {
        public static Expression<Func<City, bool>> CompareCities(CityDto city)
        {
            //TODO: Refactor
            return c => c.Name == city.Name &&
                        c.Latitude == city.Latitude &&
                        c.Longitude == city.Longitude;
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
                    CalculateDistance(s, filter.City.Latitude, filter.City.Longitude)),
                _ => recruitments.OrderBy(s => s.Title)
            };

            return filtered.ToList();
        }

        private static double CalculateDistance(Recruitment recruitment, double latitude, double longitude)
        {
            var sCoord = new GeoCoordinate(recruitment.City.Latitude, recruitment.City.Longitude);
            var eCoord = new GeoCoordinate(latitude, longitude);
            return sCoord.GetDistanceTo(eCoord);
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
}