﻿using System.Collections.Generic;
using System.Linq;

namespace Cars.Models.SonarQubeDataModels
{
    public class ScanComponent
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Qualifier { get; set; }

        public List<Measure> Measures { get; set; }

        public Measure GetMeasure(string key)
        {
            return Measures.FirstOrDefault(measure => measure.Metric.Equals(key));
        }
    }
}