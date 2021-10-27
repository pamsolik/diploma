﻿using Microsoft.AspNetCore.JsonPatch.Helpers;

namespace Cars.Models.SonarQubeDataModels
{
    public class CodeAnalysis
    {
        public Component Component { get; set; }

        public float? GetValue(string param) => Component.GetMeasure(param)?.GetValue();
    }
}