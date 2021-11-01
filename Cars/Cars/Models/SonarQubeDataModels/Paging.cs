using System.Collections.Generic;
using System.Linq;

namespace Cars.Models.SonarQubeDataModels
{
    public class Paging
    {
        public int PageIndex { get; set; }
        
        public int PageSize { get; set; }
        
        public int Total { get; set; }
    }
}