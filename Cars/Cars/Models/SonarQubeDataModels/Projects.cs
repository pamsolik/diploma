using System.Collections.Generic;
using Cars.Models.DataModels;
using Microsoft.AspNetCore.JsonPatch.Helpers;

namespace Cars.Models.SonarQubeDataModels
{
    public class Projects
    {
        public Paging Paging { get; set; }

        public List<Component> Components { get; set; }
    }
}