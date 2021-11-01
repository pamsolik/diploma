using System.Collections.Generic;
using Cars.Models.DataModels;
using Microsoft.AspNetCore.JsonPatch.Helpers;

namespace Cars.Models.SonarQubeDataModels
{
    public class ProjectCreate
    {
        public Component Project { get; set; }
    }
}