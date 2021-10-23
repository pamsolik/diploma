using System.Collections.Generic;

namespace Cars.Models.SonarQubeDataModels
{
    public class Component
    {
        public string Key { get; set; }
        
        public string Name { get; set; }
        
        public string Qualifier { get; set; }

        public List<Measure> Measures { get; set; }
    }
}